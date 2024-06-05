using Microsoft.Data.Analysis;

namespace Nickel.AI.Data
{
    public interface IDataLoader
    {
        /// <summary>
        /// An enumerator for each DataFrame.
        /// </summary>
        /// <returns>An enumerator for the data to be loaded.</returns>
        IEnumerable<DataFrame> LoadData();
    }
}
