using System;
using System.Text;

namespace MatrixMulitply.Algebra
{
    public class Matrix
    {
        private readonly int _rowCount;
        private readonly int _columnCount;
        private readonly double[] _elements;

        /// <summary>
        /// Number of rows
        /// </summary>
        public int RowCount => _rowCount;

        /// <summary>
        /// Number of columns
        /// </summary>
        public int ColumnCount => _columnCount;

        /// <summary>
        /// Gets or sets the element at the given row and column
        /// </summary>
        public double this[int rowInd, int colInd]
        {
            get
            {
                ValidateRange(rowInd, colInd);
                return _elements[rowInd * _columnCount + colInd];
            }
            set
            {
                ValidateRange(rowInd, colInd);
                _elements[rowInd * _columnCount + colInd] = value;
            }
        }

        private Matrix(int rowCount, int columnCount)
        {
            if (rowCount < 1)
                throw new ArgumentException($"Invalid value of {nameof(rowCount)}: {rowCount}");
            if (columnCount < 1)
                throw new ArgumentException($"Invalid value of {nameof(columnCount)}: {columnCount}");

            _rowCount = rowCount;
            _columnCount = columnCount;
            _elements = new double[rowCount * columnCount];
        }

        public static Matrix FromArray(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.GetLength(0) < 1 || array.GetLength(1) < 1)
                throw new ArgumentException($"{nameof(array)} is empty");

            var newMatrix = new Matrix(array.GetLength(0), array.GetLength(1));

            for (int i = 0; i < newMatrix._rowCount; i++)
            {
                for (int j = 0; j < newMatrix._columnCount; j++)
                    newMatrix._elements[i * newMatrix._columnCount + j] = array[i, j];
            }

            return newMatrix;
        }

        public override string ToString()
            => $"Matrix {_rowCount} x {_columnCount}:{Environment.NewLine}{StringArrayToString(ToStringArray())}";

        public Matrix Multiply(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (_columnCount != other._rowCount)
                throw new ArgumentException($"Matrix sizes are not compatible");

            var result = new Matrix(_rowCount, other._columnCount);
            MatrixCalculator.Multiply(_rowCount, _columnCount, _elements,
                other._rowCount, other._columnCount, other._elements, 
                result._elements);
            
            return result;
        }

        private string StringArrayToString(string[,] array)
        {
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);

            var colWidths = new int[cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    colWidths[j] = Math.Max(colWidths[j], array[i, j].Length);
            }

            var sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                sb.Append(array[i, 0].PadLeft(colWidths[0]));
                for (int j = 1; j < cols; j++)
                {
                    sb.Append("  ");
                    sb.Append(array[i, j].PadLeft(colWidths[j]));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string[,] ToStringArray()
        {
            string formatMask = $"0.{new string('#', Settings.MaxDecimalPlacesForPrint)}";

            var array = new string[_rowCount, _columnCount];
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                    array[i, j] = _elements[i * _columnCount + j].ToString(formatMask);
            }

            return array;
        }

        private void ValidateRange(int rowInd, int colInd)
        {
            if ((uint)rowInd >= (uint)_rowCount)
                throw new ArgumentOutOfRangeException(nameof(rowInd));

            if ((uint)colInd >= (uint)_columnCount)
                throw new ArgumentOutOfRangeException(nameof(colInd));
        }
    }
}
