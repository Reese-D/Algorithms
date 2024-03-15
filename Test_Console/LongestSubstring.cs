class LongestSubstring
{
    public int LengthOfLongestSubstring(string s) {
        int pFirst = 0;
        int pLast = 0;
        int largestSubstringStart = 0;
        int largestSubstringEnd = 0;
        int largestSubstringEncountered = 0;

        if(s.Length == 0)
        {
            return 0;
        }
        Dictionary<char, int> encountered = [];
        while(pLast < s.Length)
        {
            char element = s.ElementAt(pLast);
            if(encountered.ContainsKey(element))
            {
                encountered[element] += 1;
            }else
            {
                encountered[element] = 1;
            }
            
            while(encountered[element] > 1)
            {
                char firstElement = s.ElementAt(pFirst);
                encountered[firstElement]--;
                pFirst++;
            }
            if((pLast - pFirst) > largestSubstringEncountered)
            {
                largestSubstringEncountered = pLast - pFirst;
                //largestSubstringStart = pFirst;
                //largestSubstringEnd = pLast;
            }
            pLast++;
        }
        return largestSubstringEncountered;
        //IEnumerable<char> largestSubstring = "";
        //for(int i = pFirst; i <= pLast; i++)
        //{
        //    largestSubstring = largestSubstring.Append(s.ElementAt(i));
        //}

        //return new string(largestSubstring.ToArray())
    }
}
