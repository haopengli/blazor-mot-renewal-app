using MotRenewalApp.Models;
using MotRenewalApp.Services;

namespace MotRenewalApp.Components.Pages;

public partial class Home
{
    private string registrationNumber;
    private Vehicle vehicle;
    private string errorMessage;

    private async Task<Vehicle> FetchMotData()
    {
        if (!string.IsNullOrEmpty(registrationNumber))
        {
            vehicle = null;

            // Validate registration number format
            if (!System.Text.RegularExpressions.Regex.IsMatch(registrationNumber, @"^[A-Z]{2}[0-9]{2}\s?[A-Z]{3}$"))
            {
                errorMessage = "Registration number is not in the standard format. Please input a valid registration number, for example: AB12 CDE.";
                return null;
            }

            errorMessage = string.Empty;
            try
            {
                var formattedNumber = registrationNumber.Replace(" ", "");
                vehicle = await MotService.FetchMotData(formattedNumber);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        else
        {
            errorMessage = "Please enter your registration number.";
        }

        return null;
    }

    private Task OnRegistrationNumberChanged(string newRegistrationNumber)
    {
        registrationNumber = newRegistrationNumber;
        return Task.CompletedTask;
    }
}