using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FreetimeInfoPage : ContentPage
	{
		public FreetimeInfoPage()
		{
			InitializeComponent();


			Image image = new Image
			{
				Source = ImageSource.FromResource("TimeTable.resources.images.freeTime.jpg", typeof(FreetimeInfoPage).GetTypeInfo().Assembly),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(5, 5, 5, 5)
			};

			rootGrid.Children.Add(image);
			Grid.SetRow(image, 1);
		}
	}
}