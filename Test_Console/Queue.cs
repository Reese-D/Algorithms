using System.Data.SqlTypes;

namespace Queue
{

    class Queue<T>() where T : INullable
    {
        public int Count{ get; private set; } = 0;
        private class DoubleLinkedNode(T data)
        {
            public T Data = data;
            public DoubleLinkedNode? Next;
            public DoubleLinkedNode? Previous;
        }

        DoubleLinkedNode? Head;
        DoubleLinkedNode? Tail;

        //always insert to front (Could be back, doesn't really matter, as long as we delete in reverse)
        public void Enqueue(T data)
        {
            if(Head == null)
            {
                Head = new DoubleLinkedNode(data);
                Tail = Head;
            }
            Head.Previous = new DoubleLinkedNode(data)
            {
                Next = Head
            };
            Head = Head.Previous;
            Count++;
        }

        public Option<T> Dequeue()
        {
            if(Count == 0)
            {
                throw new InvalidOperationException("Cannot call dequeue on an empty queue");
            }
            Option<T> result;
            if(Tail == null) {
                result = new Option<T>();
            } else {
                result = new Option<T>(Tail.Data);
            }

            Tail = Tail?.Previous;
            if(Tail == null){Head = null;}
            Count--;
            return result;
        }
    }


}
