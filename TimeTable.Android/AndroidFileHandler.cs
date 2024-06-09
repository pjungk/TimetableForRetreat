using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Droid;
using TimeTable.Interfaces;
using Environment = System.Environment;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidFileHandler))]

namespace TimeTable.Droid
{
    class AndroidFileHandler : IPlatformFileHandler
    {
        static string storagePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public async Task<string> ReadText(string fileName)
        {
            string savedText = "";
            string filePath = Path.Combine(storagePath, fileName);

            using (var streamReader = new StreamReader(filePath))
            {
                savedText = streamReader.ReadToEnd();
            }

            return savedText;
        }

        public async Task SaveText(string fileName, string textToSave)
        {
            string filePath = Path.Combine(storagePath, fileName);

            using (var streamWriter = new StreamWriter(filePath, false))
            {
                streamWriter.Write(textToSave);
            }
        }
    }
}