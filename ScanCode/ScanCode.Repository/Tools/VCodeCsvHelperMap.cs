using CsvHelper.Configuration;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.Tools
{
    public class VCodeCsvHelperMap : ClassMap<VerificationCode>
    {
        // private string filehash;
        public VCodeCsvHelperMap(string filehash)
        {
            Map(m => m.Id).Ignore();
            Map(m => m.QRCode).Index(0).TypeConverter<VCodeConverter>();
            Map(m => m.Captcha).Index(1);
            Map(m => m.FileHash).TypeConverter(new VCodeDefaultValueConverter(filehash));
        }
    }
}