using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class DeviceCommandService
    {
        private readonly Stack<DeviceCommandRecord> _undoStack = new();

        private readonly List<DeviceCommandRecord> _history = new();

        private readonly Dictionary<string, bool> _deviceStates =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly object _lock = new();

        private static string GetStateKey(string deviceId, string commandType)
        {
            return $"{deviceId.Trim()}|{commandType.Trim()}";
        }

        public DeviceCommandRecord ExecuteCommand(DeviceCommandRequest request)
        {
            lock (_lock)
            {
                string key = GetStateKey(request.DeviceId, request.CommandType);

                bool previousState = _deviceStates.TryGetValue(key, out bool state)
                ? state
                : false;

                var record = new DeviceCommandRecord
                {
                    DeviceId = request.DeviceId.Trim(),
                    CommandType = request.CommandType.Trim(),
                    PreviousState = previousState,
                    NewState = request.TurnOn
                };

                _deviceStates[key] = request.TurnOn;

                _undoStack.Push(record);
                _history.Add(record);

                return record;
            }
        }

        public DeviceCommandRecord? UndoLastCommand()
        {
            lock (_lock)
            {
                if (_undoStack.Count == 0)
                    return null;

                var record = _undoStack.Pop();

                string key = GetStateKey(record.DeviceId, record.CommandType);

                _deviceStates[key] = record.PreviousState;
                record.WasUndone = true;

                return record;
            }
        }

        public List<DeviceCommandRecord> GetHistory()
        {
            lock (_lock)
            {
                return _history
                .OrderByDescending(c => c.ExecutedAt)
                .ToList();
            }
        }

        public object GetDeviceStates()
        {
            lock (_lock)
            {
                return _deviceStates
                .Select(kvp => new
                {
                    DeviceAndCommand = kvp.Key,
                    IsOn = kvp.Value
                })
                .ToList();
            }
        }

        public int GetUndoCount()
        {
            lock (_lock)
            {
                return _undoStack.Count;
            }
        }
    }
}


