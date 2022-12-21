using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

//! published with command below:

//* dotnet publish -c release --self-contained --runtime linux-x64 --framework net7.0

//* For more information visit https://www.nuget.org/packages/YoutubeExplode

Console.WriteLine("Enter path for mp3.txt file(default: /home/hbasri/Documents/)...");
string? path = Console.ReadLine();

if (path == null || path == "")
{
    path = "/home/hbasri/Documents/";
}

if (!path.Contains(".txt"))
    path += "mp3.txt";

Console.WriteLine("Enter save path for downloaded files(default: /home/hbasri/Documents/mp3/)...");
string? savePath = Console.ReadLine();

if (savePath == null || savePath == "")
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
        .Where(s => s.Container == Container.WebM)
        .GetWithHighestBitrate();

    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

    // await youtube.Videos.Streams.DownloadAsync(streamInfo, $"video.{streamInfo.Container}");
    await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{savePath}{title}.mp3");
}
//TODO: Add Flutter frontend and get all musics from https://music.youtube.com
