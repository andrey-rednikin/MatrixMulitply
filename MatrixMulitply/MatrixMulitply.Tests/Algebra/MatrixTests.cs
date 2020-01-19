using System;
using Xunit;
using MatrixMulitply.Algebra;

namespace MatrixMulitply.Tests.Algebra
{
    public partial class MatrixTests
    {
        /// <summary>
        /// Checks that attempt to create the matrix from null array throws <c>ArgumentNullException</c>
        /// </summary>
        [Fact]
        public void CreateMatrixFromArray_ArrayIsNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { var matrix = Matrix.FromArray(null); });
        }

        /// <summary>
        /// Checks that attempt to create the matrix from empty array throws <c>ArgumentException</c>
        /// </summary>
        [Fact]
        public void CreateMatrixFromArray_ArrayIsEmpty_ShouldThrowArgumentException()
        {
            double[,] emptyArray = { { } };
            Assert.Throws<ArgumentException>(() => { var matrix = Matrix.FromArray(emptyArray); });
        }

        /// <summary>
        /// Checks that it is possible to create the matrix from the array
        /// </summary>
        [Theory]
        [InlineData("Singular3x3")]
        [InlineData("Singular4x4")]
        [InlineData("Square3x3")]
        [InlineData("Square4x4")]
        [InlineData("Tall3x2")]
        [InlineData("Wide2x3")]
        public void CreateMatrixFromArray_ArrayContainsCorrectData_MatrixCreationSucceeded(string name)
        {
            var matrix = Matrix.FromArray(TestSourceArrays[name]);
            for (var i = 0; i < TestSourceArrays[name].GetLength(0); i++)
            {
                for (var j = 0; j < TestSourceArrays[name].GetLength(1); j++)
                    Assert.Equal(TestSourceArrays[name][i, j], matrix[i, j]);
            }
        }

        /// <summary>
        /// Checks that multiplication of matrix by null matrix throws <c>ArgumentNullException</c>
        /// </summary>
        [Fact]
        public void MultiplyMatrices_OtherMatrixIsNull_ShouldThrowArgumentNullException()
        {
            var matrix = TestMatrices["Singular3x3"];
            Assert.Throws<ArgumentNullException>(() => { var result = matrix.Multiply(null); });
        }

        /// <summary>
        /// Checks that multiplication of matrices with incompatible sizes throws <c>ArgumentException</c>
        /// </summary>
        [Fact]
        public void MultiplyMatrices_MatricesHaveIncompatibleSizes_ShouldThrowArgumentException()
        {
            var x = TestMatrices["Singular3x3"];
            var y = TestMatrices["Wide2x3"];
            Assert.Throws<ArgumentException>(() => { var result = x.Multiply(y); });
        }

        /// <summary>
        /// Checks that it is possible to multiply the matrix by the matrix
        /// </summary>
        [Theory]
        [InlineData("Singular3x3", "Square3x3")]
        [InlineData("Singular4x4", "Square4x4")]
        [InlineData("Wide2x3", "Square3x3")]
        [InlineData("Wide2x3", "Tall3x2")]
        [InlineData("Tall3x2", "Wide2x3")]
        public virtual void MultiplyMatrices_MatricesAreCorrect_MultiplicationSucceeded(string xName, string yName)
        {
            var x = TestMatrices[xName];
            var y = TestMatrices[yName];
            var result = x.Multiply(y);

            Assert.Equal(result.RowCount, x.RowCount);
            Assert.Equal(result.ColumnCount, y.ColumnCount);

            for (var i = 0; i < result.RowCount; i++)
            {
                for (var j = 0; j < result.ColumnCount; j++)
                {
                    var expected = MultiplyMatricesRowByCol(x, y, i, j);
                    var real = result[i, j];
                    if (!DoublesAlmostEqual(expected, real, decimalPlaces: 12))
                        Assert.True(false, $"Not equal. Expected:{expected}; real:{real}");
                }
            }
        }
    }
}
