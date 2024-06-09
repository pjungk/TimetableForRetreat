using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable.Interfaces
{
    public interface IPlatformFileHandler
    {
        Task<string> ReadText(string fileName);
        Task SaveText(string fileName, string textToSave);
    }
}
