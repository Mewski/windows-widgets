using System.Runtime.InteropServices;
using System.Text;

namespace WindowsWidgets
{
    public static class SystemInformation
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        private const int SPI_GETDESKWALLPAPER = 0x0073;

        public static string GetWallpaperPath()
        {
            var path = new StringBuilder(300);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, 300, path, 0);
            return path.ToString();
        }
    }
}
