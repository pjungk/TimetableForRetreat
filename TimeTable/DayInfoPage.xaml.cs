using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTable.Classes;
using Xamarin.Essentials;

namespace TimeTable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayInfoPage : ContentPage
    {
        double deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        double deviceDensity = DeviceDisplay.MainDisplayInfo.Density;
        DisplayOrientation deviceOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

        List<RetreatSupervisor> orgaTeam = new List<RetreatSupervisor>();
        string supervisors = "";

        public DayInfoPage(List<RetreatSupervisor> orgaTeam, string supervisors)
        {
            InitializeComponent();

            this.orgaTeam = orgaTeam;
            this.supervisors = supervisors;

            FillDayInfoPage();
        }

        private void FillDayInfoPage() // Gotta change this method depending on Orientation
        {
            root.Children.Clear();
            if(deviceOrientation == DisplayOrientation.Landscape)
            {
                root.Orientation = StackOrientation.Horizontal;
            }
            else
            {
                root.Orientation = StackOrientation.Vertical;
            }

            if (orgaTeam != null)
            {
                foreach (var dailySupervisor in orgaTeam)
                {
                    if (supervisors.Contains(dailySupervisor.Name))
                    {
                        StackLayout personStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };
                        string url = dailySupervisor.FileUrl.Remove(dailySupervisor.FileUrl.Length-1,1) + "1";

                        Image webImage = new Image();
                        webImage.Source = new UriImageSource
                        {
                            CachingEnabled = true,
                            CacheValidity = new TimeSpan(10, 0, 0, 0)
                        };
                        webImage = new Image { Source = ImageSource.FromUri(new Uri(url)) };

                        personStackLayout.Children.Add(webImage);


                        Label label = new Label { Text = dailySupervisor.Name, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = 30 };

                        personStackLayout.Children.Add(label);


                        root.Children.Add(personStackLayout);
                    }
                }
            }
            else
            {
                Label label = new Label { Text = supervisors, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = 30 };

                root.Children.Add(label);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var currentOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

            if (currentOrientation != deviceOrientation)
            {
                deviceOrientation = currentOrientation;
                deviceHeight = DeviceDisplay.MainDisplayInfo.Height;
                deviceWidth = DeviceDisplay.MainDisplayInfo.Width;

                FillDayInfoPage();
            }
        }
    }
}