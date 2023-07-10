using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;

namespace RandomWallpapers
{
    public class OnlineImageProvider : IImageProvider
    {
        private readonly HttpClient HttpClient;
        private readonly ImageFormatConverter ImageFormatConverter;
        private readonly Random RandomGenerator;
        public OnlineImageProvider()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
            ImageFormatConverter = new ImageFormatConverter();
            RandomGenerator = new Random();
        }

        public async Task<string?> GetImage()
        {
            return Settings.RandomImage ? await GetRandomImage() : await GetImageBySearchQueries();
        }

        private async Task<string?> GetRandomImage()
        {
            try
            {
                HttpResponseMessage response =
                    await HttpClient.GetAsync($"{Settings.BaseURL}/photos/random/?client_id={Settings.AccessKey}");
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error sending request, Status Code : {response.StatusCode}");
                    return null;
                }

                // get full image uri from json resppnse.
                dynamic json = JObject.Parse(
                    await response.Content.ReadAsStringAsync()
                    );

                var url = json.urls.full.Value;

                // getting image bytes[] array.
                byte[] imgBytes = await RequestImageBytes(url);
                var imagePath = SaveImage(imgBytes, "Random");
                
                return imagePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string?> GetImageBySearchQueries()
        {
            try
            {
                // a random page number to most likely
                // get a random result every time
                int PageNumber = RandomGenerator.Next(1, 5);
                string query = GetRandomSearchQuery();
                HttpResponseMessage queryResponse =
                    await HttpClient.GetAsync($"{Settings.BaseURL}/search/photos/?client_id={Settings.AccessKey}&query={query}&page={PageNumber}&per_page=10");
                
                if(!queryResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error sending request, Status Code : {queryResponse.StatusCode}");
                    return null;
                }

                // Getting url value from json response.
                dynamic json = JObject.Parse(
                    await queryResponse.Content.ReadAsStringAsync()
                    );

                JArray arrayResults = (JArray)json.results;
                int randIndex = RandomGenerator.Next(0, arrayResults.Count() - 1);
                dynamic randResult = json.results[randIndex];
                var url = randResult.urls.full.Value;

                // Getting image bytes[] array.
                byte[] imgBytes = await RequestImageBytes(url);
                var imagePath = SaveImage(imgBytes, query);
                
                return imagePath;
            }
            catch (Exception)
            {
                throw ;
            }
        }

        private string SaveImage(byte[] imgBytes, string subDirectory)
        {
            string fulPath = Path.Combine(Settings.AppDirectory, subDirectory);

            if (!Directory.Exists(fulPath))
                Directory.CreateDirectory(fulPath);
            
            
            using (Image image = Image.FromStream(new MemoryStream(imgBytes)))
            {
                string extension = ImageFormatConverter.ConvertToString(image.RawFormat);
                string imageName = $"{Guid.NewGuid()}.{extension}";
                string imagePath = Path.Combine(fulPath, imageName);
                image.Save(imagePath, ImageFormat.Jpeg);
                
                return imagePath;
            }
        }

        private string GetRandomSearchQuery()
        {
            string query = Settings.QueryString;
            IEnumerable<string> querys = query.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int idx = RandomGenerator.Next(0, querys.Count());
            
            return querys.ElementAt(idx).Trim();
        }

        private async Task<byte[]> RequestImageBytes(string url)
        {
            HttpResponseMessage imageResponse = await HttpClient.GetAsync(url);
            
            return await imageResponse.Content.ReadAsByteArrayAsync();
        }
    }

}
