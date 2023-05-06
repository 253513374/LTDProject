using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ScanCode.Repository.Tools
{
    public class VCodeDefaultValueConverter : ITypeConverter
    {
        private readonly object _parameter;

        public VCodeDefaultValueConverter(object parameter)
        {
            _parameter = parameter;
        }

        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            //Compare this snippet from ScanCode.Repository\Tools\VCodeDefaultValueConverter.cs:
            return _parameter;
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }
    }
}