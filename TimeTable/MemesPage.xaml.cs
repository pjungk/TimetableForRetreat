using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTable.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Core;

namespace TimeTable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MemesPage : ContentPage
    {
        public MemesPage(List<Meme> memeList)
        {
            InitializeComponent();

            var sortedMemeList = SortMemes(memeList);

            DisplayMemes(sortedMemeList);
        }

        private void DisplayMemes(List<Meme> memeList) // Possibly Frames that lead to an extra Contentpage with the MediaElement
        {
            foreach (var meme in memeList)
            {
                if (meme.FileType.ToLower().Equals("audio"))
                {
                    MediaElement mediaElement = MakeMediaElement(meme);

                    root.Children.Add(mediaElement);
                }
                else if (meme.FileType.ToLower().Equals("image"))
                {
                    string url = meme.FileUrl.Remove(meme.FileUrl.Length - 1, 1) + "1";

                    Image webImage = new Image();
                    webImage.Source = new UriImageSource
                    {
                        CachingEnabled = true,
                        CacheValidity = new TimeSpan(10, 0, 0, 0)
                    };
                    webImage.VerticalOptions = LayoutOptions.FillAndExpand;
                    webImage = new Image { Source = ImageSource.FromUri(new Uri(url)) };

                    root.Children.Add(webImage);
                }
                else if (meme.FileType.ToLower().Equals("video"))
                {
                    MediaElement mediaElement = MakeMediaElement(meme);

                    root.Children.Add(mediaElement);
                }
                else if (meme.FileType.ToLower().Equals("text"))
                {
                    string text = meme.Description;


                    Label activityLabel = new Label()
                    {
                        Text = text,
                        FontSize = 13,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    Frame activityFrame = new Frame()
                    {
                        Content = activityLabel,
                        Padding = new Thickness(2, 2, 2, 2),
                        Margin = new Thickness(2, 2, 2, 2),
                        CornerRadius = 1,
                        BackgroundColor = Color.FromHex("#CCCCCC"),
                        BorderColor = Color.FromHex("#AAAAAA")
                    };

                    root.Children.Add(activityFrame);
                }
            }
        }

        private MediaElement MakeMediaElement(Meme meme)
        {
            string url = meme.FileUrl.Remove(meme.FileUrl.Length - 1, 1) + "1";
            bool autoPlay = false;

            MediaElement mediaElement = new MediaElement();

            if (meme.FileType.ToLower().Equals("audio"))
            {
                mediaElement.HeightRequest = 85;
            }
            else if (meme.FileType.ToLower().Equals("video"))
            {
                mediaElement.VerticalOptions = LayoutOptions.FillAndExpand;
            }

            mediaElement.AutoPlay = autoPlay;
            mediaElement.Source = new UriMediaSource { Uri = new Uri(url) };

            //CacheMediaElement();

            return mediaElement;
        }

        private void CacheMediaElement()
        {
            // Save File locally

            var propertiesEntry = new savedMemes
            {
                url = "",
                localFile = ""
            };
        }

        private List<Meme> SortMemes(List<Meme> memeList)
        {
            var wrongFormatsortedMemeList = memeList.OrderBy(x => x.FileType);
            var sortedMemeString = JsonConvert.SerializeObject(wrongFormatsortedMemeList);
            return JsonConvert.DeserializeObject<List<Meme>>(sortedMemeString);
        }
    }


    class savedMemes
    {
        public string url { get; set; }
        public string localFile { get; set; }
    }
}