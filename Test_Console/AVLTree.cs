using BinaryTrees;

class AVLTree<T> where T : IComparable
{
    private const bool RIGHT = BinaryTreeWithParent<T>.RIGHT;
    private const bool LEFT = BinaryTreeWithParent<T>.LEFT;
    BinaryTreeWithParent<T>? Root;
    public AVLTree(){}
    
    public BinaryTreeWithParent<T> Insert(T data)
    {
        if(Root == null)
        {
            Root = new BinaryTreeWithParent<T>(data);
            return Root;
        }
        var child = BinaryTreeWithParent<T>.Insert(Root, data);
        CheckBalance(child);
        return child;
    }

    private void CheckBalance(BinaryTreeWithParent<T>? node)
    {
        if(node == null) return;
        Rebalance(node);
        CheckBalance(node.Parent);
    }

    private void Rebalance(BinaryTreeWithParent<T> node)
    {
        var difference = BinaryTreeWithParent<T>.GetHeight(node.Left) - BinaryTreeWithParent<T>.GetHeight(node.Right);
        if(Math.Abs(difference) <= 1) {return;}
        //if difference > 1 we must at least have a grandchild, so rotations shouldn't throw.

        var imbalancedSide = difference > 0 ? LEFT : RIGHT;
        var imbalancedChild = imbalancedSide == LEFT ? node.Left : node.Right;
        var imabalancedGrandSide = BinaryTreeWithParent<T>.GetHeight(imbalancedChild?.Left) - BinaryTreeWithParent<T>.GetHeight(imbalancedChild?.Right) > 0 ? LEFT : RIGHT;
        BinaryTreeWithParent<T> result;
        if(imbalancedSide == imabalancedGrandSide == LEFT){
            result = BinaryTreeWithParent<T>.RotateRight(node);
        } else if(imbalancedSide == imabalancedGrandSide == RIGHT){
            result = BinaryTreeWithParent<T>.RotateLeft(node);
        } else if(imbalancedSide == LEFT && imabalancedGrandSide == RIGHT){
            result = BinaryTreeWithParent<T>.RotateLeftRight(node);
        } else {
            result = BinaryTreeWithParent<T>.RotateRightLeft(node);
        }

        //after rotating we will have a new root, returned by the rotation function
        if(node == Root)
        {
            Root = result;
        }
    }

    public void PrintTree()
    {
        PrintTree(Root);
    }

    private static void PrintTree(BinaryTreeWithParent<T>? node, int level = 0)
    {
        if(node == null) {return;}
        System.Console.WriteLine(new string(Enumerable.Repeat('\t', level).ToArray()) + node.Data);
        PrintTree(node.Left, level + 1);
        PrintTree(node.Right, level + 1);
    }
}