using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable.Interfaces
{
    public interface IDownloader
    {
        Task DownloadFileAsync(string url, string fileName);
    }
}
