using Xabe.FFmpeg;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

//! published with command below:

//* dotnet publish -c release --self-contained --runtime linux-x64 --framework net7.0

//* For more information visit https://www.nuget.org/packages/YoutubeExplode

// var link = "https://www.youtube.com/watch?v=BX0lKSa_PTk&ab_channel=OliverTree";

class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine($"{args.Count()}");
        string? argument1 = null;
        string? argument2 = null;
        if (args.Count() == 2)
        {
            argument1 = args[0];
            argument2 = args[1];

            Console.WriteLine($"{argument1}");
            Console.WriteLine($"{argument2}");
        }

        string path = "";
        string savePath = "";

        if (argument1 == null && argument2 == null)
        {
            Console.WriteLine("Enter path for mp3.txt file(default: /home/hbasri/Documents/)...");
            path = Console.ReadLine()!;

            if (string.IsNullOrEmpty(path))
            {
                path = "/home/hbasri/Documents/";
            }
            Console.WriteLine($"{path}");

            if (!path.Contains(".txt"))
                path += "mp3.txt";

            Console.WriteLine("Enter save path for downloaded files(default: /home/hbasri/Documents/mp3/)...");
            savePath = Console.ReadLine()!;

            if (string.IsNullOrEmpty(savePath))
            {
                savePath = "/home/hbasri/Documents/mp3/";
            }
            Console.WriteLine($"{savePath}");
        }
        else
        {
            path = argument1!;
            savePath = argument2!;
        }

        if (savePath.Last() != '/') savePath += "/";

        string[] links = await File.ReadAllLinesAsync(path);

        var youtube = new YoutubeClient();

        foreach (var link in links)
        {
            var video = await youtube.Videos.GetAsync(link);

            var title = video.Title;
            var duration = video.Duration;

            title = title.Replace("/", "");

            Console.WriteLine($"Title: {title}\nDuration: {duration}");

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);

            var streamInfo = streamManifest
                .GetAudioOnlyStreams()
                .Where(s => s.Container == Container.Mp4)
                .GetWithHighestBitrate();

            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

            var mp4_path = $"{savePath}{title}.mp4";
            var mp3_path = $"{savePath}{title}.mp3";

            await youtube.Videos.Streams.DownloadAsync(streamInfo, mp4_path);

            if (File.Exists(mp3_path)) continue;

            try
            {
                var x = await FFmpeg.Conversions.FromSnippet.Convert(mp4_path, mp3_path);

                x.SetOutput(mp3_path);
                await x.Start();
                x.Build();

                File.Delete(mp4_path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while converting {mp4_path} to {mp3_path}!");
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}

//TODO: Add Flutter frontend and get all musics from https://music.youtube.com