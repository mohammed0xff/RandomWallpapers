using System.Text.RegularExpressions;

namespace RandomWallpapers
{
	class OfflineImageProvider : IImageProvider
	{
		public static string? GetOneRandomly(string? path = null)
		{
			path ??= Settings.OfflineDirectory;

            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("ERROR: The specified directory path is empty.");
                return null;
            }


            if (!Directory.Exists(path))
            {
                Console.WriteLine("ERROR: The specified directory path does not exist.");
                return null;
            }

            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var filesList = Directory.GetFiles(path)
                                      .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                                      .ToList();
            if (filesList.Count == 0)
			{
                Console.WriteLine("ERROR : Directory specified doesn't contain images.");
				return null;
			}

			// choose and return one randomly.
			var randgen = new Random();
			int idx = randgen.Next(0, filesList.Count -1);
			var chosenImage = filesList.ElementAt(idx);
			
			return chosenImage;
		}

        public Task<string?> GetImage()
        {
			return Task.FromResult(GetOneRandomly());
		}
    }
}

