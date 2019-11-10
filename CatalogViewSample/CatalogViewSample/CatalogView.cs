using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CatalogViewSample
{
    public class CatalogView : ScrollView
    {
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
             nameof(ColumnSpacing),
             typeof(double),
             typeof(CatalogView));

        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
            nameof(RowSpacing),
            typeof(double),
            typeof(CatalogView));

        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        public static readonly BindableProperty ShapeWidthProperty = BindableProperty.Create(
          nameof(ShapeWidth),
          typeof(int),
          typeof(CatalogView),
          60);

        public int ShapeWidth
        {
            get { return (int)GetValue(ShapeWidthProperty); }
            set { SetValue(ShapeWidthProperty, value); }
        }

        public static readonly BindableProperty ShapeHeightProperty = BindableProperty.Create(
            nameof(ShapeHeight),
            typeof(int),
            typeof(CatalogView),
            100);
        public int ShapeHeight
        {
            get { return (int)GetValue(ShapeHeightProperty); }
            set { SetValue(ShapeHeightProperty, value); }
        }

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
          nameof(ItemSource),
          typeof(IList),
          typeof(CatalogView),
          null,
          defaultBindingMode: BindingMode.TwoWay,
          propertyChanged: OnItemsSourceChanged);

        public IList ItemSource
        {
            get { return (IList)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        public DataTemplate ItemTemplate { get; set; }

        private static void OnItemsSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                if (bindableObject is CatalogView catalogView && catalogView != null && catalogView.ItemTemplate != null)
                {
                    catalogView.CreateLayout();
                }
            }
        }

        private void CreateLayout()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Content = GetContentGrid();
                    break;
                case Device.Android:
                    var contentGrid = Task.Run(GetContentGrid);
                    Device.BeginInvokeOnMainThread(async () => { Content = await contentGrid; });
                    break;
            }
        }
        private Grid GetContentGrid()
        {
            var contentGrid = new Grid { ColumnSpacing = ColumnSpacing, RowSpacing = RowSpacing };
            var parentWidth = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density - Margin.HorizontalThickness;
            var availableColumnCount = Convert.ToInt32(Math.Floor(parentWidth / ShapeWidth));
            int availableRowCount = 0;
            if (availableColumnCount != 0)
            {
                availableRowCount += Convert.ToInt32(ItemSource.Count / availableColumnCount);
                while (availableRowCount * availableColumnCount <= ItemSource.Count)
                {
                    availableRowCount++;
                }
            }
            for (int i = 0; i < availableColumnCount; i++)
            {
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = ShapeWidth });
            }

            if (availableRowCount != 0)
            {
                for (int i = 0; i < availableRowCount; i++)
                {
                    contentGrid.RowDefinitions.Add(new RowDefinition { Height = ShapeHeight });
                }
            }
            int loopRowCount = 0;
            int loopColumnCount = -1;
            if (availableRowCount == 0)
                loopRowCount--;
            for (int i = 0; i < ItemSource.Count; i++)
            {
                loopColumnCount++;
                if (loopColumnCount < availableColumnCount && loopColumnCount != 0)
                {
                    contentGrid.Children.Add((View)ItemTemplate.CreateContent());
                    contentGrid.Children[i].BindingContext = ItemSource[i];
                    Grid.SetColumn(contentGrid.Children[i], loopColumnCount);
                    if (loopRowCount != 0)
                        Grid.SetRow(contentGrid.Children[i], loopRowCount);
                }
                else if (loopColumnCount == availableColumnCount)
                {
                    contentGrid.Children.Add((View)ItemTemplate.CreateContent());
                    contentGrid.Children[i].BindingContext = ItemSource[i];
                    loopRowCount++;
                    loopColumnCount = 0;
                    Grid.SetRow(contentGrid.Children[i], loopRowCount);
                }
                else
                {
                    contentGrid.Children.Add((View)ItemTemplate.CreateContent());
                    contentGrid.Children[i].BindingContext = ItemSource[i];
                    Grid.SetRow(contentGrid.Children[i], loopRowCount);
                    Grid.SetColumn(contentGrid.Children[i], loopColumnCount);
                }
            }
            return contentGrid;
        }
    }
}
