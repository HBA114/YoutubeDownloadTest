namespace YoutubeDownloadTest.Entities;

public class Parameters
{
    public string Directory { get; set; }
    public string? FilePath { get; set; } = null;
    public string? Link { get; set; } = null;
    public string DownloadType { get; set; }

    public Parameters(string directory, string? filePath, string? link, string downloadType)
    {
        Directory = directory;
        FilePath = filePath;
        Link = link;
        DownloadType = downloadType;
    }
}
