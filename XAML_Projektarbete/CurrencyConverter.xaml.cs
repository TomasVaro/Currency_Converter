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
    public sealed partial class CurrencyConverter : Page
    {
        string lastFromCurrency = string.Empty;
        string lastToCurrency = string.Empty;
        string lastDate = string.Empty;
        double exchangeRate;
        DateTimeOffset lastDateTime = new DateTimeOffset();

        public CurrencyConverter()
        {
            this.InitializeComponent();
            DatePicker.PlaceholderText = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day).ToString();
            getCurrencies();
        }

        private async void getCurrencies()
        {
            CurrencyDataProvider cdp = new CurrencyDataProvider();
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
            CurrenciesFrom.SelectedIndex = 156;
            CurrenciesTo.SelectedIndex = 143;
            AmountFrom.Focus(FocusState.Programmatic);
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs e)
        {
            // Checks maximum AmountFrom length and allows only digits and dots
            if (e.NewText.Length < 11 && e.NewText.All(c => char.IsDigit(c) || c == '.'))
            {
                if (AmountFrom.Text.Length == 0)
                {
                    AmountFrom.Select(e.NewText.Length, 0); //Sets cursor at the end of textbox
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
        
        private async void ConvertCurrency(string amount, string date)
        {
            ComboBoxItem from = CurrenciesFrom.SelectedItem as ComboBoxItem;
            string fromCurrency = (from.Content as String).Substring(0, 3);

            ComboBoxItem to = CurrenciesTo.SelectedItem as ComboBoxItem;
            string toCurrency = (to.Content as String).Substring(0, 3);

            ExchangeRateDataProvider cdp = new ExchangeRateDataProvider();
            if ((fromCurrency != lastFromCurrency || toCurrency != lastToCurrency) && (fromCurrency != toCurrency) || (date != lastDate))
            {
                lastDate = date;
                exchangeRate = await cdp.GetExchangeRate(fromCurrency, toCurrency, date);
                lastFromCurrency = fromCurrency;
                lastToCurrency = toCurrency;
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
            AmountFrom.Focus(FocusState.Programmatic);
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
            Message.Text = string.Empty;
            if (DatePicker.Date != null)
            {
                var dateTime = (DateTimeOffset)(DatePicker.Date).Value.Date;
                if (dateTime != lastDateTime)
                {
                    if (dateTime < DateTime.Today.AddDays(-365))
                    {
                        dateTime = DateTime.Now.AddDays(-365);
                        DatePicker.Date = dateTime;
                        Message.Text = "Max 1 år bakåt";
                    }
                    else if (dateTime > DateTime.Now)
                    {
                        dateTime = DateTime.Today;
                        DatePicker.Date = dateTime;
                        Message.Text = "Dagens datum";
                    }
                    lastDateTime = dateTime;
                    string date = (dateTime.Year + "-" + dateTime.Month + "-" + dateTime.Day).ToString();
                    DatePicker.PlaceholderText = date;
                    ConvertCurrency(AmountFrom.Text, date);
                }
            }            
        }
    }
}
