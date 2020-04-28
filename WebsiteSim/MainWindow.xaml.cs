using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
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

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new Request() { ClaimRequests = new List<string>() { "Name" }, PostBackUri = "ubichipher.com/verify"};

            var requestString = JsonSerializer.Serialize(request);
            RequestString.Text = requestString;
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
