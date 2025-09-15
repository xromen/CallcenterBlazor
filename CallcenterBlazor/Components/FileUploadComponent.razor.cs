using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CallcenterBlazor.Components
{
    public partial class FileUploadComponent
    {
        [Parameter]
        public List<IBrowserFile> UploadFiles
        {
            get => _files;
            set
            {
                _files = value;
                UploadFileChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<List<IBrowserFile>> UploadFileChanged { get; set; }

        [Parameter]
        public string? Accept { get; set; }

        [Parameter]
        public string? HelpText { get; set; }

        private List<IBrowserFile> _files = new();

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            UploadFiles.AddRange(e.GetMultipleFiles());
        }
    }
}
