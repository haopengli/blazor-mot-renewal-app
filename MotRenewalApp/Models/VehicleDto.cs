namespace MotRenewalApp.Models
{
    public class VehicleDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string PrimaryColour { get; set; }
        public List<MotTestDto> MotTests { get; set; }
    }

    public class MotTestDto
    {
        public string ExpiryDate { get; set; }
        public string OdometerValue { get; set; }
    }
}
