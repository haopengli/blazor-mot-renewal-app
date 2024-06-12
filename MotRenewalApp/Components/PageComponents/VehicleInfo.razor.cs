using Microsoft.AspNetCore.Components;
using MotRenewalApp.Models;

namespace MotRenewalApp.Components.PageComponents;

public partial class VehicleInfo
{
    [Parameter] public Vehicle Vehicle { get; set; }
}
