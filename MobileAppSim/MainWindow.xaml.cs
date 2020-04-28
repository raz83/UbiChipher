using System.Windows;
using UbiChipher.Services;

namespace MobileAppSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClaimGenerationService claimGenerationService;

        public MainWindow()
        {
            InitializeComponent();
            claimGenerationService = new ClaimGenerationService();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            var QRText = InputText.Text;

            string postbackContent = claimGenerationService.GenerateClaim(QRText);

            PostBackText.Text = postbackContent;
        }
    }
}
