namespace Nickel.AI.Logic
{
    public abstract class Sentence
    {
        public static void Validate(Sentence sentence)
        {
            if (!(sentence is Sentence))
            {
                throw new ArgumentException("must be a logical sentence");
            }
        }

        public static string Parenthesize(string s)
        {
            bool Balanced(string str)
            {
                int count = 0;
                foreach (char c in str)
                {
                    if (c == '(')
                    {
                        count++;
                    }
                    else if (c == ')')
                    {
                        if (count <= 0)
                        {
                            return false;
                        }
                        count--;
                    }
                }
                return count == 0;
            }

            if (string.IsNullOrEmpty(s) || s.All(char.IsLetter) ||
                (s[0] == '(' && s[s.Length - 1] == ')' && Balanced(s.Substring(1, s.Length - 2))))
            {
                return s;
            }
            else
            {
                return $"({s})";
            }
        }

        public abstract bool Evaluate(Dictionary<string, bool> model);
        public abstract string Formula();
        public abstract HashSet<string> Symbols();

    }
}
