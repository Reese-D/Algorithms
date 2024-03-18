using System.Diagnostics;

class Heap<T> where T : IComparable
{

    Func<T, T, bool> Comparator;
    T[] Nodes;
    int Count = 0;
    int NodeMaxSize = 0;

    public Heap(Func<T, T, bool> comparator)
    {
        Comparator = comparator;
        Nodes = [];
        NodeMaxSize = 1;
    }

    //O(N) worst case we have to expand the array, which means copying everything
    //Average case log(N)
    public void Insert(T data)
    {
        if(Count == NodeMaxSize - 1)
        {
            NodeMaxSize = ArrayHelper.ExpandArray<T>(NodeMaxSize, ref Nodes);
        }
        
        Nodes[Count] = data;
        if(Count == 0) { Count++; return; }

        //if we're not root, compare to our parent node and bubble up the tree if we're a better fit
        BubbleUp(Count);
        Count++;
    }

    //O(log(N))
    public void BubbleUp(int index)
    {
        int parentIndex;
        int previous = index;
        do 
        {
            parentIndex = (previous - 1) / 2;
            
            //swap parent/child
            if(Comparator(Nodes[previous], Nodes[parentIndex])){
                (Nodes[parentIndex], Nodes[previous]) = (Nodes[previous], Nodes[parentIndex]); //tuple swap
            }
            else {
                break;
            }
            previous = parentIndex;
        } while(parentIndex > 0);   
    }

    //O(1)

    //if all items were compared with comparator, returns the item that was always returned true when it was in the first position
    //For example, if the comparator was (a,b) => return a <= b then this would be a MIN heap
    //Best fit is always the first item, in a min heap its the smallest item, in a max heap the largest, etc.
    public T Find()
    {
        return Nodes[0];
    }

    //O(N), heaps aren't binary trees so we can't search through them efficiently to find the key to delete
    public int? FindIndex(T data)
    {
        for(int i = 0; i < Count; i++)
        {
            if(data.CompareTo(Nodes[i]) == 0)
            {
                return i;
            }
        }
        
        return null;
    }

    //O(N), worst case N (last item is deleted, search through whole array to find it)
    //best case log(n), it's the first item and we just bubble down
    public T Delete()
    {
        var result = Nodes[0];
        //swap root and the very last item, then decrement so that the item is outside of bounds. It's now deleted.
        (Nodes[0], Nodes[Count - 1]) = (Nodes[Count - 1], Nodes[0]);
        Count--;

        //our new head however most likely isn't valid, so bubble it down to the correct position
        BubbleDown(0);
        
        return result;
    }

    //O(log(N))
    public void BubbleDown(int index)
    {
        var leftChild = index * 2 + 1;
        var rightChild = index * 2 + 2;

        //if we have no children
        if(leftChild >= Count){return;}
        int dominantchild;
        if(rightChild >= Count)
        {
            dominantchild = leftChild;
        } else{
            dominantchild = Comparator(Nodes[leftChild], Nodes[rightChild]) ? leftChild : rightChild;
        }

        //Left isn't out of bounds, but it's smaller so right still could be.
        if (Comparator(Nodes[dominantchild], Nodes[index]))
        {
            (Nodes[index], Nodes[dominantchild]) = (Nodes[dominantchild], Nodes[index]);
            BubbleDown(dominantchild);
        //likewise if right child doesn't exist, or left child is a beter fit, swap left child.
        }
    }

    public void Print()
    {
        for(int i = 0; i < Count; i++)
        {
            System.Console.Write(Nodes[i] + ", ");
        }
        System.Console.Write("\r\n");
    }
    public string ToString(string delimiter)
    {
        string result = "";
        for(int i = 0; i < Count; i++)
        {   
            result += Nodes[i] + delimiter;
        }
        return new string(result.SkipLast(1).ToArray());
    }
}

class TestHeap{
    public static void Test()
    {
        Func<int, int, bool> myComparator = (a,b) => {return a < b;};
        var myHeap = new Heap<int>(myComparator);

        //test basic insertion
        myHeap.Insert(5);
        myHeap.Insert(12);
        myHeap.Insert(2);
        myHeap.Insert(4);
        myHeap.Insert(19);
        myHeap.Insert(1);
        Debug.Assert(string.Compare(myHeap.ToString(","), "1,4,2,12,19,5") == 0);
        Debug.Assert(myHeap.Find() == 1);

        //test basic deletion
        myHeap.Delete();
        Debug.Assert(string.Compare(myHeap.ToString(","), "2,4,5,12,19") == 0);
        myHeap.Delete();
       
        Debug.Assert(string.Compare(myHeap.ToString(","),  "4,12,5,19") == 0);
        Debug.Assert(myHeap.Find() == 4);

        //test that array works with duplicates
        myHeap.Insert(5);
        Debug.Assert(string.Compare(myHeap.ToString(","), "4,5,5,19,12") == 0);
        myHeap.Insert(12);
        Debug.Assert(string.Compare(myHeap.ToString(","), "4,5,5,19,12,12") == 0);
        myHeap.Insert(2);
        Debug.Assert(string.Compare(myHeap.ToString(","), "2,5,4,19,12,12,5") == 0);
        myHeap.Insert(4);
        Debug.Assert(string.Compare(myHeap.ToString(","), "2,4,4,5,12,12,5,19") == 0);
        Debug.Assert(myHeap.Find() == 2);

        //check array expands properly (no exception thrown)
        myHeap.Insert(19);
        var currentHeap = myHeap.ToString(",");
        Debug.Assert(string.Compare(myHeap.ToString(","),"2,4,4,5,12,12,5,19,19") == 0);
        int currentSize = currentHeap.Length;
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        myHeap.Insert(1);
        Debug.Assert(myHeap.ToString(",").Length == 22 + currentSize);
        Debug.Assert(myHeap.Find() == 1);
    }
}
