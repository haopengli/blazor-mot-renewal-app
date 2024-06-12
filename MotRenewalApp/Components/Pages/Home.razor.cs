using MotRenewalApp.Models;
using MotRenewalApp.Services;

namespace MotRenewalApp.Components.Pages;

public partial class Home
{
    private string registrationNumber;
    private Vehicle vehicle;

    private async Task<Vehicle> FetchMotData()
    {
        if (!string.IsNullOrEmpty(registrationNumber))
        {
            vehicle = await MotService.FetchMotData(registrationNumber);
        }

        return null;
    }

    private Task OnRegistrationNumberChanged(string newRegistrationNumber)
    {
        registrationNumber = newRegistrationNumber;
        return Task.CompletedTask;
    }
}