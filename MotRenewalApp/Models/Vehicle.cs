namespace MotRenewalApp.Models
{
    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string PrimaryColour { get; set; }
        public List<MotTest> MotTests { get; set; }
    }

    public class MotTest
    {
        public string ExpiryDate { get; set; }
        public string OdometerValue { get; set; }
    }
}
