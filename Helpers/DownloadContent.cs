using Xabe.FFmpeg;

using YoutubeDownloadTest.Entities;

using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloadTest.Helpers;

public class DownloadContent
{
    private Parameters _parameters;
    private YoutubeClient _youtubeClient;

    public DownloadContent(Parameters parameters)
    {
        _parameters = parameters;
        _youtubeClient = new YoutubeClient();
    }

    public async Task StartDownloadAsync()
    {
        if (_parameters.Link != null)
        {
            // Console.WriteLine($"Download From Link");
            await Download(_parameters.Link);
        }
        else if (_parameters.FilePath != null)
        {
            // Console.WriteLine($"Download From TextFile");
            string[] links = await File.ReadAllLinesAsync(_parameters.FilePath);
            foreach (var link in links)
            {
                await Download(link);
            }
        }


    }

    private async Task Download(string link)
    {
        var video = await _youtubeClient.Videos.GetAsync(link);

        var title = video.Title;
        var duration = video.Duration;

        title = title
            .Replace("/", "")
            .Replace("\"", "");

        Console.WriteLine($"Title: {title}\nDuration: {duration}");

        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(link);

        IStreamInfo streamInfo;

        streamInfo = _parameters.DownloadType switch
        {
            "mp3" => streamManifest
                        .GetAudioOnlyStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .GetWithHighestBitrate(),
            "mp4" => streamManifest
                        .GetMuxedStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .GetWithHighestBitrate(),
            "mp3&mp4" => streamManifest
                            .GetMuxedStreams()
                            .Where(s => s.Container == Container.Mp4)
                            .GetWithHighestBitrate(),
            _ => throw new Exception($"Unknown Download Type: {_parameters.DownloadType}")
        };

        var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo);

        var mp4Path = $"{_parameters.Directory}{title}.mp4";
        var mp3Path = $"{_parameters.Directory}{title}.mp3";

        await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, mp4Path);

        bool res = _parameters.DownloadType switch
        {
            "mp3" => await ConvertToMp3AndDeleteMp4(mp4Path: mp4Path, mp3Path: mp3Path),
            "mp4" => true,
            "mp3&mp4" => await ConvertToMp3(mp4Path: mp4Path, mp3Path: mp3Path),
            _ => false
        };

        // if (res)
        //     Console.WriteLine($"Downloaded {title} Successfully");
    }

    private async Task<bool> ConvertToMp3AndDeleteMp4(string mp4Path, string mp3Path)
    {
        bool mp3ConversionResult = await ConvertToMp3(mp4Path: mp4Path, mp3Path: mp3Path);
        bool mp4DeletionResult = DeleteMp4File(mp4Path);
        return mp3ConversionResult && mp4DeletionResult;
    }

    private async Task<bool> ConvertToMp3(string mp4Path, string mp3Path)
    {
        var x = await FFmpeg.Conversions.FromSnippet.Convert(mp4Path, mp3Path);
        x.SetOutput(mp3Path);
        await x.Start();
        x.Build();
        return true;
    }

    private bool DeleteMp4File(string mp4Path)
    {
        File.Delete(mp4Path);
        return true;
    }
}
