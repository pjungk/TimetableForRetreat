using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using TimeTable.Interfaces;
using System.Threading.Tasks;
using System.IO;
using TimeTable.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(iOSFileHandler))]

namespace TimeTable.iOS
{
    class iOSFileHandler : IPlatformFileHandler
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