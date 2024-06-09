using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Interfaces;
using TimeTable.UWP;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(UWPStoreFunctions))]

namespace TimeTable.UWP
{
    class UWPStoreFunctions : IStoreUWP
    {
        public async Task<string> getSchedule(string fileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync(fileName);
            string schedule = await FileIO.ReadTextAsync(file);

            return schedule;
        }

        public async Task saveSchedule(string fileName, string schedule)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, schedule);
        }
    }
}
