using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTable.Classes
{
    public class Activity // Course
    {
        public string Remark { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public string Topic { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Instructor { get; set; }
        public string Description { get; set; }
    }

    public class Day // Day
    {
        public string Weekday { get; set; }
        public string DailySupervisor { get; set; }
        public List<Activity> Activities { get; set; } // Hours
    }

    public class RetreatSupervisor
    {
        public string Name { get; set; }
        public string FileUrl { get; set; }
    }

    public class CategoryColors
    {
        public string CategoryName { get; set; }
        public string ColorCode { get; set; }
    }

    public class Meme
    {
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public string Description { get; set; }
    }

    public class TimeTableObj
    {
        public string RetreatTopic { get; set; }
        public string Urgent { get; set; }
        public List<Meme> Memes { get; set; }
        public List<RetreatSupervisor> RetreatSupervisors { get; set; }
        public List<CategoryColors> ColorScheme { get; set; }
        public List<Day> Days { get; set; }
    }
}