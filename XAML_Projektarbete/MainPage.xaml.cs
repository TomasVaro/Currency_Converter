using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XAML_Projektarbete.DataProvider;

namespace XAML_Projektarbete
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(CurrencyConverter));
            NavigationView.Header = "Valutaomräknare";
        }

        private void MenuSelected(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = NavigationView.SelectedItem as NavigationViewItem;
            switch (item.Tag)
            {
                case "converter":
                    ContentFrame.Navigate(typeof(CurrencyConverter));
                    NavigationView.Header = "Valutaomräknare";
                    break;
                case "currency":
                    ContentFrame.Navigate(typeof(ListOfCurrencies));
                    NavigationView.Header = "Lista på valutor";
                    break;
                case "countries":
                    ContentFrame.Navigate(typeof(ListOfCountries));
                    NavigationView.Header = "Lista på länder";
                    break;
                case "historical":
                    ContentFrame.Navigate(typeof(HistoricalExchangeRates));
                    NavigationView.Header = "Historiska valutakurser";
                    break;
            }
        }
    }
}