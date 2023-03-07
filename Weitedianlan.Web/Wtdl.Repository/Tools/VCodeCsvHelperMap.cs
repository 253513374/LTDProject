using CsvHelper.Configuration;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.Tools
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