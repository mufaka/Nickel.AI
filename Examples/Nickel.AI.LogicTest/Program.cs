using Nickel.AI.Logic;

namespace Nickel.AI.LogicTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
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

                var rainCheck = ModelChecker.ModelCheck(knowledge, rain);
                var noRainCheck = ModelChecker.ModelCheck(knowledge, new Not(rain));

                Console.WriteLine($"Check with rain is {rainCheck}. Check without rain is {noRainCheck}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
