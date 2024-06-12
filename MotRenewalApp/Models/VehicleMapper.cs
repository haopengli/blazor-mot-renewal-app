using System.Globalization;

namespace MotRenewalApp.Models
{
    public static class VehicleMapper
    {
        public static Vehicle MapToVehicle(VehicleDto vehicleDto)
        {
            MotTestDto latestMot = vehicleDto.MotTests.OrderByDescending(mot => mot.ExpiryDate).FirstOrDefault();
            return new Vehicle
            {
                Make = vehicleDto.Make,
                Model = vehicleDto.Model,
                Colour = vehicleDto.PrimaryColour,
                ExpiryDate = DateTime.ParseExact(latestMot.ExpiryDate, "yyyy.MM.dd", CultureInfo.InvariantCulture),
                MileageLastMot = latestMot.OdometerValue
            };
        }

        public static List<Vehicle> MapToVehicles(List<VehicleDto> vehicleDtos)
        {
            return vehicleDtos?.Select(MapToVehicle).ToList() ?? [];
        }
    }

}
