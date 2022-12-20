using YoutubeExplode;
using YoutubeExplode.Videos.Streams;


//* For more information visit https://www.nuget.org/packages/YoutubeExplode

var youtube = new YoutubeClient();

var video = await youtube.Videos.GetAsync("https://music.youtube.com/watch?v=BX0lKSa_PTk&list=LM");

var title = video.Title;
var duration = video.Duration;

Console.WriteLine($"Title: {title}\nDuration: {duration}");

var streamManifest = await youtube.Videos.Streams.GetManifestAsync(
    "https://music.youtube.com/watch?v=BX0lKSa_PTk&list=LM"
);

var streamInfo = streamManifest
    .GetAudioOnlyStreams()
    .Where(s => s.Container == Container.WebM)
    .GetWithHighestBitrate();

var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

// await youtube.Videos.Streams.DownloadAsync(streamInfo, $"video.{streamInfo.Container}");
await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{title}.mp3");

//TODO: Add Flutter frontend and get all musics from https://music.youtube.com
