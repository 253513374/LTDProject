namespace Wtdl.Wasm
{
    public static class Extension
    {
        public static readonly string ImageBaseUrl = "https://admin.rewt.cn/";

        //string 扩展 返回图像URL
        public static string GetImageUrl(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return $"{ImageBaseUrl}{url.Replace('\\', '/')}";
            }
            else
            {
                return url;
            }
        }
    }
}