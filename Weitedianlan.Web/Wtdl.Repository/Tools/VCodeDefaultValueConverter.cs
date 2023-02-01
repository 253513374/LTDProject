using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtdl.Repository.Tools
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
            //Compare this snippet from Wtdl.Repository\Tools\VCodeDefaultValueConverter.cs:
            return _parameter;
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }
    }
}