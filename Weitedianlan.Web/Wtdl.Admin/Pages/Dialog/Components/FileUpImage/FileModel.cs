using Microsoft.AspNetCore.Components.Forms;

namespace Wtdl.Admin.Pages.Dialog.Components.FileUpImage
{
    public class FileModel
    {
        public IBrowserFile File { set; get; }

        public string ImageBase64 { get; set; }
    }
}