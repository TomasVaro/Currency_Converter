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
            APIHelper.InitilizedClient();
            //ContentFrame.Navigate(typeof(CurrencyConverter));
            ContentFrame.Content = "\n  Välkommen till Valutaguiden! \n \n  Välj ett av alternativen i menyn till vänster";
            ContentFrame.FontSize = 40;
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
                    NavigationView.Header = "Länder och valutor";
                    break;
                case "currenciesCountries":
                    ContentFrame.Navigate(typeof(CurrenciesCountries));
                    NavigationView.Header = "Valutor och länder";
                    break;
                case "historical":
                    ContentFrame.Navigate(typeof(HistoricalExchangeRates));
                    NavigationView.Header = "Historiska valutakurser";
                    break;
            }
        }
    }
}