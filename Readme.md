# Youtube Mp3 Downloader

- This program reads given txt file (youtube video link per line), downloads linked videos with only audio, and converts that m4aac formatted files to mp3 with ffmpeg file on system (or downloaded and added to the path).

### Requirements

[![Build Status](https://shields.io/badge/.Net-7-purple)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
[![Build Status](https://shields.io/badge/YoutubeExplode-6.2.5-blue)](https://www.nuget.org/packages/YoutubeExplode)
[![Build Status](https://shields.io/badge/ffmpeg-blue)](https://ffmpeg.org/download.html#get-sources)
[![Build Status](https://shields.io/badge/Xabe.FFmpeg-5.2.5-blue)](https://www.nuget.org/packages/Xabe.FFmpeg)

- [dotnet 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [YoutubeExplode](https://www.nuget.org/packages/YoutubeExplode)
- [ffmpeg](https://ffmpeg.org/download.html#get-sources), i installed with apt package manager in Ubuntu 22.04
- [Xabe.FFmpeg](https://www.nuget.org/packages/Xabe.FFmpeg)

### How To Run

- For getting packages open terminal in project directory:
```
dotnet restore
```

- When dotnet restore command executes without any error run:
```
dotnet run
```
- You can provide txt file location and downloaded file location with arguments like below:
```
dotnet run /home/$USER/Documents/mp3.txt /home/$USER/Documents/mp3/
```
- This command reads text file from first argument and downloads files to second argument location.

### How To Create Executables

- Linux, not distribution-specific
```
dotnet publish -c release --self-contained --runtime linux-x64 --framework net7.0
```
- Executables is created in {this project directory}/bin/release/net7.0/linux-x64

- Windows, not distribution-specific
```
dotnet publish -c release --self-contained --runtime win-x64 --framework net7.0
```
- Executables is created in {this project directory}/bin/release/net7.0/win-x64

- macOs, not distribution-specific
```
dotnet publish -c release --self-contained --runtime osx-x64 --framework net7.0
```
- Executables is created in {this project directory}/bin/release/net7.0/osx-x64

### For Linux

- Edit .bashrc and add {this project directory}/bin/release/net7.0/linux-x64 to path. Or you can move this folder (only linux-x64) with ingredients and change folder name. If you move folder and/or chnaged folder name add to path that folder.

- You should be able to run command below after successful path modify:
```
YoutubeDownloadTest
```
