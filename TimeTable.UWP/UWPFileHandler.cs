using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.UWP;
using TimeTable.Interfaces;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(UWPFileHandler))]

namespace TimeTable.UWP
{
    class UWPFileHandler : IPlatformFileHandler
    {
        public async Task<string> ReadText(string fileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync(fileName);
            string text = await FileIO.ReadTextAsync(file);

            return text;
        }

        public async Task SaveText(string fileName, string textToSave)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, textToSave);
        }
    }
}