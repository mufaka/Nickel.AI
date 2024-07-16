namespace Nickel.AI.Desktop.UI
{
    public static class UiMessageConstants
    {
        //--- ChatPanel messages 0x0001 to 0x0020
        /// <summary>
        /// Set the text of the chat panel question and open the panel if closed. Body is of type String.
        /// </summary>
        public const int CHAT_SET_QUESTION = 0x0001;
        /// <summary>
        /// Set text of the chat panel and prompt LLM with that text. Body is of type String.
        /// </summary>
        public const int CHAT_ASK_QUESTION = 0x0002;

        //-- LogPanel messages 0x0021 - 0x0040
        /// <summary>
        /// Opens the log panel. Body is ignored.
        /// </summary>
        public const int LOG_SHOW_LOG = 0x0021;
        /// <summary>
        /// Clears the current log. Body is ignored.
        /// </summary>
        public const int LOG_CLEAR_LOG = 0x0022;
    }
}
