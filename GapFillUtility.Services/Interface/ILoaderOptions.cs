namespace GapFillUtility.Services.Interface
{
    public interface ILoaderOptions
    {
        string Environment { get; set; }

        string ProductVersion { get; set; }

        string Loaders { get; set; }

        string Products { get; set; }
    }
}
