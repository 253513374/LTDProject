using System.Text.Encodings.Web;
using System.Text.Json;

namespace ScanCode.Model.ResponseModel
{
    public class TResult
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,//保留非ASCII字符（例如中文字符）转换为 Unicode 转义序列
            WriteIndented = true,//添加转换格式（换行）
            IgnoreNullValues = false,//序列化null属性
            PropertyNameCaseInsensitive = true,//属性名大小写不敏感
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never//永远不忽略属性
        };

        public override string ToString()
        {
            object o = this;
            var result = JsonSerializer.Serialize(o, options);
            return result;
        }

        public static T FromJson<T>(string jsonString) where T : TResult
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}