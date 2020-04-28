using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace UbiChipher.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        ZXingScannerPage scanPage;

        public ScanPage()
        {
            InitializeComponent();
            ButtonScanDefault.Clicked += ButtonScanDefault_ClickedAsync;
        }

        private async void ButtonScanDefault_ClickedAsync(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                //Do something with result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //await Navigation.PopAsync();
                    await Navigation.PopModalAsync();
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            //await Navigation.PushAsync(scanPage);
            await Navigation.PushModalAsync(scanPage);
        }
    }
}