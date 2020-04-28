using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using UbiChipher.Data;
using UbiChipher.Services;

namespace WebsiteSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClaimValidationService claimValidationService;

        public MainWindow()
        {
            InitializeComponent();
            claimValidationService = new ClaimValidationService();
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            //var request = new Request() { ClaimRequests = new List<string>() { "Name" }, PostBackUri = "ubichipher.com/verify"};
            var request = new Request() { ClaimRequests = new List<string>() { "Name" }, PostBackUri = "http://localhost:51845/api/verify/claims" };

            var requestString = JsonSerializer.Serialize(request);
            RequestString.Text = requestString;

            RequestGenerationService requetGenerationService = new RequestGenerationService();
            var imageData = await requetGenerationService.CreateQRAsync(requestString);

            QRImage.Source = LoadImage(imageData);
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void ReadPostBack_Click(object sender, RoutedEventArgs e)
        {
            var textFromRESTApi = PostBackText.Text;

            claimValidationService.ValidateClaim(textFromRESTApi, out string hashOfClient, out string hashOnBlockChain, out bool match);

            ProcessResult.Text = match ?
                $"You are logged in, your claim fingerprint {hashOfClient} mathes fingerpring {hashOnBlockChain} on the blochain." :
                $"You NOT are logged in, your claim fingerprint {hashOfClient} does not match fingerpring {hashOnBlockChain} on the blochain.";
        }
    }
}
