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
            insertionSort(ref arrNumbers);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----DESCENDING SORT INSERTION----");
            insertionSortDesc(ref arrNumbers);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----ASCENDING SORT MERGE TOP DOWN----");
            TopDownMergeSort(ref arrNumbers, ref arrWork);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine("----RANDOMIZE----");
            Randomize();
            Console.WriteLine(string.Join(",", arrNumbers));
            Console.WriteLine("----ASCENDING SORT MERGE BOTTOM UP----");
            BottomUpMergeSort(ref arrNumbers, ref arrWork);
            Console.WriteLine(string.Join(",", arrNumbers));

            Console.WriteLine($"The answer is {GetN()}");
            Console.WriteLine($"The new answer is {GetTheN()}");
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

        static int GetTheN()
        {
            for (int i = 1; i < 1000; i++)
            {
                if (100*i*i < Math.Pow(2,i))
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

        static void insertionSort(ref int[] numbers)
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

        static void insertionSortDesc(ref int[] numbers)
        {
            // 1,5,3,4
            for (int j = numbers.Length - 2; j >= 0; j--)
            {
                // j = 2
                int tmp = numbers[j]; // tmp = 3
                int i = j + 1; // i = 3

                while (i < numbers.Length && numbers[i] > tmp)
                {
                    numbers[i - 1] = numbers[i];
                    i++;

                }
                numbers[i - 1] = tmp;
            }
        }

        // Array A[] has the items to sort; array B[] is a work array.
        static void TopDownMergeSort(ref int[] A, ref int[] B)
        {
            CopyArray(ref A, 0, A.Length, ref B);           // duplicate array A[] into B[]
            TopDownSplitMerge(ref B, 0, A.Length, ref A);   // sort data from B[] into A[]
        }

        // Sort the given run of array A[] using array B[] as a source.
        // iBegin is inclusive; iEnd is exclusive (A[iEnd] is not in the set).
        static void TopDownSplitMerge(ref int[] B, int iBegin, int iEnd, ref int[] A)
        {
            if (iEnd - iBegin < 2)                       // if run size == 1
                return;                                 //   consider it sorted
                                                        // split the run longer than 1 item into halves
            int iMiddle = (iEnd + iBegin) / 2;              // iMiddle = mid point
                                                        // recursively sort both runs from array A[] into B[]
            TopDownSplitMerge(ref A, iBegin, iMiddle, ref B);  // sort the left  run
            TopDownSplitMerge(ref A, iMiddle, iEnd, ref B);  // sort the right run
                                                     // merge the resulting runs from array B[] into A[]
            TopDownMerge(ref B, iBegin, iMiddle, iEnd, ref A);
        }

        //  Left source half is A[ iBegin:iMiddle-1].
        // Right source half is A[iMiddle:iEnd-1   ].
        // Result is            B[ iBegin:iEnd-1   ].
        static void TopDownMerge(ref int[] A, int iBegin, int iMiddle, int iEnd, ref int[] B)
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

        static void CopyArray(ref int[] A, int iBegin, int iEnd, ref int[] B)
        {
            for (int k = iBegin; k < iEnd; k++)
                B[k] = A[k];
        }

        // array A[] has the items to sort; array B[] is a work array
        static void BottomUpMergeSort(ref int[] A, ref int[] B)
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
                    BottomUpMerge(ref A, i, Math.Min(i + width, A.Length), Math.Min(i + 2 * width, A.Length), ref B);
                }
                // Now work array B is full of runs of length 2*width.
                // Copy array B to array A for next iteration.
                // A more efficient implementation would swap the roles of A and B.
                CopyArray(ref B, ref A);
                // Now array A is full of runs of length 2*width.
            }
        }

        //  Left run is A[iLeft :iRight-1].
        // Right run is A[iRight:iEnd-1  ].
        static void BottomUpMerge(ref int[] A, int iLeft, int iRight, int iEnd, ref int[] B)
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

        static void CopyArray(ref int[] B, ref int[] A)
        {
            for (int i = 0; i < A.Length; i++)
                A[i] = B[i];
        }
    }
}
