

class HeapSortExample
{
    public static void PrintExample()
    {
        //there's really nothing to this, once we have a heap implemented we just insert into it and then remove everything to get it in order.
        List<int> sorted = [];
        List<int> unsorted = [1,5,233,43,6,7,12,3523,643,4,213,1,5,23,2,2,9,0,5,3];

        Heap<int> myHeap = new((a,b) => a < b);
        for(int i = 0; i < unsorted.Count; i++)
        {
            myHeap.Insert(unsorted[i]);
        }
        for(int i = 0; i < unsorted.Count; i++)
        {
            sorted.Add(myHeap.Delete());
        }

        //print out to verify
        for(int i = 0 ; i < sorted.Count; i++)
        {
            System.Console.Write(sorted[i] + ",");
        }
        System.Console.Write("\r\n");
    }
}