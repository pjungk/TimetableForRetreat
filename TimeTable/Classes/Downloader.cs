using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using TimeTable.Classes;

namespace TimeTable.Classes
{
    class Downloader
    {
        static string oriLink = TimeTablePage.oriLink;

        public static async Task<string> DownloadText(string FileName, string url)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string fileContent = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    //if(!IsDeserializable(fileContent))
                    //{
                    //    await DisplayAlert("Something went wrong", "Likely something with the formatting of the source file", "ok.");
                    //}
                    return fileContent;
                }
                catch (InvalidOperationException e) // The Link doesn't work
                {
                    await FileHandler.SaveTextAsync("link.txt", oriLink);

                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            HttpClient httpClient = new HttpClient();
                            string fileContent = httpClient.GetAsync(oriLink).Result.Content.ReadAsStringAsync().Result;
                            return fileContent;
                        }
                        catch (HttpRequestException e1)
                        {
                            return await SubstituteTimeTable(FileName);
                        }
                    }
                    else
                    {
                        return await SubstituteTimeTable(FileName);
                    }
                }
                catch (HttpRequestException e)
                {
                    return await SubstituteTimeTable(FileName);
                }
            }
            else
            {
                return await SubstituteTimeTable(FileName);
            }
        }

        private static async Task<string> SubstituteTimeTable(string FileName)
        {
            string FileText = "";

            if (!FileHandler.Exists(FileName))
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TimeTablePage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("TimeTable.resources.Zeitplan.json");

                using (var reader = new StreamReader(stream))
                {
                    FileText = reader.ReadToEnd();
                }
            }
            else
            {
                FileText = await FileHandler.ReadTextAsync(FileName);
            }

            return FileText;
        }


        public static bool IsDeserializable(string jsonString)
        {
            try
            {
                JsonConvert.DeserializeObject(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}