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
        DateTimeOffset lastEndDateTime = new DateTimeOffset();
        DateTimeOffset fromDateTime = new DateTimeOffset();
        DateTimeOffset endDateTime = new DateTimeOffset();

        public HistoricalExchangeRates()
        {
            this.InitializeComponent();
            //DatePicker.PlaceholderText = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day).ToString();
            DatePicker.Date = DateTime.Now;
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
            ListExchangeRates();
        }

        private void SwitchButton(object sender, RoutedEventArgs e)
        {
            int currencyFrom = CurrenciesTo.SelectedIndex;
            CurrenciesTo.SelectedIndex = CurrenciesFrom.SelectedIndex;
            CurrenciesFrom.SelectedIndex = currencyFrom;
        }

        string lastFromCurrency = string.Empty;
        string lastToCurrency = string.Empty;
        string lastDate = string.Empty;
        Dictionary<string, Object> exchangeRates = new Dictionary<string, Object>();
        private async void ListExchangeRates()
        {
            fromDateTime = (DatePicker.Date).Value.Date.AddDays(-8);
            string fromDate = (fromDateTime.Year + "-" + fromDateTime.Month + "-" + fromDateTime.Day).ToString();

            endDateTime = (DatePicker.Date).Value.Date;
            string endDate = (endDateTime.Year + "-" + endDateTime.Month + "-" + endDateTime.Day).ToString();

            DatePicker.PlaceholderText = endDate;

            ComboBoxItem from = CurrenciesFrom.SelectedItem as ComboBoxItem;
            string fromCurrency = (from.Content as String).Substring(0, 3);

            ComboBoxItem to = CurrenciesTo.SelectedItem as ComboBoxItem;
            string toCurrency = (to.Content as String).Substring(0, 3);

            ExchangeRateDataProvider cdp = new ExchangeRateDataProvider();
            if ((fromCurrency != lastFromCurrency || toCurrency != lastToCurrency) && (fromCurrency != toCurrency) || (endDate != lastDate))
            {
                lastDate = endDate;
                exchangeRates = await cdp.GetHistoricalExchangeRates(fromCurrency, toCurrency, fromDate, endDate);
                lastFromCurrency = fromCurrency;
                lastToCurrency = toCurrency;
            }

            ExchangeRates.Text = "Hej";   // Skriver resultaten i Textboxen ExchangeRates
            DatePicker.Focus(FocusState.Programmatic);  //Sets focus on DatePicker ??????????????????????????????
        }

        private void ChangeCurrency_OnDropDownClosed(object sender, object e)
        {
            ListExchangeRates();
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (DatePicker.Date != null)
            {
                if (endDateTime != lastEndDateTime)
                {
                    if (endDateTime < DateTime.Today.AddDays(-357))
                    {
                        endDateTime = DateTime.Now.AddDays(-357);
                        DatePicker.Date = endDateTime;
                    }
                    else if (endDateTime > DateTime.Now)
                    {
                        endDateTime = DateTime.Today;
                        DatePicker.Date = endDateTime;
                    }
                    lastEndDateTime = endDateTime;
                    ListExchangeRates();
                }
            }
        }
    }
}
