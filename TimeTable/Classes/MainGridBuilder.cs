using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    class MainGridBuilder : ContentPage
    {
        private Thickness _frameMargin = new Thickness(2, 2, 2, 2);
        private Thickness _labelPadding = new Thickness(0, 0, 0, 0);
        private double _spacing = 0;
        ColorDatabase colorDatabase;
        TimeTableObj schedule = new TimeTableObj();

        public MainGridBuilder(Thickness frameMargin, Thickness labelPadding, double spacing)
        {
            this._frameMargin = frameMargin;
            this._labelPadding = labelPadding;
            this._spacing = spacing;
        }

        // Split in Days
        public Grid BuildMainGrid(TimeTableObj timetable, double[] boundaryTimes, GridLength width)
        {
            schedule = timetable;
            colorDatabase = ColorDatabase.GetColorDatabase(timetable.ColorScheme);

            Grid mainGrid = new Grid() { RowSpacing = _spacing + 1, ColumnSpacing = _spacing + 1 };
            int col = 0;

            foreach (var day in timetable.Days)
            {
                //Debug.WriteLine(day.Day);
                var sDay = sortedDay(day.Activities);

                Grid dayColumn = MakeDayColumn(sDay, boundaryTimes, _spacing);

                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = width });
                mainGrid.Children.Add(dayColumn);
                Grid.SetColumn(dayColumn, col++);
            }

            return mainGrid;
        }


        private Grid MakeDayColumn(List<Activity> sDay, double[] boundaryTimes, double _spacing)
        {
            Grid dayColumn = new Grid() { RowSpacing = _spacing, ColumnSpacing = _spacing };
            double startTime = boundaryTimes[0];
            double endTime = boundaryTimes[1];
            double startOfFirst = 0;
            double endOfLast = 0;
            double minFreetimeCellHeight = 0.5;
            int rowPlace = 0;

            for (int row = 0; row < sDay.Count; row++) // row: In der wievielten Veranstaltung bin ich
            {
                Grid rowGrid = new Grid() { RowSpacing = _spacing, ColumnSpacing = _spacing };
                int simultan = 0; // Could probably get this from Length of simultanActivities-List instead (2022.04.30)

                // Make List of activities in next time slot and get the boundary times
                List<Activity> simultanActivities = GetActivities(sDay, row);
                double[] activityGroupBoudaryTimes = GetActivityGroupBoudaryTimes(simultanActivities);
                startOfFirst = activityGroupBoudaryTimes[0];

                // Handle Gaps in day
                if ((row == 0 && sDay[row].Start > startTime) || (row > 0 && row < sDay.Count && sDay[row].Start > endOfLast)) // Before or after activities of the day
                {
                    double cellHeight = 0;
                    bool withFilling = false;
                    if (row == 0 && sDay[row].Start > startTime)
                    {
                        cellHeight = (sDay[row].Start - startTime) / (endTime - startTime);
                        withFilling = sDay[row].Start - startTime > minFreetimeCellHeight;
                    }
                    else if (row > 0 && row < sDay.Count && sDay[row].Start > endOfLast)
                    {
                        cellHeight = (startOfFirst - endOfLast) / (endTime - startTime);
                        withFilling = startOfFirst - endOfLast > minFreetimeCellHeight;
                    }
                    int fontSize = FontSizeCalculator.SetFontSize("Free time", cellHeight, schedule.Days.Count, 1);

                    Grid freetimeGrid = MakeFreetimeGrid(withFilling, fontSize, _spacing);

                    dayColumn.Children.Add(freetimeGrid);
                    dayColumn.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellHeight, GridUnitType.Star) });
                    Grid.SetRow(freetimeGrid, rowPlace);

                    rowPlace++;
                }

                endOfLast = activityGroupBoudaryTimes[1];

                // Handle events while taking simultaneous events into account
                for (int i = 0; i < simultanActivities.Count; i++) // simultane Veranstaltungen
                {
                    Grid colInRow = new Grid() { RowSpacing = _spacing, ColumnSpacing = _spacing };
                    Activity activity = simultanActivities[i];
                    int gapInSim = 0;


                    // Handle Gaps before simultan activities
                    if (i > 0 && activity.Start > startOfFirst)
                    {
                        double cellHeight = (activity.Start - startOfFirst) / (endOfLast - startOfFirst);
                        bool withFilling = (activity.Start - startOfFirst) > minFreetimeCellHeight;
                        int FreetimeFontSize = FontSizeCalculator.SetFontSize("Free time", cellHeight, schedule.Days.Count, simultanActivities.Count);

                        Grid freetimeGrid = MakeFreetimeGrid(withFilling, FreetimeFontSize, _spacing);

                        colInRow.Children.Add(freetimeGrid);
                        colInRow.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellHeight, GridUnitType.Star) });
                        Grid.SetRow(freetimeGrid, gapInSim);

                        gapInSim++;
                    }


                    // Make the actual Grid for the Event
                    double actualHeight = (activity.End - activity.Start) / (endOfLast - startOfFirst);
                    int activityFontSize = FontSizeCalculator.SetFontSize(activity.Topic, (activity.End - activity.Start) / (endTime - startTime), schedule.Days.Count, simultanActivities.Count);

                    Grid activityGrid = MakeActivityGrid(activity, activityFontSize, _spacing);

                    colInRow.Children.Add(activityGrid);
                    colInRow.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(actualHeight, GridUnitType.Star) });
                    Grid.SetRow(activityGrid, gapInSim);

                    gapInSim++;


                    // Handle Gaps after simultan Activities
                    if (i > 0 && activity.End < endOfLast)
                    {
                        double cellHeight = (endOfLast - activity.End) / (endOfLast - startOfFirst);
                        bool withFilling = (endOfLast - activity.End) > minFreetimeCellHeight;
                        int FreetimeFontSize = FontSizeCalculator.SetFontSize("Free time", cellHeight, schedule.Days.Count, simultanActivities.Count);

                        Grid freetimeGrid = MakeFreetimeGrid(withFilling, FreetimeFontSize, _spacing);

                        colInRow.Children.Add(freetimeGrid);
                        colInRow.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(cellHeight, GridUnitType.Star) });
                        Grid.SetRow(freetimeGrid, gapInSim);
                    }

                    rowGrid.Children.Add(colInRow);
                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.SetColumn(colInRow, i);



                    if (i != 0)
                    {
                        simultan++;
                    }
                }

                double height = (endOfLast - startOfFirst) / (endTime - startTime);

                dayColumn.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(height, GridUnitType.Star) });
                dayColumn.Children.Add(rowGrid);
                Grid.SetRow(rowGrid, rowPlace); // Korrektur bei simultanen Veranstaltungen oder gaps in Day
                                                // Debug.WriteLine(height + " bei " + col + "-" + rowPlace);

                rowPlace++;

                if ((row + simultan) == sDay.Count - 1 && sDay[row].End < endTime) ///////////////////////////////// Ersetzen durch Auslaufen der Farbe
                {
                    Grid missingGrid = new Grid() { RowSpacing = _spacing, ColumnSpacing = _spacing };
                    double missingHeight = (endTime - sDay[row].End) / (endTime - startTime);

                    dayColumn.Children.Add(missingGrid);
                    dayColumn.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(missingHeight, GridUnitType.Star) });
                    Grid.SetRow(missingGrid, rowPlace);
                }

                row = row + simultan;
                simultan = 0;
            }

            return dayColumn;
        }

        private Grid MakeActivityGrid(Activity activity, int activityFontSize, double spacing)
        {
            Grid activityGrid = new Grid() { RowSpacing = spacing, ColumnSpacing = spacing };

            var tapInfo = new TapGestureRecognizer();
            tapInfo.Tapped += async (s, evt) =>
            {
                await (App.Current.MainPage as NavigationPage).PushAsync(new ActivityInfoPage(activity, colorDatabase));
            };

            Label activityLabel = new Label()
            {
                Text = activity.Topic,
                Margin = _frameMargin,
                Padding = _labelPadding,
                FontSize = activityFontSize,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Frame activityFrame = new Frame()
            {
                Content = activityLabel,
                CornerRadius = 5,
                Margin = _frameMargin,
                Padding = _labelPadding,
                BackgroundColor = colorDatabase.getFrameColor(activity.Category),
                BorderColor = colorDatabase.getBorderColor(activity.Category)
            };

            activityFrame.GestureRecognizers.Add(tapInfo);

            activityGrid.Children.Add(activityFrame);
            Debug.WriteLine(activity.Topic + "   " + activity.Start);
            Console.WriteLine(activity.Topic + "   " + activity.Start);
            return activityGrid;
        }

        private Grid MakeFreetimeGrid(bool withFilling, int fontSize, double spacing)
        {
            Grid freetimeGrid = new Grid() { RowSpacing = spacing, ColumnSpacing = spacing };

            var freeInfo = new TapGestureRecognizer();
            freeInfo.Tapped += async (s, evt) =>
            {
                await (App.Current.MainPage as NavigationPage).PushAsync(new FreetimeInfoPage());
            };

            if (withFilling)
            {

                Label freetimeLabel = new Label()
                {
                    Text = "Free time",
                    FontSize = fontSize,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Frame freetimeFrame = new Frame()
                {
                    Content = freetimeLabel,
                    CornerRadius = 5,
                    Margin = _frameMargin,
                    Padding = _labelPadding,
                    BackgroundColor = colorDatabase.getFrameColor("FreeTime"),
                    BorderColor = colorDatabase.getBorderColor("FreeTime"),
                };

                freetimeFrame.GestureRecognizers.Add(freeInfo);

                freetimeGrid.Children.Add(freetimeFrame);
            }

            return freetimeGrid;
        }



        private double[] GetActivityGroupBoudaryTimes(List<Activity> simultanActivities)
        {
            double[] _activityGroupBoudaryTimes = new double[2];
            _activityGroupBoudaryTimes[0] = simultanActivities[0].Start;
            _activityGroupBoudaryTimes[1] = simultanActivities[0].End;


            foreach (var a in simultanActivities)
            {
                if (a.Start < _activityGroupBoudaryTimes[0])
                {
                    _activityGroupBoudaryTimes[0] = a.Start;
                }
                if (a.End > _activityGroupBoudaryTimes[1])
                {
                    _activityGroupBoudaryTimes[1] = a.End;
                }
            }

            return _activityGroupBoudaryTimes;
        }

        private List<Activity> GetActivities(List<Activity> sDay, int row)
        {
            List<Activity> simultanActivities = new List<Activity>();

            for (int i = row; i < sDay.Count
                                && sDay[row].Start <= sDay[i].Start
                                && sDay[row].End > sDay[i].Start; i++) // simultane Veranstaltungen
            {
                simultanActivities.Add(sDay[i]);
            }

            return simultanActivities;
        }

        private List<Activity> sortedDay(List<Activity> aktivitäten)
        {
            var sortListOb = aktivitäten.OrderBy(x => x.Start).ThenByDescending(x => x.End);
            var sortedString = JsonConvert.SerializeObject(sortListOb);
            return JsonConvert.DeserializeObject<List<Activity>>(sortedString);
        }
    }
}
