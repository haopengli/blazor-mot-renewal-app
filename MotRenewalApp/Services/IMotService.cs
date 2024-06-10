using MotRenewalApp.Models;

namespace MotRenewalApp.Services
{
    public interface IMotService
    {
        Task<Vehicle> FetchMotData(string registrationNumber);
    }
}
