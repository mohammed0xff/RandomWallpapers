namespace RandomWallpapers
{
	class Program
	{
		private static IImageProvider Provider;
		private static bool TestConnection()
		{
            try
            {
				System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
				System.Net.NetworkInformation.PingReply reply = pingSender.Send("www.google.com");
				if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
				{
					return true;
				}
			}
			catch (Exception) {}
			return false;
		}

		static async Task Main(string[] args)
		{
            if (!OperatingSystem.IsWindows())
            {
                Console.WriteLine("Sorry, Only supporting Windows now.");
				return;
            }

            if (Settings.Online && TestConnection() == false)
            {
                Console.WriteLine("Please Check Your Internet Connection.");
				return;
            }

			if (Settings.Online)
			{
				Provider = new OnlineImageProvider();
			}
			else
			{
				Provider = new OfflineImageProvider();
			}

			string? imgPath = "";
			ScreenPainter screenPainter = new ScreenPainter(Settings.Style);
			int everyNMinutes = int.Parse(Settings.EveryNMinuites) * 60 * 1000;

			while (true)
			{
				imgPath = await Provider.GetImage();
				if (string.IsNullOrEmpty(imgPath))
				{
                    Console.WriteLine("Error : Couldn't Get Image Path.");
					return ;
				}
				screenPainter.SetWallpaperImage(imgPath);
				await Task.Delay(everyNMinutes);
			}
		}

	}
}