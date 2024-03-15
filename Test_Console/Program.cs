// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Metrics;
using System.Text;
using myConsole;


TestMethod();
void TestMethod(){
    //var test = new Test();
    //var tokens = test.parseTokens("zz3[5[2[it]]2[abc]]2[ab]c".ToList());
    //Console.WriteLine(new string(tokens.ToArray()));
    var result = Stamps.MovesToStamp("abca", "aabcaca");
    System.Console.WriteLine(result);

    SkylineProblem.GetSkyline([[1,5,3], [1,5,3], [1,5,3]]);
    PriorityQueueTest.TestMin();
    PriorityQueueTest.TestMax();
}



namespace myConsole
{

    class Test{

        public class CharToken
        {
            public CharToken(IEnumerable<char> value) { Value = value;}
            public IEnumerable<char> Value{get; private set;}
        }

        public class RecursiveToken
        {
            public RecursiveToken(CharToken token, int multiplier = 1)
            {
                Value = string.Concat(Enumerable.Repeat(token.ToString(), multiplier));
            }
            public RecursiveToken(RecursiveToken token, int multiplier)
            {
                Value = string.Concat(Enumerable.Repeat(token.ToString(), multiplier));
            }

            public string Value{get;private set;}
        }

        public IEnumerable<char> parseTokens(IEnumerable<char> input)
        {
            var stringVersion = new String(input.ToArray());
            if(stringVersion == "")
            {
                return [];
            }
            char firstChar = stringVersion.ElementAt(0);
            var newInput = stringVersion.ToList();
            if(char.IsLetter(firstChar))
            {
                (CharToken token, IEnumerable<char> remainder) = parseCharacters(newInput);
                return token.Value.Concat(parseTokens(remainder));
            }
            else if(char.IsDigit(firstChar))
            {
                (RecursiveToken token, IEnumerable<char> remainder) = parseRecursive(newInput);
                return token.Value.Concat(parseTokens(remainder));
            }
            else{
                throw new NotImplementedException();
            }
        }

        public (CharToken token, IEnumerable<char> remainder) parseCharacters(IEnumerable<char> input)
        {
            (var result, var remainder) = TakeWhileBoth(input, char.IsLetter);
            var token = new CharToken(result);
            return(token, remainder);
        }

        public (RecursiveToken token, IEnumerable<char> remainder) parseRecursive(IEnumerable<char> input)
        {
            (var multiplierString, var remainder) = TakeWhileBoth(input, char.IsDigit);
            var multiplier = int.Parse(multiplierString.ToArray());
            int count = 0;
            Func<char, bool> filterBrackets = (char input) =>
            {
                if(input == '[') count++;
                if(input == ']') count--;
                return count > 0;
            };
            (var substring, var remainderSecond) = TakeWhileBoth(remainder, filterBrackets);
            substring = substring.Skip(1); //trim []
            remainderSecond = remainderSecond.Skip(1);

            return (new RecursiveToken(new CharToken(parseTokens(substring)), multiplier), remainderSecond);
        }

        public static (IEnumerable<T> result, IEnumerable<T> remainder) TakeWhileBoth<T>(IEnumerable<T> input, Func<T, bool> filter)
        {
            return (input.TakeWhile(filter), input.SkipWhile(filter));
        }
    }
}


public class Stamps{

    public static int[] MovesToStamp(string stamp, string target) {
        
        var permutations = FindPermutations(stamp.ToList());
        return MovesToStamp(stamp.ToList(), target.ToList(), permutations);

    }

    public static int[] MovesToStamp(List<char> stamp, List<char> target, HashSet<string> permutations) 
    {
        List<int> result = [];
        for(int i = 0; i < target.Count - stamp.Count + 1; i++)
        {
            var current = target.Skip(i).Take(stamp.Count);
            if(permutations.Contains(new string(current.ToArray())))
            {
                for(int j = 0; j < stamp.Count; j++)
                {
                    target[i+j] = '?';
                }
                List<int> currResult = [i];
                result = currResult.Concat(result).ToList();
            }
        }
        Func<char, bool> isQuestionMark = (char input) => {
            return input == '?';
        };
        if(target.Count(isQuestionMark) == target.Count())
        {
            return result.ToArray();
        }
        return MovesToStamp(stamp, target, permutations).Concat(result.ToArray()).ToArray();
    }

    public static HashSet<string> FindPermutations(List<char> input)
    {
        HashSet<string> result = [];
        for(int i = 0; i < input.Count; i++)
        {
            result.Add(new string(Enumerable.Repeat('?', i).Concat(input.Skip(i)).ToArray()));
        }
        for(int i = input.Count - 1; i > 0; i--)
        {
            result.Add(new string(input.SkipLast(i).ToArray().Concat(Enumerable.Repeat('?', i)).ToArray()));
        }
        return result;
    }
}
