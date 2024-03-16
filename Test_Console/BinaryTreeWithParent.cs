namespace BinaryTrees
{
    class BinaryTreeWithParent<T>(T data) where T : IComparable
    {
        public T Data = data;
        public BinaryTreeWithParent<T>? Parent;
        public BinaryTreeWithParent<T>? Left;
        public BinaryTreeWithParent<T>? Right;

        public const bool RIGHT = true;
        public const bool LEFT = false;

        public static BinaryTreeWithParent<T> Rotate(BinaryTreeWithParent<T> node, bool rotation)
        {
            BinaryTreeWithParent<T>? child = (rotation == LEFT ? node.Right : node.Left) 
                ?? throw new InvalidOperationException("Cannot rotate a node that doesn't have a child in the direction opposite the rotation");
            
            var parent = node.Parent;
            bool nodeSide = parent?.Left == node ? LEFT : RIGHT;

            if (rotation == LEFT) {
                if(child.Left != null) child.Left.Parent = node;
                node.Right = child.Left;
                node.Parent = child;
                child.Left = node;
            } else {
                if(child.Right != null) child.Right.Parent = node;
                node.Left = child.Right;
                node.Parent = child;
                child.Right = node;
            }

            child.Parent = parent;
            if(parent == null) return child;

            if(nodeSide == LEFT)
            {
                parent.Left = child;
            } else {
                parent.Right = child;
            }
            return child;
        }

        
        public static int GetHeight(BinaryTreeWithParent<T>? node)
        {
            if(node == null) {return 0;}
            return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        public static BinaryTreeWithParent<T> Insert(BinaryTreeWithParent<T> node, T data)
        {
            var side = data.CompareTo(node.Data) > 0;
            var child = side == LEFT ? node.Left : node.Right;

            if(child != null) {return BinaryTreeWithParent<T>.Insert(child, data);}
            child = new BinaryTreeWithParent<T>(data)
            {
                Parent = node
            };
            if (side == LEFT)
            {
                node.Left = child;
            } else {
                node.Right = child;
            }
            return child;
        }

        public static BinaryTreeWithParent<T> RotateLeft(BinaryTreeWithParent<T> node)
        {
            return Rotate(node, LEFT);
        }
        public static BinaryTreeWithParent<T> RotateRight(BinaryTreeWithParent<T> node)
        {
            return Rotate(node, RIGHT);
        }
        public static BinaryTreeWithParent<T> RotateLeftRight(BinaryTreeWithParent<T> node)
        {
            if(node.Left == null || node.Left.Right == null)
            {
                throw new InvalidOperationException("Cannot LeftRight rotate a node with no left child left-right grandchild");
            }
            Rotate(node.Left, LEFT);
            return Rotate(node, RIGHT);
        }

        public static BinaryTreeWithParent<T> RotateRightLeft(BinaryTreeWithParent<T> node)
        {
            if(node.Right == null || node.Right.Left == null)
            {
                throw new InvalidOperationException("Cannot RightLeft rotate a node with no Right child and right-left grandchild");
            }
            Rotate(node.Right, RIGHT);
            return Rotate(node, LEFT);
        }
    }
}