using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UbiChipher.Data;
using UbiChipher.Infrastructure.Blockchain;


namespace EnrollmentTestUtil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Enrollment testEnrollment;

        public MainWindow()
        {
            InitializeComponent();

            CreateTestData();
        }


        private void Generate_Test_Enrollment_Button_Click(object sender, RoutedEventArgs e)
        {
            var json = JsonSerializer.Serialize(testEnrollment);
            EnrollmentJSONText.Text = json;
        }


        private void Enroll_Button_Click(object sender, RoutedEventArgs e)
        {
            var enrollment = JsonSerializer.Deserialize<Enrollment>(EnrollmentJSONText.Text);

            var enrollmentProcessor = new BlockchainEnrollmentProcessor();
            enrollmentProcessor.Enroll(enrollment);
        }

        #region Test Data
        private void CreateTestData()
        {
            testEnrollment = new Enrollment()
            {
                Claims = new List<Claim>()
                {
                    new Claim()
                    {
                        ClaimPairs = new Dictionary<string, string>()
                        {
                            { "Name", "Murray" }
                        },

                        RenewalDate = DateTime.Parse("2021-04-21T15:24:12.6242835Z"), // DateTime.UtcNow.AddDays(365),

                        PubKey = "mkHS9ne12qx9pS9VojpwU5xtRd4T7X7ZUt"
                    }
                },

                Intermediary = new Intermediary()
                {
                    Certainty = 80,
                    Certificate = "I am real, promise",
                    Id = "1",
                    Name = "Super Investments"
                }
            };
        }
        #endregion  
    }
}
