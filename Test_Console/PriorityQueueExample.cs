
using System.Diagnostics;
using System.Reflection;

interface IPriorityQueue<T> where T : IComparable
{
    T Insert(T item);
    T Delete();
    T Find();
} 

//Reading through Skiena's algorithm design manual where he proposes PriorityQueue can be implemented with
//the following
//                  Unsorted    Sorted      Balanced
//                  Array       Array       Tree
//------------------------------------------------------
//Insert(Q, x)      O(1)        O(n)        O(log n)
//Find-Minimum(Q)   O(1)        O(1)        O(1)
//Delete-Minimum(Q) O(n)        O(1)        O(log n)

//So here are a few attempts at implementing these as either min or max queues as generic structures

//Note none of these are DRY they're just copy and paste as this is informational, and don't necessarily
//follow all common code practices used in production.

//------------------------------------------------------
// UNSORTED ARRAY
//------------------------------------------------------

//min queue
class MinPriorityQueue<T> : IPriorityQueue<T> where T : IComparable
{
    T[] Data;
    T currentMinimum;
    int currentSize = 0;
    int maxSize = 0;

    public MinPriorityQueue(int initializationSize = 10)
    {
        Array.Resize(ref Data, 10);
        maxSize = initializationSize;
        currentMinimum = default;
    }


    //O(1), while some checks have to be made there's no looping, we always insert to the end
    public T Insert(T item)
    {
        if(currentSize == 0)
        {
            currentMinimum = item;
        } else if(item.CompareTo(currentMinimum) < 0)
        {
            currentMinimum = item;
        }

        if(currentSize + 1 == maxSize)
        {
            maxSize = ArrayHelper.ExpandArray(maxSize, ref Data);
        }
        Data[currentSize] = item;
        currentSize++;

        return item;
    }

    //O(N), we have to go through the whole array to find the value to delete, and then shift all numbers after
    //its position to the left

    //we could probably optimize slightly and keep track of the index so we can skip everything left of it, but
    //since this is just for practice we'll ignore some of those potential optimizations
    public T Delete()
    {
        if(currentSize == 0)
        {
            throw new InvalidOperationException("Can't delete from empty array");
        }
        T newMin = Data[0];
        bool matchingItemFound = false;
        for(int i = 0; i < currentSize; i++)
        {
            if(Data[i].CompareTo(currentMinimum) == 0)
            {
                matchingItemFound = true;
            }
            else if(Data[i].CompareTo(newMin) <= 0)
            {
                newMin = Data[i];
            }

            if(matchingItemFound)
            {
                Data[i] = Data[i+1]; //bubble forwards, copying next value here after comparison
            }
        }
        currentSize--;
        var oldMin = currentMinimum;
        currentMinimum = newMin;
        return oldMin;
    }

    public T Find()
    {
        return currentMinimum;
    }
}

//------------------------------------------------------
// SORTED ARRAY
//------------------------------------------------------

//max queue
class MaxPriorityQueue<T> : IPriorityQueue<T> where T : IComparable
{
    T[] Data;
    int currentSize = 0;
    int maxSize = 0;

    //Identical to MinPriorityQueue
    public MaxPriorityQueue(int initializationSize = 10)
    {
        Array.Resize(ref Data, 10);
        maxSize = initializationSize;
    }


    //O(n), we have to find where to insert and bubble right,
    public T Insert(T item)
    {
        if(currentSize == 0)
        {
            Data[0] = item;
            currentSize++;
            return item;
        }

        if(currentSize + 1 == maxSize)
        {
            ExpandArray();
        }
        
        //basically the same as MinPriorityQueue delete, bubbling opposite direction.

        //note currentSize always points to the next empty spot at the end of the array
        //our first iteration will erase this value which is fine, since it wasn't used.
        for(int i = currentSize; i >= 0; i--)
        {
            if(i == 0)
            {
                Data[i] = item;
                break;
            }
            Data[i] = Data[i-1]; //bubble backwards, copying next value here after comparison

            if(Data[i-1].CompareTo(item) <= 0)
            {
                Data[i] = item;
                break;
            }
            

            
        }

        currentSize++;

        return item;
    }

