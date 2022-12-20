using System;
using System.Collections.Generic;

namespace FactChecker.Levenshtein
{

    public class LevenshteinDistanceAlgorithm
    {
        //Create a matrix that returns the distance between two strings
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int LevenshteinDistance_V1(string target, string source)
        {
            double[,] matrix = new double[target.Length + 1, source.Length + 1];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i, 0] = i;
            }

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                matrix[0, i] = i;
            }
            for (int row = 1; row < matrix.GetLength(0); row = row + 1)
            {
                for (int col = 1; col < matrix.GetLength(1); col = col + 1)
                {

                    //charaters are same
                    if (source[col - 1] == target[row - 1])
                    {
                        matrix[row, col] = matrix[row - 1, col - 1];
                    }


                    //charaters are different
                    else
                    {
                        matrix[row, col] = Math.Min(matrix[row, col - 1], Math.Min(
                            matrix[row - 1, col], matrix[row - 1, col - 1])) + 1;
                    }
                }
            }

            double result = matrix[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
            return (int)result;
        }

        static void  print_matrix(int[][] m)
        {
            for (int row = 1; row < m.Length; row++)
            {
                for (int col = 1; col < m[0].Length; col++)
                {
                    Console.Write($"{m[row][col]} ");
                }
                Console.WriteLine($"");
            }
        }

        public static int LevenshteinDistance_V2(string source, string target)
        {
            int[][] matrix = new int[source.Length + 1][];
            for (int i = 0; i
                < matrix.Length; i++)
                matrix[i] = new int[target.Length + 1];

            for (int i = 0; i < source.Length + 1; i++)
                matrix[i][0] = i;
            for (int i = 0; i < target.Length + 1; i++)
                matrix[0][i] = i;
            for (int row = 1; row < source.Length + 1; row++)
                for (int col = 1; col < target.Length + 1; col++)
                {
                    var s = source[row - 1];
                    var t = target[col - 1];
                    //charaters are same
                    if (t == s)
                        matrix[row][ col] = matrix[row - 1][ col - 1];
                    //charaters are different
                    else
                        matrix[row][ col] = Math.Min(matrix[row][ col - 1], Math.Min(
                            matrix[row - 1][ col], matrix[row - 1][ col - 1])) + 1;
                }
            print_matrix(matrix);

            return matrix[source.Length][target.Length];
        }

        
    }
}
