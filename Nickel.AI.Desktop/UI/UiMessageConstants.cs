namespace Nickel.AI.Desktop.UI
{
    public static class UiMessageConstants
    {
        //--- ChatPanel messages 0x0001 to 0x0020
        /// <summary>
        /// Set the text of the chat panel question and open the panel if closed.
        /// </summary>
        public static int CHAT_SET_QUESTION = 0x0001;

        //-- LogPanel messages 0x0021 - 0x0040
        /// <summary>
        /// Clears the current log
        /// </summary>
        public static int LOG_CLEAR_LOG = 0x0021;
        public static int LOG_ADD_LOG = 0x0022;
    }
}
