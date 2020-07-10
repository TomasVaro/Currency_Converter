using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using XAML_Projektarbete.DataProvider;

namespace XAML_Projektarbete
{
    public sealed partial class ListOfCurrencies : Page
    {
        public ListOfCurrencies()
        {
            this.InitializeComponent();
            getAllCurrencies();
        }

        Dictionary<string, Models.Currency> currencies;
        private async void getAllCurrencies()
        {
            CurrencyDataProvider cdp = new CurrencyDataProvider();
            ComboBoxItem currenciesAll;
            try
            {
                currencies = await cdp.GetCurrencies();
            }
            catch
            {
                return;
            }

            foreach (var cur in currencies.OrderBy(f => f.Value.CurrencyName))
            {
                currenciesAll = new ComboBoxItem { Content = cur.Value.CurrencyName };
                Currencies.Items.Add(currenciesAll);
            }
            Currencies.SelectedIndex = 0;
            try
            {
                var firstCurrency = currencies.OrderBy(kvp => kvp.Value.CurrencyName).First();
                CurrencyId.Text = firstCurrency.Value.Id;
                CurrencySymbol.Text = firstCurrency.Value.CurrencySymbol;
            }
            catch
            {
                return;
            }
        }

        private void ShowCurrency_OnDropDownClosed(object sender, object e)
        {
            ComboBoxItem selected = Currencies.SelectedItem as ComboBoxItem;
            try
            {
                string selectedCurrency = selected.Content as String;
                foreach (var cur in currencies.OrderBy(f => f.Value.CurrencyName))
                {
                    if (cur.Value.CurrencyName == selectedCurrency)
                    {
                        CurrencyId.Text = cur.Value.Id;
                        CurrencySymbol.Text = cur.Value.CurrencySymbol ?? "Symbol saknas";
                        break;
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}
