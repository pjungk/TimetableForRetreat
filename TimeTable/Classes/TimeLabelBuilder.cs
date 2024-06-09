using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    class TimeLabelBuilder : ContentPage
    {
        Thickness _frameMargin = new Thickness(2, 2, 2, 2);
        Thickness _labelPadding = new Thickness(0, 0, 0, 0);
        double _spacing = 0;

        public TimeLabelBuilder(Thickness frameMargin, Thickness labelPadding, double spacing)
        {
            this._frameMargin = frameMargin;
            this._labelPadding = labelPadding;
            this._spacing = spacing;
        }

        public Grid BuildTimeLabel(double[] boundaryTimes, string propertiesFile, TimeTablePage timeTablePage)
        {
            List<double> times = new List<double>();

            double startTime = boundaryTimes[0];
            double endTime = boundaryTimes[1];
            times.Clear();

            Grid timeGrid = new Grid() { RowSpacing = _spacing + 1, ColumnSpacing = _spacing + 1 };

            if (startTime != Math.Floor(startTime))  //////////////////////////////////////////////////////////////////// This if-statement and the one after might be wrong
            {
                times.Add(startTime);
            }

            for (int time = (int)Math.Ceiling(startTime); time <= (int)Math.Floor(endTime); time++)
            {
                times.Add(time);
            }

            if (endTime != Math.Floor(endTime))
            {
                times.Add(endTime);
            }


            // TIMEGRID // TIMEGRID // TIMEGRID // TIMEGRID // TIMEGRID // TIMEGRID // TIMEGRID // TIMEGRID // TIMEGRID //
            for (int i = 0; i < times.Count - 1; i++)
            {
                string LabelText = "";

                if (times[i] != Math.Floor(times[i]))
                {
                    LabelText = TimeSpan.FromHours(startTime).ToString(@"h\:mm") + " - " + (int)times[i + 1];
                }
                else if (times[i + 1] != Math.Ceiling(times[i + 1]))
                {
                    LabelText = (int)times[i] + " - " + TimeSpan.FromHours(endTime).ToString(@"h\:mm");
                }
                else
                {
                    LabelText = (int)times[i] + " - " + (int)times[i + 1];
                }

                Grid hourGrid = new Grid() { RowSpacing = _spacing, ColumnSpacing = _spacing };
                Label hourLabel = new Label()
                {
                    Text = LabelText,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                Frame hourFrame = new Frame()
                {
                    Content = hourLabel,
                    CornerRadius = 2,
                    Margin = _frameMargin,
                    Padding = _labelPadding,
                    BackgroundColor = Color.Gray
                };


                if (i == 0)
                {
                    var newLinkTap = new TapGestureRecognizer();
                    newLinkTap.Tapped += async (s, evt) =>
                    {
                        string newUrl = await App.Current.MainPage.DisplayPromptAsync("Link ändern", "Neuer Link:");
                        if (!String.Equals(newUrl, "") && newUrl != null)
                        {
                            await FileHandler.SetProperty(propertiesFile, newUrl, "link");

                            await timeTablePage.BuildTimeTable();
                        }

                    };

                    hourFrame.GestureRecognizers.Add(newLinkTap);
                }

                hourGrid.Children.Add(hourFrame);

                timeGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength((times[i + 1] - times[i]) / (endTime - startTime), GridUnitType.Star) });
                timeGrid.Children.Add(hourGrid);
                Grid.SetRow(hourGrid, i);
            }

            return timeGrid;
        }
    }
}
