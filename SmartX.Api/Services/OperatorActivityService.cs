namespace SmartX.Api.Services
{
    public class OperatorActivityService
    {
        private readonly Dictionary<string, int> _deviceSearchCounts =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, int> _commandCounts =
        new(StringComparer.OrdinalIgnoreCase);

        private readonly object _lock = new();

        public void RecordSearch(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                return;

            lock (_lock)
            {
                string key = deviceId.Trim();

                _deviceSearchCounts[key] =
                _deviceSearchCounts.GetValueOrDefault(key) + 1;
            }
        }

        public void RecordCommand(string deviceId, string commandType)
        {
            if (string.IsNullOrWhiteSpace(deviceId) ||
            string.IsNullOrWhiteSpace(commandType))
                return;

            lock (_lock)
            {
                string key = $"{deviceId.Trim()}|{commandType.Trim()}";

                _commandCounts[key] =
                _commandCounts.GetValueOrDefault(key) + 1;
            }
        }

        public Dictionary<string, int> GetSearchCounts()
        {
            lock (_lock)
            {
                return new Dictionary<string, int>(
                _deviceSearchCounts,
                StringComparer.OrdinalIgnoreCase);
            }
        }

        public Dictionary<string, int> GetCommandCounts()
        {
            lock (_lock)
            {
                return new Dictionary<string, int>(
                _commandCounts,
                StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}




