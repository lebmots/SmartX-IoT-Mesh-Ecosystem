


using SmartX.Api.Data;
using SmartX.Api.Models;

namespace SmartX.Api.Services
{

    // Handles validation and registration of new Smart-X devices.
  
    public class DeviceRegistrationService
    {
        public bool RegisterDevice(
        DeviceRegistrationRequest request,
        out SensorDevice? device,
        out string message)
        {
            device = null;

            if (string.IsNullOrWhiteSpace(request.DeviceId))
            {
                message = "Device ID is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.MacAddress))
            {
                message = "MAC address is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.DeploymentLocation))
            {
                message = "Deployment location is required.";
                return false;
            }

            bool deviceIdExists =
            DeviceStore.Devices.Any(
            d => d.DeviceId.Equals(
            request.DeviceId,
            StringComparison.OrdinalIgnoreCase));

            if (deviceIdExists)
            {
                message = "A device with this Device ID is already registered.";
                return false;
            }

            bool macAddressExists =
            DeviceStore.Devices.Any(
            d => d.MacAddress.Equals(
            request.MacAddress,
            StringComparison.OrdinalIgnoreCase));

            if (macAddressExists)
            {
                message = "A device with this MAC address is already registered.";
                return false;
            }

            int newId =
            DeviceStore.Devices.Count == 0
            ? 1
            : DeviceStore.Devices.Max(d => d.Id) + 1;

            device = new SensorDevice
            {
                Id = newId,
                DeviceId = request.DeviceId.Trim(),
                MacAddress = request.MacAddress.Trim(),
                DeploymentLocation = request.DeploymentLocation.Trim(),
                Category = request.Category,
                LastSeen = DateTime.UtcNow,
                HealthStatus = DeviceHealthStatus.Healthy,
                HealthMessage = "Device registered successfully."
            };

            DeviceStore.Devices.Add(device);

            message = "Device registered successfully.";
            return true;
        }
    }
}




