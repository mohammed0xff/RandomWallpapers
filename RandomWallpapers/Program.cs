using System.Net.NetworkInformation;

namespace RandomWallpapers
{
	class Program
	{
		private static IImageProvider Provider;
		private static bool IsConnected()
		{
            try
            {
                using Ping pingSender = new();
                PingReply reply = pingSender.Send("www.google.com");
                return reply.Status == IPStatus.Success;
            }
			catch {
				return false;
			}
		}

		static async Task Main(string[] args)
		{
            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine("Sorry, Only supporting Windows now.");
				return;
            }

            if (Settings.Online && IsConnected() == false)
            {
                Console.WriteLine("Please Check Your Internet Connection.");
				return;
            }
			
			ScreenPainter screenPainter = new ScreenPainter(Settings.Style);

            Provider = Settings.Online ?
				new OnlineImageProvider() : new OfflineImageProvider();

			int everyNMinutes = Settings.EveryNMinuites * 60 * 1000;

			while (true)
			{
				try
				{
					string? imgPath = await Provider.GetImage();
					
					if (string.IsNullOrEmpty(imgPath))
					{
						Console.WriteLine("Error : Couldn't Get Image Path.");
						return;
					}
					
					screenPainter.SetWallpaperImage(imgPath);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error : {ex.Message}");
					return;
				}

				await Task.Delay(everyNMinutes);
			}
		}
	}
}