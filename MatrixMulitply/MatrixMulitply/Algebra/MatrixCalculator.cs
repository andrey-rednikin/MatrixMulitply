using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MatrixMulitply.Algebra
{
    internal static class MatrixCalculator
    {
        /// <summary>
        /// Multiplies one matrix by another
        /// </summary>
        /// <param name="rowsCountX">The number of rows in the x matrix</param>
        /// <param name="colsCountX">The number of columns in the x matrix</param>
        /// <param name="elemsX">The x matrix elements</param>
        /// <param name="rowsCountY">The number of rows in the y matrix</param>
        /// <param name="colsCountY">The number of columns in the y matrix</param>
        /// <param name="elemsY">The y matrix elements</param>
        /// <param name="elemsResult">Array to store elements of the result matrix</param>
        public static void Multiply(int rowsCountX, int colsCountX, double[] elemsX, 
            int rowsCountY, int colsCountY, double[] elemsY, double[] elemsResult)
        {
            if (elemsX == null)
                throw new ArgumentNullException(nameof(elemsX));

            if (elemsY == null)
                throw new ArgumentNullException(nameof(elemsY));

            if (elemsResult == null)
                throw new ArgumentNullException(nameof(elemsResult));

            if (colsCountX != rowsCountY)
                throw new ArgumentException($"{nameof(colsCountX)} != {nameof(rowsCountY)}");

            if (rowsCountX * colsCountX != elemsX.Length)
                throw new ArgumentException($"{nameof(rowsCountX)} * {nameof(colsCountX)} != '{nameof(elemsX)} length'");

            if (rowsCountY * colsCountY != elemsY.Length)
                throw new ArgumentException($"{nameof(rowsCountY)} * {nameof(colsCountY)} != '{nameof(elemsY)} length'");

            if (rowsCountX * colsCountY != elemsResult.Length)
                throw new ArgumentException($"{nameof(rowsCountX)} * {nameof(colsCountY)} != '{nameof(elemsResult)} length'");

            Array.Clear(elemsResult, 0, elemsResult.Length);

            // Get columns of the second matrix as arrays
            var columnsY = new double[colsCountY][];
            for (int i = 0; i < columnsY.Length; i++)
            {
                var column = new double[rowsCountY];
                GetColumn(i, rowsCountY, colsCountY, elemsY, column);
                columnsY[i] = column;
            }

            var needMultiThread = Settings.ParallelizeOrder <= rowsCountX + colsCountY + colsCountX 
                && 1 < Settings.MaxDegreeOfParallelism;

            if (needMultiThread)
            {
                Parallel.ForEach(
                    Partitioner.Create(0, rowsCountX, Math.Max(1, rowsCountX / Settings.MaxDegreeOfParallelism)),
                    new ParallelOptions
                    {
                        MaxDegreeOfParallelism = Settings.MaxDegreeOfParallelism,
                        TaskScheduler = TaskScheduler.Default,
                    },
                    range => MultiplyCore(range.Item1, range.Item2, rowsCountX, colsCountX, elemsX, columnsY, elemsResult));
            }
            else
                MultiplyCore(0, rowsCountX, rowsCountX, colsCountX, elemsX, columnsY, elemsResult);
        }

        private static void MultiplyCore(int leftBound, int rightBound ,int rowsCountX, int colsCountX, double[] elemsX,
            double[][] columnsY, double[] elemsResult)
        {
            var row = new double[colsCountX];

            // Iteration through the rows of the first matrix
            for (int i = leftBound; i < rightBound; i++)
            {
                GetRow(i, rowsCountX, colsCountX, elemsX, row);
                for (int j = 0; j < columnsY.Length; j++)
                {
                    // Get elements from each row of the first matrix and each column of the second matrix as
                    // arrays (probably, it facilitates better CPU cache usage)
                    // Summarize products of corresponding elements
                    var column = columnsY[j];
                    double sum = 0;
                    for (int elemInd = 0; elemInd < row.Length; elemInd++)
                        sum += row[elemInd] * column[elemInd];
                    
                    // Save sum of products as element of result matrix
                    elemsResult[i * columnsY.Length + j] += sum;
                }
            }
        }

        private static void GetRow(int rowIndex, int rowsCount, int colsCount, double[] matrixElems, double[] rowElems)
            => Array.Copy(matrixElems, rowIndex * colsCount, rowElems, 0, colsCount);

        private static void GetColumn(int colIndex, int rowsCount, int colsCount, double[] matrixElems, double[] columnElems)
        {
            for (int i = 0; i < rowsCount; i++)
                columnElems[i] = matrixElems[(i * colsCount) + colIndex];
        }
    }
}
