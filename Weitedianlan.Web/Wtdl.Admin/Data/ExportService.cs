using MiniExcelLibs;

namespace Wtdl.Admin.Data
{
    public class ExportService
    {
        public byte[] ExportToExcel<T>(IEnumerable<T> data)
        {
            using var stream = new MemoryStream();
            MiniExcel.SaveAs(stream, data);
            return stream.ToArray();
        }
    }
}