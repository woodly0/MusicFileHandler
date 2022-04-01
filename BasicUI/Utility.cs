using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using BasicUI.Models;
using Id3;
using PlaylistsNET.Content;
using PlaylistsNET.Models;
using System.Text.RegularExpressions;

namespace BasicUI
{
    public static class Utility
    {

        public static string ExtractFileName(string[] files, string current)
        {
            foreach (var path in files)
            {
                var fileInfo = new FileInfo(path);
                var extension = ".wpl";
                if (fileInfo.Extension == extension)
                {
                    return Path.GetFileNameWithoutExtension(fileInfo.FullName);
                }
            }
            return current;
        }

        public static void ReceiveFiles(string[] files, ObservableCollection<SongModel> playList)
        {

            foreach (var path in files)
            {
                if (File.Exists(path))  // check if file is really there
                {
                    var fileInfo = new FileInfo(path);
                    if (fileInfo.Extension == ".mp3")
                    {
                        CreateMP3Model(fileInfo, playList);

                    }
                    else if (fileInfo.Extension == ".wpl")
                    {
                        CreateWPLModel(fileInfo, playList);
                    }
                }
            }

        }

        private static void CreateMP3Model(FileInfo file, ObservableCollection<SongModel> playList)
        {
            var thisSong = new SongModel()
            {
                FileName = file.Name,
                FilePath = file.DirectoryName,
                Size = Math.Round(file.Length / 1e6, 2)       //Bytes to MB
            };

            try
            {
                using (var mp3 = new Mp3(file.FullName))
                {
                    Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);
                    thisSong.Artist = tag.Artists;
                    thisSong.Title = tag.Title;
                }
            }
            catch (System.NullReferenceException ex)
            {
                //some files won't open...
            }

            playList.Add(thisSong);
        }


        private static void CreateWPLModel(FileInfo file, ObservableCollection<SongModel> playList)
        {
            List<string> paths;

            using (FileStream stream = new FileStream(file.FullName, FileMode.Open))
            {
                WplContent content = new WplContent();
                WplPlaylist wpl = content.GetFromStream(stream);
                paths = wpl.GetTracksPaths();
            }

            paths = MergeRelativePaths(file.DirectoryName, paths);
            ReceiveFiles(paths.ToArray(), playList);
        }


        private static List<string> MergeRelativePaths(string basePath, List<string> relativePaths)
        {
            var outputList = new List<string>();

            foreach (var path in relativePaths)
            {
                string output = "";

                var baseDir = basePath.Split('\\');
                var relDir = path.Split('\\');

                int overlap = 0;

                for (int i = 0; i < relDir.Length; i++)
                {
                    if (relDir[i] == "..")
                    {
                        overlap++;
                    }
                    else
                    {
                        output += "\\" + relDir[i];
                    }
                }

                for (int j = baseDir.Length - overlap - 1; j >= 0; j--)
                {
                    output = baseDir[j] + "\\" + output;
                }

                outputList.Add(output);
            }
            return outputList;
        }



        public static bool StringIsValid(string input, int minLen, int maxLen)
        {
            var answer = false;
            input = CleanString(input);
            var length = input.Length;

            if (length >= minLen && length <= maxLen)
                answer = true;

            return answer;
        }

        public static string CleanString(string input)
        {
            return Regex.Replace(input, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.0));
        }

        public static void CreateDir(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Console.WriteLine("Created directory: " + directory);
            }
        }
        public static void CopyFile(string filePath, string newfilePath)
        {
            if (File.Exists(filePath))
            {
                if (File.Exists(newfilePath))
                {
                    File.Delete(newfilePath);
                }
                File.Copy(filePath, newfilePath);
                Console.WriteLine("Copied " + filePath);
            }

        }


        public static void GetDummySong(ObservableCollection<SongModel> list)
        {
            var randomSong = new SongModel()
            {
                Artist = "ABBA",
                FileName = "ABBA - Swagger Disco",
                FilePath = @"G:\Users\Pascal\Music\Pop & Others",
                Size = 5.23,
                Title = "Swagger Disco"
            };

            list.Add(randomSong);
        }

    }





}
