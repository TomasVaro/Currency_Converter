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
    public sealed partial class HistoricalExchangeRates : Page
    {
        public HistoricalExchangeRates()
        {
            this.InitializeComponent();
            DatePicker.PlaceholderText = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day).ToString();
            getCurrencies();
        }

        private async void getCurrencies()
        {
            ConvertDataProvider cdp = new ConvertDataProvider();
            var currencies = await cdp.GetCurrencies();
            ComboBoxItem currenciesFrom;
            ComboBoxItem currenciesTo;

            foreach (var cur in currencies.OrderBy(e => e.Value.CurrencyName))
            {
                if (cur.Value.CurrencySymbol == null)
                {
                    currenciesFrom = new ComboBoxItem { Content = cur.Key + "   " + cur.Value.CurrencyName };
                    currenciesTo = new ComboBoxItem { Content = cur.Key + "   " + cur.Value.CurrencyName };
                }
                else
                {
                    currenciesFrom = new ComboBoxItem { Content = cur.Key + "   " + cur.Value.CurrencyName + " (" + cur.Value.CurrencySymbol + ")" };
                    currenciesTo = new ComboBoxItem { Content = cur.Key + "   " + cur.Value.CurrencyName + " (" + cur.Value.CurrencySymbol + ")" };
                }
                CurrenciesFrom.Items.Add(currenciesFrom);
                CurrenciesTo.Items.Add(currenciesTo);
            }
            CurrenciesFrom.SelectedIndex = 156; // Väljer default valuta
            CurrenciesTo.SelectedIndex = 143;
            AmountFrom.Focus(FocusState.Programmatic);  //Sets focus on Textbox AmountFrom
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs e)
        {
            // Checks maximum AmountFrom length and allows only digits and dots
            if (e.NewText.Length < 11 && e.NewText.All(c => char.IsDigit(c) || c == '.'))
            {
                if (AmountFrom.Text.Length == 0)
                {
                    AmountFrom.Select(e.NewText.Length, 0);   //Sets cursor at the end of textbox
                }
                ConvertCurrency(e.NewText, DatePicker.PlaceholderText);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void SwitchButton(object sender, RoutedEventArgs e)
        {
            int currencyFrom = CurrenciesTo.SelectedIndex;
            CurrenciesTo.SelectedIndex = CurrenciesFrom.SelectedIndex;
            CurrenciesFrom.SelectedIndex = currencyFrom;
            ConvertCurrency(AmountFrom.Text, DatePicker.PlaceholderText);
        }


        string lastFromCurrency = string.Empty;
        string lastToCurrency = string.Empty;
        string lastDate = string.Empty;
        double exchangeRate;
        private async void ConvertCurrency(string amount, string date)
        {
            ComboBoxItem from = CurrenciesFrom.SelectedItem as ComboBoxItem;
            string fromCurrency = (from.Content as String).Substring(0, 3);

            ComboBoxItem to = CurrenciesTo.SelectedItem as ComboBoxItem;
            string toCurrency = (to.Content as String).Substring(0, 3);

            ConvertDataProvider cdp = new ConvertDataProvider();
            if ((fromCurrency != lastFromCurrency || toCurrency != lastToCurrency) && (fromCurrency != toCurrency) || (date != lastDate))
            {
                exchangeRate = await cdp.GetHistoricalExchangeRate(fromCurrency, toCurrency, date);
                lastFromCurrency = fromCurrency;
                lastToCurrency = toCurrency;
                lastDate = date;
            }

            if (fromCurrency == toCurrency)
            {
                AmountTo.Text = amount;
            }
            // Checks if AmountFrom textbox is empty or not
            else if (double.TryParse(amount, out double result))
            {
                if (result * exchangeRate > 0.01)
                {
                    AmountTo.Text = string.Format("{0:#,###0.00}", result * exchangeRate);
                }
                else
                {
                    decimal resultInDecimal = (decimal)(result * exchangeRate);
                    AmountTo.Text = resultInDecimal.ToString();
                }
            }
            else
            {
                AmountTo.Text = "";
            }
            AmountFrom.Focus(FocusState.Programmatic);  //Sets focus on Textbox AmountFrom
        }

        private void ChangeCurrency_OnDropDownClosed(object sender, object e)
        {
            if (AmountFrom.Text != string.Empty)
            {
                ConvertCurrency(AmountFrom.Text, DatePicker.PlaceholderText);
            }
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var dateTime = (DateTimeOffset)(DatePicker.Date).Value.Date;
            string date = (dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day).ToString();
            ConvertCurrency(AmountFrom.Text, date);
        }
    }
}
