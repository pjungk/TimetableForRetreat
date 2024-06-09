using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    static class FontSizeCalculator
    {
        static int[] _possibleFontSizes = new int[3] { 10, 12, 20 };
        static double deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
        static double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        static double deviceDensity = DeviceDisplay.MainDisplayInfo.Density;
        static DisplayOrientation deviceOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

        static int homebarHeight = 57; // Ideally I can get this from the device
        static int statusbarHeight = 27; // Ideally I can get this from the device
        static double dayLabelHeight = 42; // Ideally I can get this from the XAML
        static double timeLabelHeight = 70; // Ideally I can get this from the XAML

        static double stringSizeFactor = 0.65;

        public static int SetFontSize(string topic, double cellHeight, int dayCount, int activityCount)
        {
            double deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
            double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
            double deviceDensity = DeviceDisplay.MainDisplayInfo.Density;
            DisplayOrientation deviceOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
            int fontSize = 7;

            double frameHeight = 0;
            double frameWidth = 0;

            /*
            if (deviceOrientation == DisplayOrientation.Landscape)
            {
                frameHeight = (int)Math.Floor((deviceHeight / deviceDensity - statusbarHeight - dayLabelHeight) * cellHeight);
                frameWidth = (int)Math.Floor((deviceWidth / deviceDensity - homebarHeight - timeLabelHeight) / dayCount / activityCount);
            }
            else
            {
                frameHeight = (int) Math.Floor((deviceHeight / deviceDensity - statusbarHeight - dayLabelHeight) * cellHeight);
                frameWidth = 169/activityCount;
            }
            */

            if (deviceOrientation != DisplayOrientation.Landscape)
            {
                frameHeight = (int)Math.Floor((deviceHeight / deviceDensity - statusbarHeight - dayLabelHeight) * cellHeight);
                frameWidth = 169 / activityCount;

                for (int tempFontSize = 25; tempFontSize >= 8; tempFontSize--)
                {
                    // Check for length of longest word
                    var words = topic.Split(new Char[] { ' ', '-', '/', '+' });
                    string longestWord = words.OrderByDescending(n => n.Length).First();
                    int longestWordLength = (int)Math.Ceiling(longestWord.Length * tempFontSize * stringSizeFactor);

                    int lines = (int)Math.Floor(frameHeight / (tempFontSize + 3));
                    int length = (int)Math.Ceiling(topic.Length * tempFontSize * stringSizeFactor);
                    int linesNeeded = (int)Math.Ceiling(length / frameHeight);

                    if (lines == words.Length && Math.Ceiling(length / frameHeight) % 1 < 0.3) // 2nd statement is supposed to test if the linesNeeded barely fit (cause then new lines, because of small words lead to more lines needed in actuality
                    {
                        linesNeeded++;
                    }
                    if (words.Length > 6) // 2nd statement is supposed to test if the linesNeeded barely fit (cause then new lines, because of small words lead to more lines needed in actuality
                    {
                        linesNeeded--;
                        if (words.Length > 10) // 2nd statement is supposed to test if the linesNeeded barely fit (cause then new lines, because of small words lead to more lines needed in actuality
                        {
                            linesNeeded--;
                        }
                    }

                    if (lines >= linesNeeded && longestWordLength <= frameWidth)
                    {
                        fontSize = tempFontSize;
                        break;
                    }
                }

                if (fontSize < 13)
                {
                    fontSize = 8;
                }
                else if (fontSize < 20)
                {
                    fontSize = 13;
                }
                else if (fontSize <= 25)
                {
                    fontSize = 20;
                }
            }
            else if (deviceOrientation == DisplayOrientation.Landscape)
            {
                fontSize = 6;
            }

            return fontSize;
        }
    }
}