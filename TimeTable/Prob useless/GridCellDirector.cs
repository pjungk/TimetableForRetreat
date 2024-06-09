using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTable.Classes
{
    class GridCellDirector
    {
        private readonly Interfaces.IGridCellBuilder _gridCellBuilder;


        // For EmptyGrid
        public GridCellDirector(Interfaces.IGridCellBuilder gridCellBuilder)
        {
            _gridCellBuilder = gridCellBuilder;
        }

        public void BuildEmptyCell(GridLayout gridLayout)
        {
            _gridCellBuilder.BuildEmptyCell(gridLayout);
        }

        public void BuildTimeCell(double cellStartTime, double cellEndTime, GridLayout gridLayout)
        {
            _gridCellBuilder.BuildEmptyCell(gridLayout);
            _gridCellBuilder.BuildTimeFilling(cellStartTime, cellEndTime);
        }

        public void BuildDayCell(string day, string guide, GridLayout gridLayout)
        {
            _gridCellBuilder.BuildEmptyCell(gridLayout);
            _gridCellBuilder.BuildDayFilling(day, new dayInfo(guide));
        }

        public void BuildFreeTimeCell(string freeTime, GridLayout gridLayout)
        {
            _gridCellBuilder.BuildEmptyCell(gridLayout);
            _gridCellBuilder.BuildFreeTimeFilling(freeTime, new freeTimeInfo());

        }

        public void BuildActivityCell(Aktivitäten activity, GridLayout gridLayout)
        {
            _gridCellBuilder.BuildEmptyCell(gridLayout);
            _gridCellBuilder.BuildActivityFilling(activity);

        }
    }
}
