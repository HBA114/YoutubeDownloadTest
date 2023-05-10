namespace YoutubeDownloadTest.Entities;

public class Parameters
{
    public string _directory;
    public string? _filePath = null;
    public string? _link = null;
    public string _downloadType;

    public Parameters(string directory, string? filePath, string? link, string downloadType)
    {
        _directory = directory;
        _filePath = filePath;
        _link = link;
        _downloadType = downloadType;
    }
}
