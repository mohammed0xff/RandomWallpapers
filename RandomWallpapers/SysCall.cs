using System.Runtime.InteropServices;

namespace RandomWallpapers
{
	class SysCall
	{
		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

		public enum ProcessDpiAwareness
		{
			ProcessDpiUnaware = 0,
			ProcessSystemDpiAware = 1,
			ProcessPerMonitorDpiAware = 2
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

		[DllImport("shcore.dll")]
		private static extern int SetProcessDpiAwareness(ProcessDpiAwareness Dpi);

		// Setting the current process as dots per inch (dpi) aware to get best resutls.
		public static bool EnableDpiAwareness()
		{
			try
			{
				if (Environment.OSVersion.Version.Major < 6)
					return false;
				SetProcessDpiAwareness(ProcessDpiAwareness.ProcessPerMonitorDpiAware);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public static bool SetDesktopWallpaper(string wallpaperFilePath)
		{
			try
			{
				SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperFilePath,
					SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
