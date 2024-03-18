using System.Globalization;

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

        private BinaryTreeWithParent<T> DeleteNode()
        {
            bool childSide  = (Parent?.Left == this) ? LEFT : RIGHT;
            if(Left == null && Right == null)
            {
                //we're a leaf, just delete reference to ourself
                if(Parent != null)
                {
                    if(childSide == LEFT)
                    {
                        Parent.Left = null;
                    } else {
                        Parent.Right = null;
                    }
                }
                //we might be root if parent was null, just return ourselves regardless
                return this;
            }

            //we're not a leaf, copy a childs value and delete them instead
            //left side picked arbitrarily, either works.
            if(Left != null)
            {
                Data = Left.Data;
                return Left.DeleteNode();
            } else {
                Data = Right.Data;
                return Right.DeleteNode();
            }
        }
        //Unused: This was an old implementation that works correctly, but is a bit verbose
        private BinaryTreeWithParent<T> DeleteNode_()
        {
            bool childSide  = (Parent?.Left == this) ? LEFT : RIGHT;

            //choose right arbitrarily, could potentially pick on tree height or something
            var chosenChild = Right != null ? Right : Left;
            var oppositeChild = Right != null ? Left : Right;

            //we might have to remove a node to promote chosen child
            BinaryTreeWithParent<T>? removed = chosenChild?.Left;

            if(chosenChild != null)
            {
                chosenChild.Parent = Parent;
                
                if(oppositeChild != null)
                {
                    chosenChild.Left = oppositeChild;
                }
            }
            
            if(oppositeChild != null){oppositeChild.Parent = chosenChild;}
            
            if(Parent != null)
            {
                if(childSide == LEFT)
                {
                    Parent.Left = chosenChild;
                } else {
                    Parent.Right = chosenChild;
                }
            }

            //if we replaced our chosen child's left node in promotion, insert it back in at our chosen child level
            if(removed != null && oppositeChild != null && chosenChild != null)
            {
                chosenChild.Insert(removed.Data);
            }
            
            return this;
        }

        public BinaryTreeWithParent<T>? Delete(T data)
        {
            if(data.CompareTo(Data) == 0)
            {
                return DeleteNode();
            }
            return Right?.Delete(data) ?? Left?.Delete(data);
        }

        public BinaryTreeWithParent<T> Insert( T data)
        {
            var side = data.CompareTo(Data) > 0;
            var child = side == LEFT ? Left : Right;

            if(child != null) {return child.Insert(data);}
            child = new BinaryTreeWithParent<T>(data)
            {
                Parent = this
            };
            if (side == LEFT)
            {
                Left = child;
            } else {
                Right = child;
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