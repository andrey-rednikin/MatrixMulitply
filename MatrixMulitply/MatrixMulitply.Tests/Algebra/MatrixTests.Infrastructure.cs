using System;
using System.Collections.Generic;

using MatrixMulitply.Algebra;

namespace MatrixMulitply.Tests.Algebra
{
    public partial class MatrixTests
    {
        protected Dictionary<string, double[,]> TestSourceArrays { get; set; }

        protected Dictionary<string, Matrix> TestMatrices { get; set; }

        public MatrixTests()
        {
            SetupTestData();
        }

        private void SetupTestData()
        {
            TestSourceArrays = new Dictionary<string, double[,]>
            {
                { "Singular3x3", new[,] { { 1.0, 1.0, 2.0 }, { 1.0, 1.0, 2.0 }, { 1.0, 1.0, 2.0 } } },
                { "Square3x3", new[,] { { -1.1, -2.2, -3.3 }, { 0.0, 1.1, 2.2 }, { -4.4, 5.5, 6.6 } } },
                { "Square4x4", new[,] { { -1.1, -2.2, -3.3, -4.4 }, { 0.0, 1.1, 2.2, 3.3 }, { 1.0, 2.1, 6.2, 4.3 }, { -4.4, 5.5, 6.6, -7.7 } } },
                { "Singular4x4", new[,] { { -1.1, -2.2, -3.3, -4.4 }, { -1.1, -2.2, -3.3, -4.4 }, { -1.1, -2.2, -3.3, -4.4 }, { -1.1, -2.2, -3.3, -4.4 } } },
                { "Tall3x2", new[,] { { -1.1, -2.2 }, { 0.0, 1.1 }, { -4.4, 5.5 } } },
                { "Wide2x3", new[,] { { -1.1, -2.2, -3.3 }, { 0.0, 1.1, 2.2 } } },
                { "Symmetric3x3", new[,] { { 1.0, 2.0, 3.0 }, { 2.0, 2.0, 0.0 }, { 3.0, 0.0, 3.0 } } }
            };

            TestMatrices = new Dictionary<string, Matrix>();
            foreach (var name in TestSourceArrays.Keys)
                TestMatrices.Add(name, Matrix.FromArray(TestSourceArrays[name]));
        }

        private static double MultiplyMatricesRowByCol(Matrix x, Matrix y, int xRowInd, int yColInd)
        {
            double result = 0;
            for (int i = 0; i < x.ColumnCount; i++)
                result += x[xRowInd, i] * y[i, yColInd];

            return result;
        }

        private static bool DoublesAlmostEqual(double a, double b, int decimalPlaces)
        {
            if (double.IsNaN(a) && double.IsNaN(b))
                return true;

            if (double.IsNaN(a) || double.IsNaN(b))
                return false;

            if (double.IsInfinity(a) || double.IsInfinity(b))
                return a == b;

            var diff = a - b;

            // The values are equal if the difference between the two numbers is smaller than
            // 10^(-numberOfDecimalPlaces). We divide by two so that we have half the range
            // on each side of the numbers, e.g. if decimalPlaces == 2,
            // then 0.01 will equal between 0.005 and 0.015, but not 0.02 and not 0.00
            return Math.Abs(diff) < Math.Pow(10, -decimalPlaces) / 2d;
        }
    }
}