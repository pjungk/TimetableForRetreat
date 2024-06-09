using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using TimeTable.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TimeTable.Classes
{
    static class FileHandler
    {
        static string StoragePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        static IPlatformFileHandler platformFileHandler = DependencyService.Get<IPlatformFileHandler>();

        public static async Task SaveTextAsync(string FileName, string textToSave)
        {
            await platformFileHandler.SaveText(FileName, textToSave);
        }

        public static async Task<string> ReadTextAsync(string FileName)
        {
            string SavedText = await platformFileHandler.ReadText(FileName);

            return SavedText;
        }

        public static async Task Update(string FileName, string FileLink)
        {
            string downloadedText = await Downloader.DownloadText(FileName, FileLink);
            //Debug.WriteLine(downloadedText);

            await SaveTextAsync(FileName, downloadedText);
        }

        public static bool Exists(string FileName)
        {
            bool fileExists = File.Exists(Path.Combine(StoragePath, FileName));

            return fileExists;
        }

        public static DateTime GetLastWriteTime(string FileName)
        {
            return File.GetLastWriteTime(Path.Combine(StoragePath, FileName));
        }


        public static async Task<string> GetProperty(string propertiesFile, string propertySelection)
        {
            Properties properties = JsonConvert.DeserializeObject<Properties>(await ReadTextAsync(propertiesFile));
            string propertyContent = "";

            switch (propertySelection)
            {
                case "link":
                    propertyContent = properties.link;
                    break;
                case "message":
                    propertyContent = properties.message;
                    break;

            }

            return propertyContent;
        }

        public static async Task SetProperty(string propertiesFile, string propertyContent, string propertySelection)
        {
            Properties properties = JsonConvert.DeserializeObject<Properties>(await ReadTextAsync(propertiesFile));

            switch (propertySelection)
            {
                case "link":
                    properties.link = propertyContent;
                    break;
                case "message":
                    properties.message = propertyContent;
                    break;

            }

            string jsonString = JsonConvert.SerializeObject(properties);
            await SaveTextAsync(propertiesFile, jsonString);

        }
    }
}