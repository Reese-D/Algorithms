namespace BinaryTrees
{
    class RedBlackTreeNode<T>(T data) : BinaryTreeWithParent<T>(data) where T : IComparable
    {
        public enum COLOR
        {
            BLACK,
            RED
        }
        public COLOR Color;
    }
}
