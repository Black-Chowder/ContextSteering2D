using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Black_Magic.Utils
{
    public static class MyMatrixExtensions
    {
        //Source: https://stackoverflow.com/questions/21986909/convert-multidimensional-array-to-jagged-array-in-c-sharp
        internal static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }

        //Transpose a matrix
        internal static T[,] Transpose<T>(this T[,] matrix)
        {
            T[,] newMatrix = new T[matrix.GetLength(1), matrix.GetLength(0)];

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    newMatrix[y, x] = matrix[x, y];
                }
            }

            return newMatrix;
        }

        //Print contents of a bool matrix
        internal static void Print(this bool[,] matrix)
        {
            //Error Control:
            if (matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0) throw new ArgumentOutOfRangeException(nameof(matrix), " was empty");
            for (int row = 0; row < matrix.GetLength(1); row++)
            {
                for (int col = 0; col < matrix.GetLength(0); col++)
                {
                    System.Diagnostics.Debug.Write((matrix[col, row] ? 1 : 0) + ", ");
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        //Print generic type matrix
        internal static void Print<T>(this T[,] matrix)
        {
            if (matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0) throw new ArgumentOutOfRangeException(nameof(matrix), " was empty");
            for (int row = 0; row < matrix.GetLength(1); row++)
            {
                for (int col = 0; col < matrix.GetLength(0); col++)
                {
                    Debug.Write(matrix[col, row]);
                }
                Debug.WriteLine("");
            }
        }

        //Returns-by-value the given matrix
        internal static T[,] Clone<T>(this T[,] matrix)
        {
            T[,] newMatrix = new T[matrix.GetLength(0), matrix.GetLength(1)];

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    newMatrix[x, y] = matrix[x, y];
                }
            }

            return newMatrix;
        }

        //Converts bool matrix to byte matrix
        internal static byte[,] AsBytes(this bool[,] matrix)
        {
            byte[,] byteMatrix = new byte[matrix.GetLength(0), matrix.GetLength(1)];

            byte one = 1;
            byte zero = 0;
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    byteMatrix[x, y] = matrix[x, y] ? one : zero;
                }
            }
            return byteMatrix;
        }

        //Returns a byte matrix as a bool matrix
        internal static bool[,] AsBools(this byte[,] matrix)
        {
            bool[,] boolMatrix = new bool[matrix.GetLength(0), matrix.GetLength(1)];

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    boolMatrix[x, y] = matrix[x, y] != 0;
                }
            }
            return boolMatrix;
        }

        //Returns the given jagged integer array as a byte matrix
        internal static byte[,] AsByteMatrix(this int[][] jagged)
        {
            byte[,] byteMatrix = new byte[jagged.Length, jagged[0].Length];

            for (int x = 0; x < byteMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < byteMatrix.GetLength(1); y++)
                {
                    byteMatrix[x, y] = (byte)jagged[x][y];
                }
            }

            return byteMatrix;
        }

        //Returns the given jagged integer array as a bool matrix
        internal static bool[,] AsBoolMatrix(this int[][] jagged)
        {
            if (jagged.Length == 0) return null;

            bool[,] boolMatrix = new bool[jagged.Length, jagged[0].Length];

            for (int x = 0; x < boolMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < boolMatrix.GetLength(1); y++)
                {
                    boolMatrix[x, y] = jagged[x][y] != 0;
                }
            }

            return boolMatrix;
        }
    }
}
