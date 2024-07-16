using System.Collections.Concurrent;

namespace Nickel.AI.Desktop.UI
{
    public class MessageQueue
    {
        public static readonly MessageQueue Instance = new MessageQueue();
        // NOTE: This is a thread-safe FIFO queue.
        private ConcurrentQueue<UiMessage> _queue = new ConcurrentQueue<UiMessage>();

        public void Enqueue(UiMessage message)
        {
            _queue.Enqueue(message);
        }

        public void Enqueue(int type, object? body)
        {
            Enqueue(new UiMessage() { MessageType = type, Body = body });
        }

        public UiMessage? Dequeue()
        {
            UiMessage? message;

            // TODO: Should this attempt to dequeue all messages? A fixed amount?
            _queue.TryDequeue(out message);

            return message;
        }
    }
}
