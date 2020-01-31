using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
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
            try
            {
                countries = await cdp.getAllCountriesUrl();
                CountryName.SelectedIndex = 0;
                var firstCounty = countries.OrderBy(kvp => kvp.Value.Name).First();

                CountryCode.Text = firstCounty.Value.Alpha3;
                CountryId.Text = firstCounty.Value.Id;
                CurrencyName.Text = firstCounty.Value.CurrencyName;
                CurrencyId.Text = firstCounty.Value.CurrencyId;
                CurrencySymbol.Text = firstCounty.Value.CurrencySymbol;
            }
            catch
            {
                return;
            }

            ComboBoxItem countriesAll;
            foreach (var cur in countries.OrderBy(f => f.Value.Name))
            {
                countriesAll = new ComboBoxItem { Content = cur.Value.Name };
                CountryName.Items.Add(countriesAll);
            }
        }

        private void ShowCountries_OnDropDownClosed(object sender, object e)
        {
            ComboBoxItem selected = CountryName.SelectedItem as ComboBoxItem;
            try
            {
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
            catch
            {
                return;
            }
        }
    }
}
