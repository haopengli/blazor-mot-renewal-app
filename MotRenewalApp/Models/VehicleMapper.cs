using System.Globalization;

namespace MotRenewalApp.Models
{
    public static class VehicleMapper
    {
        public static Vehicle MapToVehicle(VehicleDto vehicleDto)
        {
            return new Vehicle
            {
                Make = vehicleDto.Make,
                Model = vehicleDto.Model,
                Colour = vehicleDto.PrimaryColour,
                MotTests = vehicleDto.MotTests?.Select(MapToMotTest).ToList() ?? []
            };
        }

        private static MotTest MapToMotTest(MotTestDto motTestDto)
        {
            return new MotTest
            {
                ExpiryDate = DateTime.ParseExact(motTestDto.ExpiryDate, "yyyy.MM.dd", CultureInfo.InvariantCulture),
                OdometerValue = motTestDto.OdometerValue
            };
        }

        public static List<Vehicle> MapToVehicles(List<VehicleDto> vehicleDtos)
        {
            return vehicleDtos?.Select(MapToVehicle).ToList() ?? [];
        }
    }

}
