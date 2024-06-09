using MediaManager;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTable.Classes;
using TimeTable.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeTablePage : ContentPage
    {
        static string fileName = "Zeitplan.json";
        static string propertiesFile = "properties.json";

        double deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        double deviceDensity = DeviceDisplay.MainDisplayInfo.Density;
        DisplayOrientation deviceOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

        int autoRefreshTime = 0;
        int refreshTime = 0;
        public static string oriLink = "https://drive.google.com/uc?export=download&id=1v70uAS_PZmikxOO6-UFfKXHfCAhrbMrnAA";

        public TimeTablePage()
        {
            InitializeComponent();

            InitializeTimeTableAsync();

            dataScrollView.Scrolled += DataScrollView_Scrolled;
        }

        public Task InitializeTimeTableAsync()
        {
            return BuildTimeTable();
        }

        public async Task BuildTimeTable()
        {
            Thickness _frameMargin = new Thickness(2, 2, 2, 2);
            Thickness _labelPadding = new Thickness(0, 0, 0, 0);
            double _spacing = 0;
            GridLength dayColumnWidth = 169;
            int homebarHeight = 57; // Ideally I can get this from the device
            int statusbarHeight = 27; // Ideally I can get this from the device
            double dayLabelHeight = 42; // Ideally I can get this from the XAML
            double timeLabelHeight = 70; // Ideally I can get this from the XAML

            ClearTable();
            await InitializePropertiesFile();

            TimeTableObj schedule = await GetSchedule();
            string message = schedule.Urgent;
            double[] boundaryTimes = GetBoundaryTimes(schedule);

            await ShowImportantMessage(schedule.Urgent);

            if (deviceOrientation == DisplayOrientation.Landscape)
            {
                dayColumnWidth = (deviceWidth / deviceDensity - homebarHeight - timeLabelHeight) / schedule.Days.Count;
            }
            else
            {
                dayColumnWidth = 169;
            }


            // EMPTYGRID // EMPTYGRID // EMPTYGRID // EMPTYGRID // EMPTYGRID // EMPTYGRID // EMPTYGRID // EMPTYGRID //
            var memesBtn = new TapGestureRecognizer();
            memesBtn.Tapped += async (s, evt) => { await Navigation.PushAsync(new MemesPage(schedule.Memes)); }; // new TestMemesPage()); };
            emptyGrid.GestureRecognizers.Add(memesBtn);


            Grid timeLabelGrid = new TimeLabelBuilder(_frameMargin, _labelPadding, _spacing).BuildTimeLabel(boundaryTimes, propertiesFile, this);
            Grid dayLabelGrid = new DayLabelBuilder(_frameMargin, _labelPadding, _spacing).BuildDayLabel(schedule, dayColumnWidth);
            Grid mainGrid = new MainGridBuilder(_frameMargin, _labelPadding, _spacing).BuildMainGrid(schedule, boundaryTimes, dayColumnWidth);

            if (deviceOrientation == DisplayOrientation.Landscape)
            {
                rootTimeGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(deviceHeight / deviceDensity - statusbarHeight - dayLabelHeight) });
                rootDayGrid.RowDefinitions.Add(new RowDefinition());
                rootMainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(deviceHeight / deviceDensity - statusbarHeight - dayLabelHeight) });
            }
            else
            {
                rootTimeGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(deviceHeight / deviceDensity - homebarHeight - statusbarHeight - dayLabelHeight) });
                rootDayGrid.RowDefinitions.Add(new RowDefinition());
                rootMainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(deviceHeight / deviceDensity - homebarHeight - statusbarHeight - dayLabelHeight) });
            }

            rootTimeGrid.Children.Add(timeLabelGrid);
            rootDayGrid.Children.Add(dayLabelGrid);
            rootMainGrid.Children.Add(mainGrid);
        }

        private async void DataScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            await rowScrollView.ScrollToAsync(0, e.ScrollY, false);
            await colScrollView.ScrollToAsync(e.ScrollX, 0, false);
        }

        private void ClearTable()
        {
            emptyGrid.Children.Clear();
            emptyGrid.GestureRecognizers.Clear();
            emptyGrid.RowDefinitions.Clear();
            emptyGrid.ColumnDefinitions.Clear();
            rootTimeGrid.Children.Clear();
            rootTimeGrid.RowDefinitions.Clear();
            rootTimeGrid.ColumnDefinitions.Clear();
            rootDayGrid.Children.Clear();
            rootDayGrid.RowDefinitions.Clear();
            rootDayGrid.ColumnDefinitions.Clear();
            rootMainGrid.Children.Clear();
            rootMainGrid.RowDefinitions.Clear();
            rootMainGrid.ColumnDefinitions.Clear();
        }

        private async Task<TimeTableObj> GetSchedule()
        {
            TimeTableObj schedule = new TimeTableObj();
            var oldJsonString = "";

            if (FileHandler.Exists(fileName))
            {
                Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBB1");
                oldJsonString = await FileHandler.ReadTextAsync(fileName);
                Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBB2", oldJsonString);
            }

            if ((DateTime.Now - FileHandler.GetLastWriteTime(fileName)).TotalMinutes > autoRefreshTime || !FileHandler.Exists(fileName)) //autoRefreshTime || !FileHandler.Exists(fileName))
            {
                await FileHandler.Update(fileName, await FileHandler.GetProperty(propertiesFile, "link"));
                var newJsonString = await FileHandler.ReadTextAsync(fileName);
                CheckScheduleChange(newJsonString, oldJsonString);

                try
                {
                    schedule = JsonConvert.DeserializeObject<TimeTableObj>(newJsonString);
                }
                catch
                {
                    try
                    {
                        Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBB3", oldJsonString);
                        await DisplayAlert("Something went wrong", "Likely something with the formatting of the source file", "ok.");
                        schedule = JsonConvert.DeserializeObject<TimeTableObj>(oldJsonString);
                    }
                    catch
                    {
                        var backupJsonString = "";
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TimeTablePage)).Assembly;
                        Stream stream = assembly.GetManifestResourceStream("TimeTable.resources.Zeitplan.json");

                        using (var reader = new StreamReader(stream))
                        {
                            backupJsonString = reader.ReadToEnd();
                        }
                        await FileHandler.SaveTextAsync(fileName, backupJsonString);

                        Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBB3", backupJsonString);
                        schedule = JsonConvert.DeserializeObject<TimeTableObj>(backupJsonString);
                    }
                }

                return schedule;
            }
            else
            {
                return JsonConvert.DeserializeObject<TimeTableObj>(oldJsonString);
            }
        }

        private async void CheckScheduleChange(string newJsonString, string oldJsonString) // Implement a function to get the exact changes in the Schedule
        {
            if (!String.Equals(newJsonString, oldJsonString) && !String.Equals(oldJsonString, ""))
            {
                await DisplayAlert("Changes", "The schedule has changed somewhere", "Okay!");
            }
        }

        private double[] GetBoundaryTimes(TimeTableObj schedule)
        {
            double[] boundaryTimes = new double[2];
            double startTime = 14;
            double endTime = 15;

            foreach (var d in schedule.Days)
            {
                foreach (var a in d.Activities)
                {
                    if (a.Start < startTime)
                    {
                        startTime = a.Start;
                    }
                    if (a.End > endTime)
                    {
                        endTime = a.End;
                    }
                }
            }

            boundaryTimes[0] = startTime;
            boundaryTimes[1] = endTime;

            return boundaryTimes;
        }

        private async Task ShowImportantMessage(string importantMessage)
        {
            if (importantMessage != null && !importantMessage.Equals(await FileHandler.GetProperty(propertiesFile, "message")))
            {
                await DisplayAlert("Important message!", importantMessage, "Good to know");
                await FileHandler.SetProperty(propertiesFile, importantMessage, "message");
            }
            else if (importantMessage == null)
            {
                await FileHandler.SetProperty(propertiesFile, "", "message");
            }
        }

        private async Task InitializePropertiesFile()
        {
            var propertiesEntry = new Properties
            {
                link = oriLink,
                message = ""
            };

            string jsonString = JsonConvert.SerializeObject(propertiesEntry);

            if (!FileHandler.Exists(propertiesFile))
            {
                await FileHandler.SaveTextAsync(propertiesFile, jsonString);
            }
        }

        private async void Update(object sender, EventArgs e)
        {
            if ((DateTime.Now - FileHandler.GetLastWriteTime(fileName)).TotalMinutes > refreshTime)
            {
                string link = await FileHandler.GetProperty(propertiesFile, "link");
                await FileHandler.Update(fileName, link);

                await BuildTimeTable();
            }
            else
            {
                await DisplayAlert("Updating on Cooldown", "You can update in  " + TimeSpan.FromMinutes(Math.Round(refreshTime - (DateTime.Now - FileHandler.GetLastWriteTime(fileName)).TotalMinutes, 2)).ToString(@"mm\:ss") + "!", "OK :|");
            }

            updateView.IsRefreshing = false;
        }

        protected async override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var currentOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

            if(currentOrientation != deviceOrientation)
            {
                deviceOrientation = currentOrientation;
                deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
                deviceWidth = DeviceDisplay.MainDisplayInfo.Width;

                await BuildTimeTable();
            }
        }
    }
}