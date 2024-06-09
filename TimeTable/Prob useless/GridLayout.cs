using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeTable.Classes
{
    public class GridLayout
    {
        public double width { get; }
        public double height { get; }
        public Thickness FrameMargin { get; }
        public Thickness LabelPadding { get; }
        public double spacing { get; }

        public GridLayout(double width, double height, Thickness frameMargin, Thickness labelPadding, double spacing)
        {
            this.width = width;
            this.height = height;
            this.FrameMargin = frameMargin;
            this.LabelPadding = labelPadding;
            this.spacing = spacing;
        }

        public GridLayout(double width, double height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
