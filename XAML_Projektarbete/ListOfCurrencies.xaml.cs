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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XAML_Projektarbete
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListOfCurrencies : Page
    {
        private string apiKey = "37caa54a777a956b193b";
        public ListOfCurrencies()
        {
            this.InitializeComponent();
            getAllCurrencies();
        }

        private async void getAllCurrencies()
        {
            ConvertDataProvider cdp = new ConvertDataProvider();
            var currencies = await cdp.GetCurrencies();
            ComboBoxItem currenciesAll;

            foreach (var cur in currencies.OrderBy(f => f.Value.CurrencyName))
            {
                currenciesAll = new ComboBoxItem { Content = cur.Value.CurrencyName };
                Currencies.Items.Add(currenciesAll);
            }
            Currencies.SelectedIndex = 0; // Väljer default valuta
            var firstCurrency = currencies.OrderBy(kvp => kvp.Value.CurrencyName).First();
            CurrencyId.Text = firstCurrency.Value.Id;
            CurrencySymbol.Text = firstCurrency.Value.CurrencySymbol;
        }

        private void ShowCurrency_OnDropDownClosed(object sender, object e)
        {

        }
    }
}
