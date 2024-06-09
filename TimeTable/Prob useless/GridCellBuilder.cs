using System;
using System.Collections.Generic;
using System.Text;
using TimeTable.Interfaces;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    class GridCellBuilder : IGridCellBuilder
    {
        private Grid _gridCell;


        public void BuildEmptyCell(GridLayout gridLayout)
        {
            _gridCell = new Grid() { RowSpacing = gridLayout.spacing, ColumnSpacing = gridLayout.spacing };
        }

        public void BuildFreeTimeFilling(string FreeTime, freeTimeInfo FreeTimePage)
        {
            throw new NotImplementedException();
        }

        public void BuildTimeFilling(double cellStartTime, double cellEndTime)
        {
            throw new NotImplementedException();
        }

        public void BuildDayFilling(string day, dayInfo DayPage)
        {
            throw new NotImplementedException();
        }

        public void BuildActivityFilling(Aktivitäten activity)
        {
            throw new NotImplementedException();
        }


        public Color[] ColorStuff(string category)
        {
            return ColorDatabase.getColorScheme(category);
        }


        public Grid GetGridCell()
        {
            var gridCell = _gridCell;
            Clear();
            return gridCell;
        }

        private void Clear()
        {
            _gridCell = null;
        }
    }
}
