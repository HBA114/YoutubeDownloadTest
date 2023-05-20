using Xabe.FFmpeg;
using YoutubeDownloadTest.Entities;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloadTest.Helpers;

public class DownloadContent
{
    private Parameters _parameters;
    private YoutubeClient _youtube;

    public DownloadContent(Parameters parameters)
    {
        _parameters = parameters;
        _youtube = new YoutubeClient();
    }

    public async Task StartDownloadAsync()    // task
    {
        if (_parameters._link != null)
        {
            Console.WriteLine($"Download From Link");
            await Download(_parameters._link);
        }
        else if (_parameters._filePath != null)
        {
            Console.WriteLine($"Download From TextFile");
            string[] links = await File.ReadAllLinesAsync(_parameters._filePath);
            foreach (var link in links)
            {
                await Download(link);
            }
        }


    }

    private async Task Download(string link)
    {
        var video = await _youtube.Videos.GetAsync(link);

        var title = video.Title;
        var duration = video.Duration;

        title = title.Replace("/", "");

        // Console.WriteLine($"Title: {title}\nDuration: {duration}");

        var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(link);

        IStreamInfo streamInfo;

        switch (_parameters._downloadType)
        {
            case "mp3":
                streamInfo = streamManifest
                    .GetAudioOnlyStreams()
                    .Where(s => s.Container == Container.Mp4)
                    .GetWithHighestBitrate();
                break;

            case "mp4":
                streamInfo = streamManifest
                        .GetMuxedStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .GetWithHighestBitrate();
                break;

            case "mp3&mp4":
                streamInfo = streamManifest
                    .GetMuxedStreams()
                    .Where(s => s.Container == Container.Mp4)
                    .GetWithHighestBitrate();
                break;

            default:
                streamInfo = streamManifest
                        .GetAudioOnlyStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .GetWithHighestBitrate();
                break;
        }

        var stream = await _youtube.Videos.Streams.GetAsync(streamInfo);

        var mp4_path = $"{_parameters._directory}{title}.mp4";
        var mp3_path = $"{_parameters._directory}{title}.mp3";

        await _youtube.Videos.Streams.DownloadAsync(streamInfo, mp4_path);

        switch (_parameters._downloadType)
        {
            case "mp3":
                await ConvertToMp3(mp4_path, mp3_path);
                DeleteMp4File(mp4_path);
                break;

            case "mp4":
                break;

            case "mp3&mp4":
                await ConvertToMp3(mp4_path, mp3_path);
                break;

            default:
                break;
        }
    }

    private async Task ConvertToMp3(string mp4_path, string mp3_path)
    {
        var x = await FFmpeg.Conversions.FromSnippet.Convert(mp4_path, mp3_path);
        x.SetOutput(mp3_path);
        await x.Start();
        x.Build();
    }

    private void DeleteMp4File(string mp4_path)
    {
        File.Delete(mp4_path);
    }
}
