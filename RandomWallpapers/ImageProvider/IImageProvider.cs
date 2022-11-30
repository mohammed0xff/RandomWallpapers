namespace RandomWallpapers
{
    public interface IImageProvider
    {
        Task<string?> GetImage();
    }
}
