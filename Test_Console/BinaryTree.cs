namespace BinaryTrees
{
    class BinaryTree<T>(T data) where T : IComparable
    {
        public T Data = data;
        public BinaryTree<T>? Left;
        public BinaryTree<T>? Right;
        public const bool RIGHT = true;
        public const bool LEFT = false;

        public static BinaryTree<T> Rotate(BinaryTree<T> node, BinaryTree<T>? parent, bool rotation)
        {
            BinaryTree<T>? child = (rotation == LEFT ? node.Right : node.Left) 
                ?? throw new InvalidOperationException("Cannot rotate a node that doesn't have a child in the direction opposite the rotation");
            
            if (rotation == LEFT) {
                node.Right = child.Left;
                child.Left = node;
            } else {
                node.Left = child.Right;
                child.Right = node;
            }

            if(parent == null) return child;

            if(parent.Left == node)
            {
                parent.Left = child;
            } else {
                parent.Right = child;
            }
            return child;
        }

        public static BinaryTree<T> RotateLeft(BinaryTree<T> node, BinaryTree<T>? parent)
        {
            return Rotate(node, parent, LEFT);
        }
        public static BinaryTree<T> RotateRight(BinaryTree<T> node, BinaryTree<T>? parent)
        {
            return Rotate(node, parent, RIGHT);
        }
        public static BinaryTree<T> RotateLeftRight(BinaryTree<T> node, BinaryTree<T>? parent)
        {
            if(node.Left == null || node.Left.Right == null)
            {
                throw new InvalidOperationException("Cannot LeftRight rotate a node with no left child left-right grandchild");
            }
            Rotate(node.Left, node, LEFT);
            return Rotate(node, parent, RIGHT);
        }

        public static BinaryTree<T> RotateRightLeft(BinaryTree<T> node, BinaryTree<T>? parent)
        {
            if(node.Right == null || node.Right.Left == null)
            {
                throw new InvalidOperationException("Cannot RightLeft rotate a node with no Right child and right-left grandchild");
            }
            Rotate(node.Right, node, RIGHT);
            return Rotate(node, parent, LEFT);
        }
    }
} 

