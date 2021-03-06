﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Windows.UI.Xaml.Controls;
using XAML_Projektarbete.DataProvider;

namespace XAML_Projektarbete
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            APIHelper.InitilizedClient();
            string apiKey = "37caa54a777a956b193b";
            CountryDataProvider.ApiKey = apiKey;
            CurrencyDataProvider.ApiKey = apiKey;
            ExchangeRateDataProvider.ApiKey = apiKey;
            HistoricalExchangeRateDataProvider.ApiKey = apiKey;
        }

        private void MenuSelected(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = NavigationView.SelectedItem as NavigationViewItem;
            switch (item.Tag)
            {
                case "converter":
                    ContentFrame.Navigate(typeof(CurrencyConverter));
                    break;
                case "currency":
                    ContentFrame.Navigate(typeof(ListOfCurrencies));
                    break;
                case "countries":
                    ContentFrame.Navigate(typeof(ListOfCountries));
                    break;
                case "currenciesCountries":
                    ContentFrame.Navigate(typeof(CurrenciesCountries));
                    break;
                case "historical":
                    ContentFrame.Navigate(typeof(HistoricalExchangeRates));
                    break;
            }
        }
    }
}