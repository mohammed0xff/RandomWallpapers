using Microsoft.Win32;

namespace RandomWallpapers
{

	public class ScreenPainter
	{
        public ScreenPainter(WallpaperStyle style)
        {
			SetWallPaperStyle(style);
			SysCall.EnableDpiAwareness();
		}

		private void SetWallPaperStyle(WallpaperStyle style)
        {
			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                switch (style)
                {
                    case WallpaperStyle.Fill:
						key.SetValue(@"WallpaperStyle", 10.ToString());
						key.SetValue(@"TileWallpaper", 0.ToString());
						break;
                    case WallpaperStyle.Fit:
						key.SetValue(@"WallpaperStyle", 6.ToString());
						key.SetValue(@"TileWallpaper", 0.ToString());
						break;
                    case WallpaperStyle.Tile:
						key.SetValue(@"WallpaperStyle", 0.ToString());
						key.SetValue(@"TileWallpaper", 1.ToString());
						break;
                    case WallpaperStyle.Span:
						key.SetValue(@"WallpaperStyle", 22.ToString());
						key.SetValue(@"TileWallpaper", 0.ToString());
						break;
                    case WallpaperStyle.Stretch:
						key.SetValue(@"WallpaperStyle", 2.ToString());
						key.SetValue(@"TileWallpaper", 0.ToString());
						break;
                    case WallpaperStyle.Center:
						key.SetValue(@"WallpaperStyle", 0.ToString());
						key.SetValue(@"TileWallpaper", 0.ToString());
						break;
                    default:
                        break;
                }
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
		public void SetWallpaperImage(string ImageFilePath)
		{
			try
			{
				SysCall.SetDesktopWallpaper(ImageFilePath);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

	}

	public enum WallpaperStyle
	{
		Fill,
		Fit,
		Tile,
		Span,
		Stretch,
		Center
	}
}
