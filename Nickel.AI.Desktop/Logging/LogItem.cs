namespace Nickel.AI.Desktop.Logging
{
    public class LogItem
    {
        public DateTime LogDate { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }

        public LogItem(string level, string message, string detail)
        {
            this.LogDate = DateTime.Now;
            this.Level = level;
            this.Message = message;
            this.Detail = detail;
        }
    }
}
