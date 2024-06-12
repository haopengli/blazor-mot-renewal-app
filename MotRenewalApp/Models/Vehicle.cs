namespace MotRenewalApp.Models
{
    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string MileageLastMot { get; set; }
    }
}
