using Microsoft.VisualBasic;

class Node<T>(T data){
    public T Data = data;
    public Node<T>? Next;
}
interface IMergeSortable<T> where T : IComparable
{
    (IMergeSortable<T>? firstHalf, IMergeSortable<T>? secondHalf) Split();
    Node<T>? Head{get; set;}
    void Insert(Node<T> node);
}

class SimpleSingleList<T> : IMergeSortable<T> where T : IComparable
{
    public Node<T>? Head { get; set; }

    public SimpleSingleList(Node<T> existing)
    {
        Head = existing;
    }

    public SimpleSingleList()
    {
        Head = null;
    }

    public void Insert(T data)
    {
        if(Head == null)
        {
            Head = new Node<T>(data);
            return;
        }

        var inserted = new Node<T>(data)
        {
            Next = Head
        };
        Head = inserted;
    }

    public void Insert(Node<T> node)
    {
        if(Head == null)
        {
            Head = node;
            node.Next = null;
            return;
        }

        node.Next = Head;
        Head  = node;
    }

    public void Print()
    {
        var current = Head;
        while(current != null)
        {
            System.Console.Write(current.Data + ",");
            current = current.Next;
        }
        System.Console.Write("\r\n");
    }

    public (IMergeSortable<T> firstHalf, IMergeSortable<T> secondHalf) Split()
    {
        if(Head == null)
        {
            return(new SimpleSingleList<T>(), new SimpleSingleList<T>());
        }
        Node<T> fast = Head;
        Node<T>? slow = Head;
        int count = 0;
        while(fast.Next != null)
        {
            fast = fast.Next;
            count++;
            if(count % 2 == 0)
            {
                slow = slow.Next;
            }
        }
        //slow should now be at midpoint

        //break our list in half
        Node<T> temp = slow;
        slow = slow.Next; //slow may become null here
        temp.Next = null;

        SimpleSingleList<T> secondHalf;

        
        if(slow != null)
        {
            secondHalf = new SimpleSingleList<T>(slow);
        } else{
            secondHalf = new SimpleSingleList<T>();
        }
       return (new SimpleSingleList<T>(Head), secondHalf);
    }
}

class MergeSort
{
    public static IMergeSortable<T>? Sort<T>(IMergeSortable<T> Sortable, bool flipSort = true) where T : IComparable
    {
        (var firstHalf, var secondHalf) = Sortable.Split();

        if(firstHalf != null && firstHalf?.Head?.Next != null) {firstHalf = MergeSort.Sort(firstHalf, flipSort == false);};
        if(secondHalf != null && secondHalf?.Head?.Next != null){secondHalf = MergeSort.Sort(secondHalf, flipSort == false);};

        return Merge(firstHalf, secondHalf, flipSort);
    }

    private static IMergeSortable<T>? Merge<T>(IMergeSortable<T>? firstHalf, IMergeSortable<T>? secondHalf, bool flipSort) where T : IComparable
    {
        if(firstHalf?.Head == null) {return secondHalf;}
        if(secondHalf?.Head == null) {return firstHalf;}

        var first = firstHalf.Head;
        var second = secondHalf.Head;

        //note, inserting will destroy the "next" connection on the node passed in
        //this is necessary, as we'll keep adding to the front of the first list
        //and the existing items will be moved back, so it won't contain the old data at the end
        Node<T>? insert(Node<T> node)
        {
            var next = node.Next;
            firstHalf.Insert(node);
            return next;
        }
        
        //realized too late that single linked list inserts to head, but we traverse towards tail, so our merged lists are backwards...
        //so just flip the comparison every time
        Func<int, bool> compare = (input) =>
        {
            if(!flipSort) return input < 0;
            return input > 0;
        };
        bool firstMove = true;
        while (first != null && second != null)
        {
            if(compare(first.Data.CompareTo(second.Data)))
            {
                first = insert(first);
            } else
            {
                second = insert(second);
            }

            //avoid a cycle
            if(firstMove) {
                firstMove = false;
                firstHalf.Head.Next = null;
            }
        }

        while(first != null){first = insert(first);}
        while(second != null){second = insert(second);}

        return firstHalf;
    }
}