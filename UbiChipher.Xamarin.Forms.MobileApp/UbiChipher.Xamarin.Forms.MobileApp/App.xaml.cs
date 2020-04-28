using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UbiChipher.Xamarin.Forms.MobileApp.Services;
using UbiChipher.Xamarin.Forms.MobileApp.Views;

namespace UbiChipher.Xamarin.Forms.MobileApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
