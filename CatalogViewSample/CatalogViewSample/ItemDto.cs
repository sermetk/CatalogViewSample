using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogViewSample
{
    public class ItemDto : BindingBase
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ImageName { get; set; }
        private int _productBasketCount;
        public int ProductBasketCount
        {
            get { return _productBasketCount; }
            set { _productBasketCount = value; OnPropertyChanged(); }
        }
    }
}
