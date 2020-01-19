using System;

using MatrixMulitply.Algebra;

namespace MatrixMulitply
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, arguments) =>
            {
                Environment.Exit(0);
            };
            
            Console.WriteLine("Please enter first matrix");
            var x = Matrix.FromArray(ReadMatrix());

            Console.WriteLine();

            Console.WriteLine("Please enter second matrix");
            var y = Matrix.FromArray(ReadMatrix());

            Console.WriteLine();
            Console.WriteLine(x.Multiply(y).ToString());

            Console.ReadLine();
        }

        static double[,] ReadMatrix()
        {
            Console.Write("Enter rows count: ");
            int rowsCount = 0;
            while (!int.TryParse(Console.ReadLine(), out rowsCount) || rowsCount < 1)
                Console.Write("Enter correct value for rows count:");

            Console.Write("Enter columns count: ");
            int colsCount = 0;
            while (!int.TryParse(Console.ReadLine(), out colsCount) || colsCount < 1)
                Console.Write("Enter correct value for colums count:");

            double[,] matrix = new double[rowsCount, colsCount];

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    double element;
                    Console.Write($"Enter element ({i + 1},{j + 1}): ");
                    while (!double.TryParse(Console.ReadLine(), out element))
                        Console.Write($"Enter correct value for element ({i + 1},{j + 1}): ");

                    matrix[i, j] = element;
                }
            }

            return matrix;
        }
    }
}
