using Microsoft.AspNetCore.Components;

namespace MotRenewalApp.Components.Pages;

public partial class RegistrationForm
{
    private string _registrationNumber;

    [Parameter]
    public string RegistrationNumber
    {
        get => _registrationNumber;
        set
        {
            if (RegistrationNumber == value) return;
            _registrationNumber = value;
            RegistrationNumberChanged.InvokeAsync(value);
        }
    }

    [Parameter]
    public EventCallback<string> RegistrationNumberChanged { get; set; }

    [Parameter]
    public EventCallback OnCheckMotClicked { get; set; }
}
