using System;
using System.Collections.Generic;
using System.Text;
using TimeTable.Classes;
using Xamarin.Forms;

namespace TimeTable.Interfaces
{
    public interface IGridCellBuilder
    {
        void BuildEmptyCell(GridLayout gridLayout);
        void BuildFreeTimeFilling(string FreeTime, freeTimeInfo FreeTimePage);
        void BuildTimeFilling(double cellStartTime, double cellEndTime);
        void BuildDayFilling(string day, dayInfo DayPage);
        void BuildActivityFilling(Classes.Aktivitäten activity);
        Grid GetGridCell();
    }
}
