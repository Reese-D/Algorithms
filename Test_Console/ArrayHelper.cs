class ArrayHelper
{
    public static int ExpandArray<T>(int currentSize, ref T[] arrayToExpand)
    {
        //if currentSize * 2 went into the negative...
        if(Math.Max(currentSize * 2, 10) < currentSize)
        {
            throw new ArgumentOutOfRangeException("Cannot insert into Heap anymore, heap is full and can't expand further");
        }

        currentSize = Math.Max(currentSize * 2, 10);
        T[] newArray = [];
        Array.Resize(ref newArray, currentSize);
        Array.Copy(arrayToExpand, newArray, arrayToExpand.Length);
        arrayToExpand = newArray;

        return currentSize;
    }
}