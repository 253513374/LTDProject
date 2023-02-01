using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wtdl.Repository.Tools
{
    public class VCodeConverter : ITypeConverter
    {
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            var pattern = @"[\d]{12,20}$";
            var match = Regex.Match(text, pattern);
            if (match.Success)
            {
                var result = match.Value;
                return result;
            }
            return "";
            // return formattedNumber;
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }
    }
}