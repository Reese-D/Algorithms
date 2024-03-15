
//NOTE: This can be much more efficiently solved with the 2 pointer approach.
class MinimumSubArray
{

    //if non contigouous
    public int MinNonContiguousSubArrayLen(int target, int[] nums) {
        int[] copy = new int[nums.Length];
        Array.Copy(nums, copy, nums.Length);
        Array.Sort(copy);
        int total = 0;
        int count = 0;
        if(target <= 0){
            return 0;
        }
        for(int i = copy.Length-1; i >= 0; i--)
        {
            total += copy[i];
            count++;
            //Console.WriteLine("index: " + i + ", total: " + total + ", count: " + count);
            if(total >= target)
            {
                return count;
            }
        }

        return 0;
    }

    public int sumWindow(int start, int end, int[] array)
    {
        int total = 0;
        for(int i = start; i < end; i++)
        {
            total += array[i];
        }
        return total;
    }

    public int MinSubArrayLen(int target, int[] nums) {
        if(target <= 0)
        {
            return 0;
        }

        int minPossibleArray = MinNonContiguousSubArrayLen(target, nums);
        //Console.WriteLine("minPossibleArray size: " + minPossibleArray);
        if(minPossibleArray == 1)
        {
            return 1;
        }else if(minPossibleArray == nums.Length)
        {
            return minPossibleArray;
        }

        for(int windowSize = minPossibleArray; windowSize < nums.Length; windowSize++)
        {
            int currentSum = sumWindow(0, windowSize, nums);
            if(currentSum >= target)
            {
                return windowSize;
            }
            //Console.WriteLine("window size: " + windowSize);
            for(int i = 0; i < nums.Length - windowSize; i++)
            {
                currentSum -= nums[i];
                currentSum += nums[i + windowSize];
                //System.Console.WriteLine("currentSum: " + currentSum);
                if(currentSum >= target)
                {
                    return windowSize;
                }
            }
        }
        return 0;
    }
}