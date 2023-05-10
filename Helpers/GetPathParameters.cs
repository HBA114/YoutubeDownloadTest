using YoutubeDownloadTest.Entities;

namespace YoutubeDownloadTest.Helpers;

public static class GetPathParameters
{
    public static Parameters GetAllParams(string[] args)
    {
        return decodeParameters(args);
    }

    static Parameters decodeParameters(string[] parameters)
    {
        var paramTypeList = new List<ParameterType?>();
        Parameters parametersEntity = new Parameters();
        foreach (var parameter in parameters)
        {
            paramTypeList.Add(GetParameterType(parameter));
        }

        var dirIndex = paramTypeList.IndexOf(ParameterType.Directory);
        if (dirIndex == -1)
        {
            string dirPathVar = "YoutubeDownloaded/";
            string dirPathGenerated = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), dirPathVar);
            if (!Path.Exists(dirPathGenerated)) System.IO.Directory.CreateDirectory(dirPathGenerated);
            parametersEntity._directory = dirPathGenerated;
        }
        else
        {
            parametersEntity._directory = parameters[dirIndex];
        }

        var downloadTypeIndex = paramTypeList.IndexOf(ParameterType.DownloadType);
        if (downloadTypeIndex == -1)
        {
            parametersEntity._downloadType = "mp3";
        }
        else
        {
            parametersEntity._downloadType = parameters[downloadTypeIndex];
        }

        var filePathIndex = paramTypeList.IndexOf(ParameterType.FilePath);
        if (filePathIndex != -1) parametersEntity._filePath = parameters[filePathIndex];

        var linkIndex = paramTypeList.IndexOf(ParameterType.Link);
        if (linkIndex != -1) parametersEntity._link = parameters[linkIndex];

        return parametersEntity;
    }

    static ParameterType? GetParameterType(string parameter)
    {
        if (checkParamIsDownloadContentType(parameter)) return ParameterType.DownloadType;
        if (checkParamIsDirectory(parameter)) return ParameterType.Directory;
        if (checkParamIsLink(parameter)) return ParameterType.Link;
        if (checkParamIsTextFile(parameter)) return ParameterType.FilePath;

        return null;
        // throw new Exception("Parameter Type is Undefined!");
    }

    private static bool checkParamIsDownloadContentType(string parameter)
    {
        if (parameter != "mp3" && parameter != "mp4" && parameter != "mp3&mp4") return false;
        return true;
    }

    private static bool checkParamIsDirectory(string parameter)
    {
        if (!Path.Exists(parameter) || Path.GetExtension(parameter) != "") return false;
        return true;
    }

    private static bool checkParamIsTextFile(string parameter)
    {
        if (Path.GetExtension(parameter) == ".txt") return true;
        return false;
    }

    private static bool checkParamIsLink(string parameter)
    {
        if (parameter.Contains("youtube.com")) return true;
        return false;
    }

}
