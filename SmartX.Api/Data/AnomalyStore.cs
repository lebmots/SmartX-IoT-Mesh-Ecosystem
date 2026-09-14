
using SmartX.Api.Models;

namespace SmartX.Api.Data
{
    public static class AnomalyStore
    {
        public static List<AnomalyRecord> Anomalies { get; }
        = new();
    }
}


