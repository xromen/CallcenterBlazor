using Callcenter.Shared;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Callcenter.Web.Components
{
    public partial class FileUploadComponent : IDisposable
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

        [Inject] public DeclarationsService Service { get; set; } = null!;
        
        [Inject] public IJSRuntime Js { get; set; } = null!;
        
        [Inject] public ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

        private List<IBrowserFile> _files = new();
        
        private CancellationTokenSource _tokenSource = new();

        private async Task OnInputFileChanged(InputFileChangeEventArgs e)
        {
            UploadFiles.AddRange(e.GetMultipleFiles());
        }

        private async Task DownloadFile(FileDto file)
        {
            var result = await Service.DownloadFile(file.Id, _tokenSource.Token);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
                return;
            }
            
            await Js.InvokeVoidAsync("saveAsFile", file.Name,
                Convert.ToBase64String(result.Data!));
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}
