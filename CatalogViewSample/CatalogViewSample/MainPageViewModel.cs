using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CatalogViewSample
{
    public class MainPageViewModel : BindingBase
    {
        private List<ItemDto> _catalogItemSource;
        public List<ItemDto> CatalogItemSource
        {
            get { return _catalogItemSource; }
            set { _catalogItemSource = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public MainPageViewModel()
        {
            AddCommand = new Command<ItemDto>((item) =>
            {
                if (item == null)
                    return;
                item.ProductBasketCount++;
            });
            RemoveCommand = new Command<ItemDto>((item) =>
            {
                if (item == null)
                    return;
                item.ProductBasketCount--;
            });

            CatalogItemSource = new List<ItemDto>();

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (int i = 0; i < 50; i++)
            {
                CatalogItemSource.Add(new ItemDto
                {
                    Name = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray()),
                    DiscountedPrice = 3m,
                    Price = 5m,
                    ImageName = "https://source.unsplash.com/random",
                    Quantity = "Liter"
                });
            }
        }
    }

}
