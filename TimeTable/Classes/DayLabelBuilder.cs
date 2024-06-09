using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    class DayLabelBuilder : ContentPage
    {
        Thickness _frameMargin = new Thickness(2, 2, 2, 2);
        Thickness _labelPadding = new Thickness(0, 0, 0, 0);
        double _spacing = 0;

        public DayLabelBuilder(Thickness frameMargin, Thickness labelPadding, double spacing)
        {
            this._frameMargin = frameMargin;
            this._labelPadding = labelPadding;
            this._spacing = spacing;
        }

        public Grid BuildDayLabel(TimeTableObj schedule, GridLength width)
        {
            List<Day> splits = schedule.Days;
            Grid dayGrid = new Grid() { RowSpacing = _spacing + 1, ColumnSpacing = _spacing + 1 };
            // DAYGRID // DAYGRID // DAYGRID // DAYGRID // DAYGRID // DAYGRID // DAYGRID // DAYGRID // DAYGRID // DAYGRID //
            for (int i = 0; i < splits.Count; i++)
            {
                var day = splits[i];
                var dayBtn = new TapGestureRecognizer();
                dayBtn.Tapped += async (s, evt) =>
                {
                    await (App.Current.MainPage as NavigationPage).PushAsync(new DayInfoPage(schedule.RetreatSupervisors, day.DailySupervisor));
                };
                // Show day
                Grid showDay = new Grid() { RowSpacing = _spacing, ColumnSpacing = _spacing };
                Label dayLbl = new Label() { Text = day.Weekday, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
                Frame dayFr = new Frame()
                {
                    Content = dayLbl,
                    CornerRadius = 2,
                    Margin = _frameMargin,
                    Padding = _labelPadding,
                    BackgroundColor = Color.DarkGray
                };

                dayFr.GestureRecognizers.Add(dayBtn);

                showDay.Children.Add(dayFr);

                dayGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = width });
                dayGrid.Children.Add(showDay);
                Grid.SetColumn(showDay, i);
            }

            return dayGrid;
        }
    }
}
