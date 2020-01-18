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
    public sealed partial class ListOfCountries : Page
    {
        public ListOfCountries()
        {
            this.InitializeComponent();
            getAllCountries();
        }

        Dictionary<string, Models.Countries> countries;
        private async void getAllCountries()
        {
            CountryDataProvider cdp = new CountryDataProvider();
            countries = await cdp.getAllCountriesUrl();
            ComboBoxItem countriesAll;

            foreach (var cur in countries.OrderBy(f => f.Value.Name))
            {
                countriesAll = new ComboBoxItem { Content = cur.Value.Name };
                CountryName.Items.Add(countriesAll);
            }
            CountryName.SelectedIndex = 0;
            var firstCounty = countries.OrderBy(kvp => kvp.Value.Name).First();

            CountryCode.Text = firstCounty.Value.Alpha3;
            CountryId.Text = firstCounty.Value.Id;
            CurrencyName.Text = firstCounty.Value.CurrencyName;
            CurrencyId.Text = firstCounty.Value.CurrencyId;
            CurrencySymbol.Text = firstCounty.Value.CurrencySymbol;
        }

        private void ShowCountries_OnDropDownClosed(object sender, object e)
        {
            ComboBoxItem selected = CountryName.SelectedItem as ComboBoxItem;
            string selectedCountry = selected.Content as String;

            foreach (var cur in countries.OrderBy(f => f.Value.Name))
            {
                if (cur.Value.Name == selectedCountry)
                {
                    CountryCode.Text = cur.Value.Alpha3;
                    CountryId.Text = cur.Value.Id;
                    CurrencyName.Text = cur.Value.CurrencyName;
                    CurrencyId.Text = cur.Value.CurrencyId;
                    CurrencySymbol.Text = cur.Value.CurrencySymbol;
                    break;
                }
            }
        }
    }
}
