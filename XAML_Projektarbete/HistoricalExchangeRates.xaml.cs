using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XAML_Projektarbete.DataProvider;

namespace XAML_Projektarbete
{
    public sealed partial class HistoricalExchangeRates : Page
    {
        
        string lastFromCurrency = string.Empty;
        string lastToCurrency = string.Empty;
        string lastDate = string.Empty;
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>();
        DateTimeOffset lastDateTime = new DateTimeOffset();

        public HistoricalExchangeRates()
        {
            this.InitializeComponent();
            DatePicker.PlaceholderText = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day).ToString();
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
            ListExchangeRates();
        }
        
        private async void ListExchangeRates()
        {
            DateTimeOffset fromDateTime = new DateTimeOffset();
            fromDateTime = (DatePicker.Date).Value.Date.AddDays(-8);
            string fromDate = (fromDateTime.Year + "-" + fromDateTime.Month + "-" + fromDateTime.Day).ToString();
            DateTimeOffset endDateTime = new DateTimeOffset();
            endDateTime = (DatePicker.Date).Value.Date;
            string endDate = (endDateTime.Year + "-" + endDateTime.Month + "-" + endDateTime.Day).ToString();

            ComboBoxItem from = CurrenciesFrom.SelectedItem as ComboBoxItem;
            string fromCurrency = (from.Content as String).Substring(0, 3);
            ComboBoxItem to = CurrenciesTo.SelectedItem as ComboBoxItem;
            string toCurrency = (to.Content as String).Substring(0, 3);

            HistoricalExchangeRateDataProvider cdp = new HistoricalExchangeRateDataProvider();
            if ((fromCurrency != lastFromCurrency || toCurrency != lastToCurrency) || (endDate != lastDate))
            {
                lastDate = endDate;
                exchangeRates = await cdp.GetHistoricalExchangeRates(fromCurrency, toCurrency, fromDate, endDate);
                lastFromCurrency = fromCurrency;
                lastToCurrency = toCurrency;
            }
            ExchangeRates.Text = string.Empty;
            foreach (KeyValuePair<string, double> keyValuePair in exchangeRates)
            {
                if (keyValuePair.Value >= 0.0001)
                {
                    ExchangeRates.Text = ExchangeRates.Text + (keyValuePair.Key + "  =  " + string.Format("{0:#,###0.000000}", keyValuePair.Value) + "  " + to.Content.ToString().Substring(0, 3) + "\n");
                }
                else
                {
                    ExchangeRates.Text = ExchangeRates.Text + (keyValuePair.Key + "  =  " + string.Format("{0:#,###0.000000000000}", keyValuePair.Value) + "  " + to.Content.ToString().Substring(0, 3) + "\n");
                }
            }
        }

        private void ChangeCurrency_OnDropDownClosed(object sender, object e)
        {
            ListExchangeRates();
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            Message.Text = string.Empty;
            if (DatePicker.Date != null && CurrenciesFrom.SelectedItem != null)
            {
                var dateTime = (DateTimeOffset)(DatePicker.Date).Value.Date;
                if (dateTime != lastDateTime)
                {
                    if (dateTime < DateTime.Today.AddDays(-357))
                    {
                        dateTime = DateTime.Now.AddDays(-357);
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
                    ListExchangeRates();
                }
            }
        }
    }
}
