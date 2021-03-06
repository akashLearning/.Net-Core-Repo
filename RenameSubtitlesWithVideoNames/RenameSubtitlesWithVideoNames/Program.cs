﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameSubtitlesWithVideoNames
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This will rename all subtitle files with respective video names.\n" +
                "In Order to this work make sure you have atlease some common character sequence in video and subtitle and are are different for all videos\n" +
                "For example, Friends.S01E01_posted_by_akash_**.mp4 and Friends.S01E01_random_subtitle**.srt \n" +
                "---------------------------------------------------------------------------------------------------------" +
                "---------------------------------------------------------------------------------------------------------");
            var dir = Directory.Exists(ConfigurationManager.AppSettings["VideoSubtitleFolderPath"]) ? ConfigurationManager.AppSettings["VideoSubtitleFolderPath"] : Directory.GetCurrentDirectory();
            var videoExtention = ConfigurationManager.AppSettings["VideoExtention"] ?? ".mkv";
            var subtitleExtention = ConfigurationManager.AppSettings["SubtitleExtention"] ?? ".srt";
            string[] files = Directory.GetFiles(dir);
            string[] subtitles = files.Where(str => str.EndsWith(subtitleExtention)).ToArray();
            string[] videos = files.Where(str => str.EndsWith(videoExtention)).ToArray();
            if (videos.Length == 0)
            {
                Console.WriteLine("No files were found in this folder\n" +
                    "Folder: " + dir);
                Console.WriteLine("\nTips:\n" +
                    "1. Make sure the video extension is correct. which is: "+ videoExtention +", if not then change it in config file\n2.You can run this application from the videos folder itself or just put folder path into the config file\n\nPress any key to exit");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Video files:\n");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            foreach (var item in videos)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Subtitle files:\n");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            foreach (var item in subtitles)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            string commonString = "";
            int seasonNumber = Convert.ToInt32(ConfigurationManager.AppSettings["SeasonNumber"] ?? "1");
            try
            {

                for (int i = 1; i <= videos.Length; i++)
                {
                    var seasonEpisode = string.Format("S{0:00}E{1:00}",seasonNumber, i);
                    if (videos[i - 1].ToUpper().Contains(seasonEpisode))
                    {
                        var subtitle = subtitles.FirstOrDefault(sub => sub.ToUpper().Contains(seasonEpisode));
                        File.Move(subtitle, videos[i - 1].Substring(0, videos[i - 1].Length - videoExtention.Length) + subtitleExtention);
                        Console.WriteLine(videos[i - 1].Substring(0, videos[i - 1].Length - videoExtention.Length) + subtitleExtention + "   Created OK");
                    }
                }
                Console.WriteLine("Subtitles assigned successfully.Press any key to exit...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while renaming the files: " + ex.Message);
            }
            Console.ReadKey();
        }
    }
}
