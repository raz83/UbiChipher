using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using UbiChipher.Data;
using Newtonsoft.Json;
using System.Text;
using UbiChipher.Services;

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
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");

                    Request request = JsonConvert.DeserializeObject<Request>(result.Text);

                    {// Debug

                        await DisplayAlert("Requested Claims", string.Join(",", request.ClaimRequests.ToArray()), "OK");
                        await DisplayAlert("Reply Url", request.PostBackUri, "OK");
                    }// Debug

                    ClaimGenerationService claimGenerationService = new ClaimGenerationService();
                    string claim = claimGenerationService.GenerateClaim(result.Text);
                    string errorMessage =  await claimGenerationService.SubmitClaim(request, claim);

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        await DisplayAlert("Error", errorMessage, "OK");
                    }

                });
            };

            //await Navigation.PushAsync(scanPage);
            await Navigation.PushModalAsync(scanPage);
        }

    }
}