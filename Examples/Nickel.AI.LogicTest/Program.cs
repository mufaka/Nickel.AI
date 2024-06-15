using Nickel.AI.Logic;

namespace Nickel.AI.LogicTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Basic Example");
                Console.WriteLine("".PadLeft(20, '-'));
                BasicExample();
                Console.WriteLine();

                Console.WriteLine("Clue Example");
                Console.WriteLine("".PadLeft(20, '-'));
                ClueExample();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void BasicExample()
        {
            var rain = new Symbol("rain");
            var hagrid = new Symbol("hagrid");
            var dumbledore = new Symbol("dumbledore");

            /*
                Facts:
                    If it didn't rain, Harry visited Hagrid today.
                    Harry visted Hagrid or Dumbledore today, but not both.
                    Harry visited Dumbledore today.
            */
            var knowledge = new And(
                new Implication(new Not(rain), hagrid),
                new Or(hagrid, dumbledore), new Not(new And(hagrid, dumbledore)),
                dumbledore);

            Console.WriteLine(knowledge.Formula());
            Console.WriteLine();

            PrintKnowledgeCheck(knowledge, new List<Symbol>() { hagrid, dumbledore, rain });
        }

        private static void PrintKnowledgeCheck(Sentence knowledge, List<Symbol> symbols)
        {
            Console.WriteLine("Knowledge Check");
            Console.WriteLine("".PadLeft(20, '-'));
            foreach (Symbol symbol in symbols)
            {
                if (ModelChecker.ModelCheck(knowledge, symbol))
                {
                    Console.WriteLine($"{symbol} YES");
                }
                else if (!ModelChecker.ModelCheck(knowledge, new Not(symbol)))
                {
                    // we don't know if it's true and we don't know if it's false
                    Console.WriteLine($"{symbol} MAYBE");
                }
            }
            Console.WriteLine();
        }

        private static void ClueExample()
        {
            // characters
            var mustard = new Symbol("ColMustard");
            var plum = new Symbol("ProfPlum");
            var scarlet = new Symbol("MsScarlet");

            var characters = new List<Symbol>() { mustard, plum, scarlet };

            // rooms
            var ballroom = new Symbol("ballroom");
            var kitchen = new Symbol("kitchen");
            var library = new Symbol("library");

            var rooms = new List<Symbol>() { ballroom, kitchen, library };

            // weapons
            var knife = new Symbol("knife");
            var revolver = new Symbol("revolver");
            var wrench = new Symbol("wrench");

            var weapons = new List<Symbol>() { knife, revolver, wrench };

            var symbols = characters.Concat(rooms).Concat(weapons).ToList();

            // there must be a person, room, and weapon
            var knowledge = new And(
                new Or(mustard, plum, scarlet),
                new Or(ballroom, kitchen, library),
                new Or(knife, revolver, wrench));

            // show formula with only symbols
            Console.WriteLine(knowledge.Formula());
            Console.WriteLine();

            // show current truths
            PrintKnowledgeCheck(knowledge, symbols);

            // player recieves the following cards so they aren't right
            knowledge.Add(new And(new Not(mustard), new Not(kitchen), new Not(revolver)));

            // mustard, kitchen, and revolver shouldn't be in the following now
            PrintKnowledgeCheck(knowledge, symbols);

            // Unknown card (someone guessed scarlet, library, wrench so you know it's not at least one of those)
            knowledge.Add(new Or(
                new Not(scarlet), new Not(library), new Not(wrench)
            ));

            // We know it's not Plum because someone showed the card
            knowledge.Add(new Not(plum));

            // Because we have Col Mustard and someone showed Plum, MsScarlet is the murderer
            PrintKnowledgeCheck(knowledge, symbols);

            // We know it's not the ballroom because someone showed the card
            knowledge.Add(new Not(ballroom));

            // Because we have kitchen and someone showed ballroom, we know it's library
            PrintKnowledgeCheck(knowledge, symbols);

            // We also now know that knife is true ... but how?
            Console.WriteLine(knowledge.Formula());

            /*
                (ColMustard OR ProfPlum OR MsScarlet) 
                AND 
                (ballroom OR kitchen OR library) 
                AND 
                (knife OR revolver OR wrench) 
                AND (
                    (NOT ColMustard) 
                    AND 
                    (NOT kitchen) 
                    AND (NOT revolver)
                ) 
                AND (
                    (NOT MsScarlet) <-- we know MsScarlet did it so this isn't true
                    OR 
                    (NOT library)  <-- we also know that this isn't true
                    OR 
                    (NOT wrench) <-- so this must be true, which means it's not the wrench and we have the revolver, so it's knife.
                ) 
                AND 
                (NOT ProfPlum) 
                AND 
                (NOT ballroom)
            */
        }
    }
}
