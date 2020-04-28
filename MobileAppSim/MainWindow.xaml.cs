using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using UbiChipher.Data;
using UbiChipher.Services;

namespace MobileAppSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClaimGenerationService claimGenerationService;
        string postbackContent;

        public MainWindow()
        {
            InitializeComponent();
            claimGenerationService = new ClaimGenerationService();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            var QRText = InputText.Text;

            postbackContent = claimGenerationService.GenerateClaim(QRText);

            PostBackText.Text = postbackContent;
        }

        private async void HTTPPostButton_Click(object sender, RoutedEventArgs e)
        {
            Request request = JsonConvert.DeserializeObject<Request>(InputText.Text);
            string error = await claimGenerationService.SubmitClaim(request, postbackContent);
        }
    }
}
