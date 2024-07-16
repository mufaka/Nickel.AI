namespace Nickel.AI.Desktop.Models
{
    public class DataProject
    {
        public string Name { get; set; } = String.Empty;

        // NOTE: Eventually different IDataLoader and IDataStorage implementations will need different parameters but
        //       not trying to over-engineer this (yet). The following two properties should just be a config dictionary
        //       and IDataLoader and IDataStorage should accept that instead of named properties. IDataLoader and IDataStorage
        //       should also expose the properties they are looking for in order to properly present a dialog for
        //       configuration. Deferring doing this "the right way" until other IDataLoader or IDataStorage implementations are
        //       done. (Technical Debt).
        public string SourcePath { get; set; } = String.Empty;
        public bool SourceHasHeaderRow { get; set; } = true;
        public string DestinationPath { get; set; } = String.Empty;
        public int FrameSize { get; set; } = 1000;

        public bool IsValid()
        {
            // NOTE: IDataLoader and IDataFrameStorage should validate beyond this.
            return !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(SourcePath) && !String.IsNullOrEmpty(DestinationPath);
        }
    }
}
