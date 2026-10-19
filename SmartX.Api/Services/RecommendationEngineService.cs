using SmartX.Api.Models;

namespace SmartX.Api.Services
{
    public class RecommendationEngineService
    {
        private readonly TelemetryHistoryService _historyService;
        private readonly ActiveAlertService _alertService;
        private readonly OperatorActivityService _activityService;

        public RecommendationEngineService(
        TelemetryHistoryService historyService,
        ActiveAlertService alertService,
        OperatorActivityService activityService)
        {
            _historyService = historyService;
            _alertService = alertService;
            _activityService = activityService;
        }

        public List<SuggestedAction> GenerateRecommendations()
        {
            var recommendations = new List<SuggestedAction>();

            var history = _historyService.GetHistory();
            var activeAlerts = _alertService.GetActiveAlerts();
            var searchCounts = _activityService.GetSearchCounts();
            var commandCounts = _activityService.GetCommandCounts();

            // Rule 1: Repeated critical temperature readings.
            var temperatureGroups = history
            .Where(r => r.Metric.Equals(
            "Temperature",
            StringComparison.OrdinalIgnoreCase))
            .GroupBy(r => r.DeviceId, StringComparer.OrdinalIgnoreCase)
            .ToList();

            foreach (var group in temperatureGroups)
            {
                int criticalCount = group.Count(r =>
                r.Value < 10 || r.Value > 40);

                if (criticalCount >= 3)
                {
                    recommendations.Add(new SuggestedAction
                    {
                        DeviceId = group.Key,
                        Title = "Recurring Temperature Warning",
                        Description =
                    $"Device {group.Key} has reported " +
                    $"{criticalCount} critical temperature readings.",
                        RecommendedCommand = "Inspect Device",
                        Priority = "High",
                        Reason = "Repeated critical temperature readings"
                    });
                }

                // Rule 2: Elevated recent temperature trend.
                var recentReadings = group
                .OrderByDescending(r => r.ReceivedAt)
                .Take(5)
                .ToList();

                if (recentReadings.Count == 5 &&
                recentReadings.Average(r => r.Value) > 35)
                {
                    recommendations.Add(new SuggestedAction
                    {
                        DeviceId = group.Key,
                        Title = "Elevated Temperature Trend",
                        Description =
                    $"The latest five temperature readings " +
                    $"for {group.Key} average above 35°C.",
                        RecommendedCommand = "Inspect Cooling System",
                        Priority = "Medium",
                        Reason = "Elevated recent temperature average"
                    });
                }
            }

            // Rule 3: Active device alerts.
            foreach (var alert in activeAlerts)
            {
                var parts = alert.Split('|', 2);

                if (parts.Length != 2)
                    continue;

                recommendations.Add(new SuggestedAction
                {
                    DeviceId = parts[0],
                    Title = "Active Device Alert",
                    Description =
                $"Device {parts[0]} has an active {parts[1]} alert.",
                    RecommendedCommand = "Inspect Device",
                    Priority = "High",
                    Reason = $"Active alert: {parts[1]}"
                });
            }

            // Rule 4: Frequently searched devices.
            foreach (var search in searchCounts.Where(s => s.Value >= 3))
            {
                recommendations.Add(new SuggestedAction
                {
                    DeviceId = search.Key,
                    Title = "Frequently Investigated Device",
                    Description =
                $"Operators have searched for {search.Key} " +
                $"{search.Value} times.",
                    RecommendedCommand = "Review Device History",
                    Priority = "Medium",
                    Reason = "Repeated operator searches"
                });
            }

            // Rule 5: Frequently issued manual commands.
            foreach (var command in commandCounts.Where(c => c.Value >= 3))
            {
                var parts = command.Key.Split('|', 2);

                if (parts.Length != 2)
                    continue;

                recommendations.Add(new SuggestedAction
                {
                    DeviceId = parts[0],
                    Title = "Repeated Manual Overrides",
                    Description =
                $"Operators have issued {parts[1]} commands " +
                $"{command.Value} times for {parts[0]}.",
                    RecommendedCommand = $"Inspect {parts[1]}",
                    Priority = "Medium",
                    Reason = "Frequent manual intervention"
                });
            }

            return recommendations
            .OrderBy(r => r.Priority == "High" ? 0 : 1)
            .ThenBy(r => r.DeviceId)
            .ThenBy(r => r.Title)
            .ToList();
        }
    }
}