    //O(1)
    public T Delete()
    {

        if(currentSize == 0)
        {
            throw new InvalidOperationException("can't delete when size is 0");
        }
        currentSize--;
        return Data[currentSize];
    }

    //O(1)
    public T Find()
    {
        if(currentSize == 0)
        {
            throw new InvalidOperationException("can't find when size is 0");
        }
        //note not handling edge cases/bad input, such as 0 sized array
        return Data[currentSize-1];
    }

    private void ExpandArray()
    {
        maxSize *= 2;
        T[] newArray = [];
        Array.Resize(ref newArray, maxSize);
        Array.Copy(Data, newArray, Data.Length);
        Data = newArray;
    }
}




class PriorityQueueTest
{
    public static void TestMin()
    {
        MinPriorityQueue<int> myPriorityQueue = new MinPriorityQueue<int>(10);

        myPriorityQueue.Insert(20);
        Debug.Assert(myPriorityQueue.Find() == 20); 
        myPriorityQueue.Insert(10);
        Debug.Assert(myPriorityQueue.Find() == 10); 
        myPriorityQueue.Insert(99);
        myPriorityQueue.Insert(128);
        myPriorityQueue.Insert(-1);
        myPriorityQueue.Insert(0);
        myPriorityQueue.Insert(15);
        myPriorityQueue.Insert(2);
        myPriorityQueue.Insert(44);
        myPriorityQueue.Insert(232);
        myPriorityQueue.Insert(5345);
        myPriorityQueue.Insert(6);
        myPriorityQueue.Insert(9);
        myPriorityQueue.Insert(24);

        Debug.Assert(myPriorityQueue.Find() == -1); 
        Debug.Assert(myPriorityQueue.Find() == -1);

        Debug.Assert(myPriorityQueue.Delete() == -1);
        Debug.Assert(myPriorityQueue.Find() == 0);
        Debug.Assert(myPriorityQueue.Delete() == 0);
        Debug.Assert(myPriorityQueue.Find() == 2);
    }

    public static void TestMax()
    {
        MaxPriorityQueue<int> myPriorityQueue = new MaxPriorityQueue<int>(10);

        myPriorityQueue.Insert(20);
        Debug.Assert(myPriorityQueue.Find() == 20); 
        myPriorityQueue.Insert(10);
        Debug.Assert(myPriorityQueue.Find() == 20); 
        myPriorityQueue.Insert(99);
        myPriorityQueue.Insert(128);
        myPriorityQueue.Insert(-1);
        myPriorityQueue.Insert(0);
        myPriorityQueue.Insert(15);
        myPriorityQueue.Insert(2);
        myPriorityQueue.Insert(44);
        myPriorityQueue.Insert(232);
        myPriorityQueue.Insert(5345);
        myPriorityQueue.Insert(6);
        myPriorityQueue.Insert(9);
        myPriorityQueue.Insert(24);

        Debug.Assert(myPriorityQueue.Find() == 5345); 
        Debug.Assert(myPriorityQueue.Find() == 5345);

        Debug.Assert(myPriorityQueue.Delete() == 5345);
        Debug.Assert(myPriorityQueue.Find() == 232);
        Debug.Assert(myPriorityQueue.Delete() == 232);
        Debug.Assert(myPriorityQueue.Find() == 128);
    }
}



//TODO implement with binary tree, this time use comparator function so user can sort by min/max or however they wish
/* class PriorityQueue<T> : IPriorityQueue<T> where T : IComparable
{
    //how to compare two items, example input (a, b) picks a on true and b on false
    //find and delete will return the item that would return true for all comparisons
    public PriorityQueue(Func<T, T, bool> Comparator)
    {

    }
    public T Insert(T item)
    {

    }

    public T Delete()
    {

    }

    public T Find()
    {
        
    }
} */