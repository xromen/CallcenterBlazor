using Callcenter.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Callcenter.Web.Components
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
                UploadFilesChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<List<IBrowserFile>> UploadFilesChanged { get; set; }

        [Parameter]
        public string? Accept { get; set; }

        [Parameter] public List<FileDto> DbFiles { get; set; } = new();

        private List<IBrowserFile> _files = new();

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            UploadFiles.AddRange(e.GetMultipleFiles());
        }
    }
}
