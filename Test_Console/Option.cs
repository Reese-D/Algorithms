
    class Option<T>
    {
        T? Data;
        public bool HasValue { get; private set; }

        public Option(T data)
        {
            Data = data;
            HasValue = true;
        }

        public Option()
        {
            HasValue = false;
            Data = default;
        }

        public void SetValue(T data)
        {
            Data = data;
            HasValue = true;
        }

        public static Option<T> Create()
        {
            return new Option<T>();
        }

        public T Value()
        {
            if(!HasValue)
            {
                throw new InvalidOperationException("Can't get value on an option that doesn't have a value");
            }
            return Data;
        }
    }