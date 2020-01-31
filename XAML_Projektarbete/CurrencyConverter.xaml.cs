using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XAML_Projektarbete.DataProvider;

namespace XAML_Projektarbete
{
    public sealed partial class CurrencyConverter : Page, INotifyPropertyChanged
    {
        private string lastFromCurrency = string.Empty;
        private string lastToCurrency = string.Empty;
        private string lastDate = string.Empty;
        private double exchangeRate;
        private string amountToText;
        private string fromCurrency;
        private string toCurrency;
        private string date;
        private DispatcherTimer timer = new DispatcherTimer();
        private DateTimeOffset lastDateTime = new DateTimeOffset();
        public event PropertyChangedEventHandler PropertyChanged;
        ExchangeRateDataProvider erdp = new ExchangeRateDataProvider();
        
        public string AmountToText 
        { 
            get { return amountToText; }
            set
            {
                if (amountToText != value)
                {
                    amountToText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AmountToText)));
                }
            }
        }

        public CurrencyConverter()
        {
            this.InitializeComponent();
            DatePicker.PlaceholderText = (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day).ToString();
            getCurrencies();
            date = DatePicker.PlaceholderText;
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            Unloaded += OnPageLeave;
        }

        private async void Timer_Tick(object sender, object e)
        {
            if(fromCurrency != null || toCurrency != null)
            {
                exchangeRate = await erdp.GetExchangeRate(fromCurrency, toCurrency, date);
                ConvertCurrency(AmountFrom.Text, date);
            }
        }

        private void OnPageLeave(object sender, RoutedEventArgs e)
        {
            timer.Stop();
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
        
        private async void ConvertCurrency(string amount, string date)
        {
            ComboBoxItem from = CurrenciesFrom.SelectedItem as ComboBoxItem;
            try
            {
                fromCurrency = (from.Content as String).Substring(0, 3);
            }
            catch
            {
                return;
            }

            ComboBoxItem to = CurrenciesTo.SelectedItem as ComboBoxItem;
            toCurrency = (to.Content as String).Substring(0, 3);

            if ((fromCurrency != lastFromCurrency || toCurrency != lastToCurrency) && (fromCurrency != toCurrency) || (date != lastDate))
            {
                lastDate = date;
                exchangeRate = await erdp.GetExchangeRate(fromCurrency, toCurrency, date);
                lastFromCurrency = fromCurrency;
                lastToCurrency = toCurrency;
            }

            if (fromCurrency == toCurrency)
            {
                AmountToText = amount;
            }
            // Checks if AmountFrom textbox is empty or not
            else if (double.TryParse(amount, out double result))
            {
                if (result * exchangeRate > 0.01)
                {
                    AmountToText = string.Format("{0:#,###0.00}", result * exchangeRate);
                }
                else
                {
                    decimal resultInDecimal = (decimal)(result * exchangeRate);
                    AmountToText = resultInDecimal.ToString();
                }
            }
            else
            {
                AmountToText = "";
            }
            AmountFrom.Focus(FocusState.Programmatic);
        }

        private void SwitchButton(object sender, RoutedEventArgs e)
        {
            int currencyFrom = CurrenciesTo.SelectedIndex;
            CurrenciesTo.SelectedIndex = CurrenciesFrom.SelectedIndex;
            CurrenciesFrom.SelectedIndex = currencyFrom;
            ConvertCurrency(AmountFrom.Text, DatePicker.PlaceholderText);
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
