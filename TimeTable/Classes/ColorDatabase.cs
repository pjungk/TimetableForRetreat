using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    public sealed class ColorDatabase
    {
        private List<CategoryColors> colors = new List<CategoryColors>();
        private Color defaultColor = new Color(); // Could potentially define a specific defaultColor
        private double luminosity = 0.8;

        private static ColorDatabase _instance;

        private ColorDatabase(List<CategoryColors> colorScheme)
        {
            List<CategoryColors> emergencyList = new List<CategoryColors>();
            emergencyList.Add(new CategoryColors { CategoryName = "default", ColorCode = "#1b7e4b" });

            if (colorScheme == null)
            {
                colorScheme = emergencyList;
            }

            foreach (var combo in colorScheme)
            {
                if (combo.CategoryName.ToLower().Equals("default"))
                {
                    defaultColor = Color.FromHex(combo.ColorCode);
                }
            }

            this.colors = colorScheme;
        }

        public static ColorDatabase GetColorDatabase(List<CategoryColors> colorScheme)
        {
            if (_instance == null)
            {
                _instance = new ColorDatabase(colorScheme);
            }

            return _instance;
        }




        public Color[] getColorScheme(string category)
        {
            Color[] colorScheme = new Color[2];

            colorScheme[0] = getFrameColor(category);
            colorScheme[1] = getBorderColor(colorScheme[1]);

            return colorScheme;
        }

        public Color getFrameColor(string category)
        {
            Color frameColor = new Color();

            foreach (var combo in colors)
            {
                if (category != null && combo.CategoryName.Equals(category))
                {
                    frameColor = Color.FromHex(combo.ColorCode);
                }
            }
            if (frameColor == null || frameColor == Color.Default)
            {
                if (colors.Count == 0)
                {
                    frameColor = defaultColor;
                }
                else
                {
                    frameColor = Color.FromHex(colors[0].ColorCode);
                }
            }

            return frameColor;
        }

        public Color getBorderColor(string category)
        {
            Color frameColor = getFrameColor(category);

            return frameColor.WithLuminosity(frameColor.Luminosity * luminosity);
        }

        private Color getBorderColor(Color frameColor)
        {
            return frameColor.WithLuminosity(frameColor.Luminosity * luminosity);
        }
    }
}
