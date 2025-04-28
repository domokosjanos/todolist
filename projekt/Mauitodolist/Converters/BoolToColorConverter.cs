using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiToDoList.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool allapot && parameter is string parameterValue)
            {
                if (parameterValue.ToLower() == "kesz" && allapot)
                {
                    return Colors.Green;
                }
                if (parameterValue.ToLower() == "nemkesz" && !allapot)
                {
                    return Colors.Red;
                }
            }
            // Alapértelmezett szín, ha egyik feltétel sem teljesül
            return Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}