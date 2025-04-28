using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiToDoList.Converters
{
    // Ellenőrzi, hogy a feladat létrehozója-e a bejelentkezett felhasználó
    public class IsCreatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int FHO_id && parameter is int creatorId)
            {
                return FHO_id == creatorId;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}