using YoutubeDownloadTest.Helpers;

//! published with command below:

//* dotnet publish -c release --self-contained --runtime linux-x64 --framework net7.0

//* For more information visit https://www.nuget.org/packages/YoutubeExplode

// var link = "https://www.youtube.com/watch?v=BX0lKSa_PTk&ab_channel=OliverTree";

/*

dotnet run "/home/hbasri/Downloads" "/home/hbasri/Documents/mp3.txt" "https://music.youtube.com/watch?v=8puHmESPh1g&list=RDAMVM8puHmESPh1g" "mp3"
dotnet run "/home/hbasri/Downloads" "/home/hbasri/Documents/mp3.txt" "mp3" "https://music.youtube.com/watch?v=8puHmESPh1g&list=RDAMVM8puHmESPh1g"
dotnet run "/home/hbasri/Documents/mp3.txt" "/home/hbasri/Downloads" "mp3" "https://music.youtube.com/watch?v=8puHmESPh1g&list=RDAMVM8puHmESPh1g"
dotnet run "/home/hbasri/Documents/mp3.txt" "/home/hbasri/Downloads"
*/


public class Program
{
    private static async Task Main(string[] args)
    {
        var parameters = GetPathParameters.GetAllParams(args);

        DownloadContent downloadContent = new DownloadContent(parameters);

        await downloadContent.StartDownloadAsync();
    }
}

//TODO: Add Flutter frontend and get all musics from https://music.youtube.com
