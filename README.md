# XAML_Projektarbete
I have created an UWP-project for currency conversion for current and historical currency rates. It's also possible to se what currency a country uses and which countries uses a specific currency.
I have used a free API from https://www.currencyconverterapi.com/ which has som limitations e.g
* Number of Requests per Hour: 100
* Date Range in History: 8 Days
* Allowed Back in History: 1 Year

I made some changes to the Template for ComboBox, CalendarDatePicker and Button.
I also made a binding to AmountToText so the exchangeRate updates automatically every minute wich will show in the AmountTo Textbox-Text.
