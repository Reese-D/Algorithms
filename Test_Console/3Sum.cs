/* using System;

TestMethod();
void TestMethod(){
    //var test = new Test();
    //var tokens = test.parseTokens("zz3[5[2[it]]2[abc]]2[ab]c".ToList());
    //Console.WriteLine(new string(tokens.ToArray()));
    //var result = Stamps.MovesToStamp("abca", "aabcaca");
    System.Console.WriteLine("test");
}
class ThreeSum
{

    public static Comparer<(int key, List<int> values)> keyComparer = 
  Comparer<(int key, List<int> values)>.Create((a, b) => a.key.CompareTo(b.key));

    public static IList<IList<int>> ThreeSum(int[] nums) {
        List<(int key, List<int> values)> doubles = [];
        for(int i = 0; i < nums.Length; i++)
        {
            for(int j = i; j < nums.Length; j++)
            {
                doubles.Add((nums[i] + nums[j], [i,j]));
                System.Console.WriteLine(doubles[nums[i] + nums[j]].key);
            }
        }

        doubles.Sort(keyComparer);

        List<IList<int>> result = [];
        for(int i = 0; i < nums.Length; i++)
        {
            System.Console.WriteLine("current i: " + i);
            int matchIndex = doubles.BinarySearch((nums[i] * -1, []), keyComparer);
            System.Console.WriteLine("match index: " + matchIndex);
            if(matchIndex >= 0)
            {
                List<int> matching = doubles.ElementAt(matchIndex).values;
                for(int j = 0; j < 2; j++)
                {
                    System.Console.WriteLine("matched indexes for (" + i + "): " + matching[j]);
                }
                if(!matching.Contains(i))
                {
                    result.Add(matching.Select(x => nums[x]).Append(nums[i]).ToList());
                }
            }
        }
        Func<IList<int>, int> sorter = (theList) =>
        {
            int hash = 0;
            for(int i = 0; i < theList.Count; i++)
            {
                hash = ((int)Math.Pow(10, i)) * theList.ElementAt(i);
            }
            return hash;
        };
        return result.DistinctBy<IList<int>, int>(sorter).ToList();
    }
} */