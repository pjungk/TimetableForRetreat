using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityInfoPage : ContentPage
    {
        public ActivityInfoPage(Activity activity, ColorDatabase colorDatabase)
        {
            InitializeComponent();

            var remark = activity.Remark;
            var start = activity.Start;
            var end = activity.End;
            var theme = activity.Topic;
            var place = activity.Location;
            var guides = activity.Instructor;
            var category = activity.Category;
            var description = activity.Description;

            if (remark != null)
            {
                achtung.Height = GridLength.Auto;
                remarkLbl.Text = "Achtung: " + remark;
            }

            if (String.Equals(theme, "Daily Gathering"))
            {
                start = 10.75;
            }

            string startToString = TimeSpan.FromHours(start).ToString(@"hh\:mm");
            string endToString = TimeSpan.FromHours(end).ToString(@"hh\:mm");

            themeLblQ.Text = "Was?";
            themeLblA.Text = theme;
            themeLblQ.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
            themeLblA.BackgroundColor = colorDatabase.getFrameColor(activity.Category);

            timeLblQ.Text = "Wann?";
            timeLblA.Text = startToString + " - " + endToString;
            timeLblQ.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
            timeLblA.BackgroundColor = colorDatabase.getFrameColor(activity.Category);

            placeLblQ.Text = "Wo?";
            placeLblA.Text = place;
            placeLblQ.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
            placeLblA.BackgroundColor = colorDatabase.getFrameColor(activity.Category);

            guidesLblQ.Text = "Wer?";
            guidesLblA.Text = guides;
            guidesLblQ.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
            guidesLblA.BackgroundColor = colorDatabase.getFrameColor(activity.Category);

            descriptionLblQ.Text = "Was passiert?";
            descriptionLblA.Text = description;
            descriptionLblQ.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
            descriptionLblA.BackgroundColor = colorDatabase.getFrameColor(activity.Category);

            categoryLblQ.Text = "Category:";
            categoryLblA.Text = category;
            categoryLblQ.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
            categoryLblA.BackgroundColor = colorDatabase.getFrameColor(activity.Category);
        }
    }
}