using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using World;
using System.Windows;
using Common;

namespace GooseUnrunnableRuns
{
    class OffsetToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int offsetX = (int)value;
                int offsetY = (int)parameter;
                return new Thickness(offsetX, offsetY, 0, 0);
            }
            catch { }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
