using Microsoft.AspNetCore.Components.Forms;

namespace ScanCode.Web.Admin.Pages.Dialog.Components.FileUpImage
{
    public class FileModel
    {
        public IBrowserFile File { set; get; }

        public string ImageBase64 { get; set; }
    }
}