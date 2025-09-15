using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CallcenterBlazor.Pages;

public partial class Profile : ComponentBase
{
    private bool _isShowPassword = false;
    private InputType _passwordInputType = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    void PasswordShowToggle()
    {
        if (_isShowPassword)
        {
            _isShowPassword = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInputType = InputType.Password;
        }
        else {
            _isShowPassword = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInputType = InputType.Text;
        }
    }
}