using System.Diagnostics;

class QuickSort
{
    public static void Sort<T>(T[] arr) where T : IComparable
    {
        Sort(arr, 0, arr.Length - 1);
    }

    private static void Sort<T>(T[] arr, int min, int max) where T : IComparable
    {
        if(max - min < 1) {return;}
        //assume sorting is completely random, so we just grab end as pivot (could grab middle if we thought that it had some sorting already)
        int pivot = max;
        int midpoint = min;
        for(int i = min; i < max; i++)
        {
            if(arr[i].CompareTo(arr[pivot]) < 0)
            {
                (arr[midpoint], arr[i]) = (arr[i], arr[midpoint]);
                midpoint++;
            }
        }
        if(midpoint != min || arr[midpoint].CompareTo(arr[pivot]) > 0)
        {
            (arr[midpoint], arr[pivot]) = (arr[pivot], arr[midpoint]);
        }
        
        Sort(arr, min, midpoint - 1);
        Sort(arr, midpoint + 1, max);
    }
}
class QuickSortTests
{
    public static void TestBasicSort()
    {
        int[] test = [1,5,2,4,9,8,7,3,0,1,2,1];
        QuickSort.Sort(test);
        var result = test.Aggregate("", (a,b) => a + ((a.Length != 0) ? "," : "") + b);
        Console.WriteLine(result);
        Debug.Assert(string.Compare(result, "0,1,1,1,2,2,3,4,5,7,8,9") == 0);  
    }
}
