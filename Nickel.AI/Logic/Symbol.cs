
namespace Nickel.AI.Logic
{
    public class Symbol : Sentence
    {
        public string Name { get; }

        public Symbol(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return obj is Symbol symbol && Name == symbol.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("symbol", Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Evaluate(Dictionary<string, bool> model)
        {
            if (model.TryGetValue(Name, out bool value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException($"variable {Name} not in model");
            }
        }

        public override string Formula()
        {
            return Name;
        }

        public override HashSet<string> Symbols()
        {
            return new HashSet<string> { Name };
        }
    }
}
