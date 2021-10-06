using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native.Text
{

    interface ITokenizer
    {
        string[] Execute(string value);
    }

    class Tokenizer : ITokenizer
    {
        private char Separator { get; }
        public Tokenizer(char separator)
            => Separator = separator;

        public string[] Execute(string value) => value.Split(new char[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
    }

    class WhitespaceTokenizer : ITokenizer
    {

        public string[] Execute(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var startTokens = new List<int>();
                var endTokens = new List<int>();
                bool tokenRunning = false;

                for (int i = 0; i < value.Length; i++)
                {
                    if (char.IsLetterOrDigit(value[i]) || char.Parse("-") == value[i])
                    {
                        if (!tokenRunning)
                            startTokens.Add(i);
                        tokenRunning = true;
                    }
                    else if (char.IsWhiteSpace(value[i]))
                    {
                        if (tokenRunning)
                            endTokens.Add(i);
                        tokenRunning = false;
                    }
                }
                if (tokenRunning)
                    endTokens.Add(value.Length);

                var tokens = new List<string>();
                var boundedTokens = startTokens.Zip(endTokens, (start, end) => new { Start = start, End = end });
                foreach (var tokenBoundary in boundedTokens)
                {
                    var substring = value.Substring(tokenBoundary.Start, tokenBoundary.End - tokenBoundary.Start);
                    if (!string.IsNullOrWhiteSpace(substring))
                        tokens.Add(substring.Trim());
                }
                return tokens.ToArray();
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
