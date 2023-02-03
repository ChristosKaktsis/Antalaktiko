using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactPage : ContentPage
	{
		public ContactPage (string url)
		{
			InitializeComponent ();
            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = url,
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    
                    webView
                }
            };
        }
	}
}