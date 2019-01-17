using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16_Jan_2018_Csharp_Sorting
{
    class Program
    {
        static int[] arrNumbers = new int[100];
        static int[] arrWork = new int[100];
        static Random rand = new Random();
        
        static void Main(string[] args)
        {
            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----ASCENDING SORT INSERTION----");
            insertionSort(arrNumbers);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----DESCENDING SORT INSERTION----");
            insertionSortDesc(arrNumbers);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----ASCENDING SORT MERGE TOP DOWN----");
            TopDownMergeSort( arrNumbers,  arrWork);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----ASCENDING SORT MERGE BOTTOM UP----");
            BottomUpMergeSort( arrNumbers,  arrWork);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine($"The answer is {GetN()}");
            Console.WriteLine($"The new answer is {GetTheN()}");
            
            bool[] A = { true, true, true };
            bool[] B = { true, false, true };
            Console.WriteLine(BinToString(BinaryAdd(A,B)));
        }

        static string BinToString(bool[] v)
        {
            string x = string.Empty;
            foreach (var item in v)
            {
                
                if (item)
                {
                    x = x.Insert(0, "1");
                }
                else
                {
                    x = x.Insert(0, "0");
                }
            }
            return x;
        }

        static bool[] BinaryAdd(bool[] A, bool[] B)
        {
            bool[] result = new bool[A.Length + 1];
            for (int i = 0; i < result.Length-1; i++)
            {
                if (A[i] && B[i])
                {
                    result[i + 1] = true;
                }
                else if (A[i] || B[i])
                {
                    if (result[i])
                    {
                        result[i] = false;
                        result[i + 1] = true;
                    }
                    else
                    {
                        result[i] = true;
                    }
                }
            }
            return result;
        }

        static int GetN()
        {
            for (int n = 1000; n >= 1; n--)
            {
                if (n < 8 * Math.Log(n, 2))
                {
                    return n;
                }
            }
            return 0;
        }

        static float GetTheN()
        {
            for (float i = 1; i < 1000; i = i + 0.01f)
            {
                if (100*i*i < Math.Pow(2.0f,i))
                {
                    return i;
                }
            }
            return 0;
        }

        private static void Randomize()
        {
            for (int i = 0; i < arrNumbers.Length; i++)
            {
                arrNumbers[i] = rand.Next(100);
            }
        }

        static void insertionSort(int[] numbers)
        {
            for (int j = 1; j < numbers.Length; j++)
            {
                int tmp = numbers[j];
                int i = j - 1;
                while (i > -1 && numbers[i] > tmp)
                {
                    numbers[i + 1] = numbers[i];
                    i--;
                   
                }
                numbers[i + 1] = tmp;
            }
        }

        static void insertionSortDesc(int[] numbers)
        {
            for (int j = numbers.Length - 1; j >= 0; j--)
            {
                int tmp = numbers[j];
                int i = j + 1;

                while (i < numbers.Length && numbers[i] > tmp)
                {
                    numbers[i - 1] = numbers[i];
                    i++;

                }
                numbers[i - 1] = tmp;
            }
        }

        // Array A[] has the items to sort; array B[] is a work array.
        static void TopDownMergeSort( int[] A,  int[] B)
        {
            CopyArray( A, 0, A.Length,  B);           // duplicate array A[] into B[]
            TopDownSplitMerge( B, 0, A.Length,  A);   // sort data from B[] into A[]
        }

        // Sort the given run of array A[] using array B[] as a source.
        // iBegin is inclusive; iEnd is exclusive (A[iEnd] is not in the set).
        static void TopDownSplitMerge( int[] B, int iBegin, int iEnd,  int[] A)
        {
            if (iEnd - iBegin < 2)                       // if run size == 1
                return;                                 //   consider it sorted
                                                        // split the run longer than 1 item into halves
            int iMiddle = (iEnd + iBegin) / 2;              // iMiddle = mid point
                                                        // recursively sort both runs from array A[] into B[]
            TopDownSplitMerge( A, iBegin, iMiddle,  B);  // sort the left  run
            TopDownSplitMerge( A, iMiddle, iEnd,  B);  // sort the right run
                                                     // merge the resulting runs from array B[] into A[]
            TopDownMerge( B, iBegin, iMiddle, iEnd,  A);
        }

        //  Left source half is A[ iBegin:iMiddle-1].
        // Right source half is A[iMiddle:iEnd-1   ].
        // Result is            B[ iBegin:iEnd-1   ].
        static void TopDownMerge( int[] A, int iBegin, int iMiddle, int iEnd,  int[] B)
        {
            int i = iBegin, j = iMiddle;

            // While there are elements in the left or right runs...
            for (int k = iBegin; k < iEnd; k++)
            {
                // If left run head exists and is <= existing right run head.
                if (i < iMiddle && (j >= iEnd || A[i] <= A[j]))
                {
                    B[k] = A[i];
                    i = i + 1;
                }
                else
                {
                    B[k] = A[j];
                    j = j + 1;
                }
            }
        }

        static void CopyArray( int[] A, int iBegin, int iEnd,  int[] B)
        {
            for (int k = iBegin; k < iEnd; k++)
                B[k] = A[k];
        }

        // array A[] has the items to sort; array B[] is a work array
        static void BottomUpMergeSort( int[] A,  int[] B)
        {
            // Each 1-element run in A is already "sorted".
            // Make successively longer sorted runs of length 2, 4, 8, 16... until whole array is sorted.
            for (int width = 1; width < A.Length; width = 2 * width)
            {
                // Array A is full of runs of length width.
                for (int i = 0; i < A.Length; i = i + 2 * width)
                {
                    // Merge two runs: A[i:i+width-1] and A[i+width:i+2*width-1] to B[]
                    // or copy A[i:n-1] to B[] ( if(i+width >= n) )
                    BottomUpMerge( A, i, Math.Min(i + width, A.Length), Math.Min(i + 2 * width, A.Length),  B);
                }
                // Now work array B is full of runs of length 2*width.
                // Copy array B to array A for next iteration.
                // A more efficient implementation would swap the roles of A and B.
                CopyArray( B,  A);
                // Now array A is full of runs of length 2*width.
            }
        }

        //  Left run is A[iLeft :iRight-1].
        // Right run is A[iRight:iEnd-1  ].
        static void BottomUpMerge( int[] A, int iLeft, int iRight, int iEnd,  int[] B)
        {
            int i = iLeft, j = iRight;
            // While there are elements in the left or right runs...
            for (int k = iLeft; k < iEnd; k++)
            {
                // If left run head exists and is <= existing right run head.
                if (i < iRight && (j >= iEnd || A[i] <= A[j]))
                {
                    B[k] = A[i];
                    i = i + 1;
                }
                else
                {
                    B[k] = A[j];
                    j = j + 1;
                }
            }
        }

        static void CopyArray( int[] B,  int[] A)
        {
            for (int i = 0; i < A.Length; i++)
                A[i] = B[i];
        }
    }
}
