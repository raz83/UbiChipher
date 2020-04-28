using System;

using UbiChipher.Xamarin.Forms.MobileApp.Models;

namespace UbiChipher.Xamarin.Forms.MobileApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
