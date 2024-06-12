using Microsoft.AspNetCore.Components;

namespace MotRenewalApp.Components.PageComponents;

public partial class ErrorComponent
{
    [Parameter] public string ErrorMessage { get; set; }
}
