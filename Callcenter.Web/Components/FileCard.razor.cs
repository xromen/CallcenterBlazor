using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Callcenter.Web.Components;

public partial class FileCard : ComponentBase
{
    [Parameter]
    public string Name { get; set; }
    
    [Parameter]
    public  EventCallback<MouseEventArgs> DeleteFile { get; set; }
}