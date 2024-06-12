namespace MotRenewalApp.Models
{
    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
        public List<MotTest> MotTests { get; set; }
    }

    public class MotTest
    {
        public DateTime ExpiryDate { get; set; }
        public string OdometerValue { get; set; }
    }
}
