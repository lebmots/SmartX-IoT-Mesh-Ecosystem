using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class TelemetryQueueService
    {
        private readonly Queue<QueuedTelemetry> _normalQueue = new();

        private readonly PriorityQueue<QueuedTelemetry, int> _priorityQueue = new();

        private readonly List<QueuedTelemetry> _processedMessages = new();

        private readonly object _lock = new();

        public void Enqueue(QueuedTelemetry message)
        {
            lock (_lock)
            {
                if (message.IsCritical)
                {
                    _priorityQueue.Enqueue(message, 0);
                }
                else
                {
                    _normalQueue.Enqueue(message);
                }
            }
        }

        public QueuedTelemetry? ProcessNext()
        {
            lock (_lock)
            {
                QueuedTelemetry? message = null;

                if (_priorityQueue.Count > 0)
                {
                    message = _priorityQueue.Dequeue();
                }
                else if (_normalQueue.Count > 0)
                {
                    message = _normalQueue.Dequeue();
                }

                if (message != null)
                {
                    _processedMessages.Add(message);
                }

                return message;
            }
        }

        public object GetQueueStatus()
        {
            lock (_lock)
            {
                return new
                {
                    NormalQueued = _normalQueue.Count,
                    CriticalQueued = _priorityQueue.Count,
                    TotalQueued = _normalQueue.Count + _priorityQueue.Count,
                    TotalProcessed = _processedMessages.Count
                };
            }
        }

        public List<QueuedTelemetry> GetProcessedMessages()
        {
            lock (_lock)
            {
                return _processedMessages
                .OrderByDescending(m => m.ReceivedAt)
                .ToList();
            }
        }
    }
}



