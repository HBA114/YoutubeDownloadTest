using Xabe.FFmpeg;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

//! published with command below:

//* dotnet publish -c release --self-contained --runtime linux-x64 --framework net7.0

//* For more information visit https://www.nuget.org/packages/YoutubeExplode

// var link = "https://www.youtube.com/watch?v=BX0lKSa_PTk&ab_channel=OliverTree";
#region Version2Test
// var youtube = new YoutubeClient();
// var video = await youtube.Videos.GetAsync(link);
// var streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);

// foreach (var item in streamManifest.GetAudioOnlyStreams().Where(x => x.Container == Container.Mp4).OrderByDescending(y => y.Size).ToList())
// {
//     System.Console.WriteLine($"Size: {item.Size}, Container: {item.Container}");
// }


// var title = video.Title;
// var duration = video.Duration;

// title = title.Replace("/", "");

// var streamInfo = streamManifest.GetAudioOnlyStreams().Where(x => x.Container == Container.Mp4).OrderByDescending(y => y.Size).First();


// var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

// await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{title}.mp3");

#endregion

Console.WriteLine("Enter path for mp3.txt file(default: /home/hbasri/Documents/)...");
string? path = Console.ReadLine();

if (string.IsNullOrEmpty(path))
{
    path = "/home/hbasri/Documents/";
}

if (!path.Contains(".txt"))
    path += "mp3.txt";

Console.WriteLine("Enter save path for downloaded files(default: /home/hbasri/Documents/mp3/)...");
string? savePath = Console.ReadLine();

if (string.IsNullOrEmpty(savePath))
{
    savePath = "/home/hbasri/Documents/mp3/";
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

    // await youtube.Videos.Streams.DownloadAsync(streamInfo, $"video.{streamInfo.Container}");
    await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{savePath}{title}.mp4");

    // var converter = new NReco.VideoConverter.FFMpegConverter();
    // converter.ConvertMedia($"{savePath}{title}.mp4", $"{savePath}{title}.mp3", ".mp3");

    // Conversion conversion = new Conversion();
    // conversion.SetOutputFormat(Format.mp3);
    // conversion.SetInputFormat(Format.mp4);
    // conversion.SetOutput($"{savePath}{title}.mp3");

    // var x = await FFmpeg.Conversions.FromSnippet.ExtractAudio($"{savePath}{title}.mp4", $"{savePath}{title}.mp3");
    var x = await FFmpeg.Conversions.FromSnippet.Convert($"{savePath}{title}.mp4", $"{savePath}{title}.mp3");

    x.SetOutput($"{savePath}{title}.mp3");
    await x.Start();
    x.Build();

    // IConversionResult result = await Conversion.ExtractAudio(Resources.Mp4WithAudio, output)
    //    .Start();
}

//TODO: Add Flutter frontend and get all musics from https://music.youtube.com