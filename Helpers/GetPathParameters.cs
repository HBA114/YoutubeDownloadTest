using YoutubeDownloadTest.Entities;

namespace YoutubeDownloadTest.Helpers;

public static class GetPathParameters
{
    public static Parameters GetAllParams(string[] args)
    {
        return DecodeParameters(args);
    }

    static Parameters DecodeParameters(string[] parameters)
    {
        var paramTypeList = new List<ParameterType?>();
        string directory, downloadType;
        string? filePath = null, link = null;

        foreach (var parameter in parameters)
            paramTypeList.Add(GetParameterType(parameter));

        var dirIndex = paramTypeList.IndexOf(ParameterType.Directory);
        if (dirIndex == -1)
        {
            string dirPathVar = "YoutubeDownloaded/";
            string dirPathGenerated = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), dirPathVar);
            if (!Path.Exists(dirPathGenerated)) System.IO.Directory.CreateDirectory(dirPathGenerated);
            directory = dirPathGenerated;
        }
        else
        {
            directory = parameters[dirIndex];
        }

        var downloadTypeIndex = paramTypeList.IndexOf(ParameterType.DownloadType);
        if (downloadTypeIndex == -1)
        {
            downloadType = "mp3";
        }
        else
        {
            downloadType = parameters[downloadTypeIndex];
        }

        var filePathIndex = paramTypeList.IndexOf(ParameterType.FilePath);
        if (filePathIndex != -1) filePath = parameters[filePathIndex];

        var linkIndex = paramTypeList.IndexOf(ParameterType.Link);
        if (linkIndex != -1) link = parameters[linkIndex];

        return new Parameters(directory, filePath, link, downloadType);
    }

    static ParameterType? GetParameterType(string parameter)
    {
        if (CheckParamIsDownloadContentType(parameter)) return ParameterType.DownloadType;
        if (CheckParamIsDirectory(parameter)) return ParameterType.Directory;
        if (CheckParamIsLink(parameter)) return ParameterType.Link;
        if (CheckParamIsTextFile(parameter)) return ParameterType.FilePath;

        throw new Exception("Parameter Type is Undefined!");
    }

    private static bool CheckParamIsDownloadContentType(string parameter)
    {
        if (parameter != "mp3" && parameter != "mp4" && parameter != "mp3&mp4") return false;
        return true;
    }

    private static bool CheckParamIsDirectory(string parameter)
    {
        if (!Path.Exists(parameter) || Path.GetExtension(parameter) != "") return false;
        return true;
    }

    private static bool CheckParamIsTextFile(string parameter)
    {
        if (Path.GetExtension(parameter) == ".txt") return true;
        return false;
    }

    private static bool CheckParamIsLink(string parameter)
    {
        if (parameter.Contains("youtube.com")) return true;
        return false;
    }

}
