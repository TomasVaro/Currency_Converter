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
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>();
        DateTimeOffset lastDateTimeFrom = new DateTimeOffset();
        DateTimeOffset lastDateTimeTo = new DateTimeOffset();
        DateTimeOffset dateTimeFrom = new DateTimeOffset();
        DateTimeOffset dateTimeTo = new DateTimeOffset();

        public HistoricalExchangeRates()
        {
            this.InitializeComponent();
            DatePickerFrom.Date = DateTime.Now.AddDays(-8);            
            DatePickerFrom.PlaceholderText = DatePickerFrom.Date.ToString();
            getCurrencies();
        }

        private async void getCurrencies()
        {
            CurrencyDataProvider cdp = new CurrencyDataProvider();
            Dictionary<string, Models.Currency> currencies;
            ComboBoxItem currenciesFrom;
            ComboBoxItem currenciesTo;
            try
            {
                currencies = await cdp.GetCurrencies();
            }
            catch
            {
                return;
            }
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
        }

        private void SwitchButton(object sender, RoutedEventArgs e)
        {
            int currencyFrom = CurrenciesTo.SelectedIndex;
            CurrenciesTo.SelectedIndex = CurrenciesFrom.SelectedIndex;
            CurrenciesFrom.SelectedIndex = currencyFrom;
        }

        private async void ListExchangeRates()
        {
            DateTimeOffset fromDateTime = new DateTimeOffset();
            fromDateTime = (DatePickerFrom.Date).Value.Date;
            string fromDate = string.Empty;

            DateTimeOffset toDateTime = new DateTimeOffset();
            toDateTime = (DatePickerTo.Date).Value.Date;
            string toDate = (toDateTime.Year + "-" + toDateTime.Month + "-" + toDateTime.Day).ToString();

            int nrOfDaysToShow = (int)((dateTimeTo - dateTimeFrom).TotalDays);
            int nrOfApiCalls = (int)Math.Ceiling(((dateTimeTo - dateTimeFrom).TotalDays + 1)/9);
            ExchangeRates.Text = string.Empty;

            try
            {
                ComboBoxItem from = CurrenciesFrom.SelectedItem as ComboBoxItem;
                string fromCurrency = (from.Content as String).Substring(0, 3);
                ComboBoxItem to = CurrenciesTo.SelectedItem as ComboBoxItem;
                string toCurrency = (to.Content as String).Substring(0, 3);
                HistoricalExchangeRateDataProvider cdp = new HistoricalExchangeRateDataProvider();

                for(int i = 0; i < nrOfApiCalls; i++)
                {
                    if (nrOfDaysToShow < 9)
                    {
                        fromDateTime = toDateTime.AddDays(- nrOfDaysToShow);
                    }
                    else
                    {
                        fromDateTime = toDateTime.AddDays(-8);
                        nrOfDaysToShow -= 9;
                    }
                    fromDate = (fromDateTime.Year + "-" + fromDateTime.Month + "-" + fromDateTime.Day).ToString();
                    exchangeRates = await cdp.GetHistoricalExchangeRates(fromCurrency, toCurrency, fromDate, toDate);
                    foreach (KeyValuePair<string, double> keyValuePair in exchangeRates.Reverse())
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
                    toDateTime = toDateTime.AddDays(-9);
                    toDate = toDate = (toDateTime.Year + "-" + toDateTime.Month + "-" + toDateTime.Day).ToString();
                }
            }
            catch
            {
                return;
            }
        }

        private void CalendarDatePicker_DateFromChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (DatePickerTo.Date == null)
            {
                DatePickerTo.Date = DateTime.Now;
                DatePickerTo.PlaceholderText = DatePickerTo.Date.ToString();
            }
            if (DatePickerFrom.Date != null)
            {
                dateTimeFrom = (DateTimeOffset)(DatePickerFrom.Date).Value.Date;
                dateTimeTo = (DateTimeOffset)(DatePickerTo.Date).Value.Date;
                if (dateTimeFrom != lastDateTimeFrom)
                {
                    if (dateTimeFrom > dateTimeTo)
                    {
                        dateTimeFrom = dateTimeTo;
                        MessageFrom.Text = "Datumet justerat";
                    }
                    else if (dateTimeFrom < DateTime.Today.AddDays(-365))
                    {
                        dateTimeFrom = DateTime.Now.AddDays(-365);
                        DatePickerFrom.Date = dateTimeFrom;
                        MessageFrom.Text = "Max 1 år bakåt";
                    }
                    else
                    {
                        MessageFrom.Text = string.Empty;
                    }
                    lastDateTimeFrom = dateTimeFrom;
                    DatePickerFrom.Date = dateTimeFrom;
                }
            }
            else
            {
                DatePickerFrom.Date = lastDateTimeFrom;
            }
        }

        private void CalendarDatePicker_DateToChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {            
            if (DatePickerTo.Date != null)
            {
                dateTimeTo = (DateTimeOffset)(DatePickerTo.Date).Value.Date;
                dateTimeFrom = (DateTimeOffset)(DatePickerFrom.Date).Value.Date;
                if (dateTimeTo != lastDateTimeTo)
                {
                    if (dateTimeTo < dateTimeFrom)
                    {
                        dateTimeTo = dateTimeFrom;
                        MessageTo.Text = "Datumet justerat";
                    }
                    else if (dateTimeTo > DateTime.Now)
                    {
                        dateTimeTo = DateTime.Today;
                        DatePickerTo.Date = dateTimeTo;
                        MessageTo.Text = "Dagens datum";
                    }
                    else
                    {
                        MessageTo.Text = string.Empty;
                    }
                    lastDateTimeTo = dateTimeTo;
                    DatePickerTo.Date = dateTimeTo;
                }
            }
            else
            {
                DatePickerTo.Date = lastDateTimeTo;
            }
        }

        private void Button_OnButtonClick(object sender, RoutedEventArgs e)
        {
            ListExchangeRates();
        }
    }
}
