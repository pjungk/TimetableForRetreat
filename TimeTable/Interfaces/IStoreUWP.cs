using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable.Interfaces
{
    public interface IStoreUWP
    {
        Task<string> getSchedule(string fileName);
        Task saveSchedule(string fileName, string schedule);
    }
}
