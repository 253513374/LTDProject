using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace PPK_Logistics.Config
{
    //public class NotNullValidationRule : ValidationRule
    //{
    //    public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
    //    {
    //        if (string.IsNullOrEmpty(value as string) || string.IsNullOrWhiteSpace(value as string))
    //        {
    //            return new ValidationResult(false, "不能为空！");
    //        }
    //        return new ValidationResult(true, null);
    //    }
    //}

    internal class BeepUP
    {
        /// <param name="iFrequency">声音频率（从37Hz到32767Hz）。在windows95中忽略</param>
        /// <param name="iDuration">声音的持续时间，以毫秒为单位。</param>
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;
        public static extern bool Beep(int frequency, int duration);

        public static SoundPlayer player = new SoundPlayer(Tools.AppPath + "music\\6.wav");
        public static SoundPlayer player1 = new SoundPlayer(Tools.AppPath + "music\\7.wav");
    }
}