using System;

namespace MatrixMulitply
{
    public static class Settings
    {
        private static int _maxDegreeOfParallelism = Environment.ProcessorCount;
        private static int _parallelizeOrder = 64;
        private static int _maxDecimalPlacesForPrint = 2;

        /// <summary>
        /// How many parallel worker threads will be used when parallelization is available.
        /// Default - number of processor cores
        /// </summary>
        public static int MaxDegreeOfParallelism
        {
            get { return _maxDegreeOfParallelism; }
            set { _maxDegreeOfParallelism = Math.Max(1, Math.Min(1024, value)); }
        }

        /// <summary>
        /// Minimal order of the matrix for use parallel threads during matrix multiplication. 
        /// Default 64, must be at least 3
        /// </summary>
        public static int ParallelizeOrder
        {
            get { return _parallelizeOrder; }
            set { _parallelizeOrder = Math.Max(3, value); }
        }

        /// <summary>
        /// Maximal number of decimal places that matrix elements rounded to during printing. 
        /// Default 2, must be between 2 and 12
        /// </summary>
        public static int MaxDecimalPlacesForPrint
        {
            get { return _maxDecimalPlacesForPrint; }
            set { _maxDecimalPlacesForPrint = Math.Max(2, Math.Min(12, value)); }
        }
    }
}
