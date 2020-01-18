using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            ConvertDataProvider cdp = new ConvertDataProvider();
            currencies = await cdp.GetCurrencies();
            ComboBoxItem currenciesAll;

            foreach (var cur in currencies.OrderBy(f => f.Value.CurrencyName))
            {
                currenciesAll = new ComboBoxItem { Content = cur.Value.CurrencyName };
                Currencies.Items.Add(currenciesAll);
            }
            Currencies.SelectedIndex = 0;
            var firstCurrency = currencies.OrderBy(kvp => kvp.Value.CurrencyName).First();
            CurrencyId.Text = firstCurrency.Value.Id;
            CurrencySymbol.Text = firstCurrency.Value.CurrencySymbol;
        }

        private void ShowCurrency_OnDropDownClosed(object sender, object e)
        {
            ComboBoxItem selected = Currencies.SelectedItem as ComboBoxItem;
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
    }
}
