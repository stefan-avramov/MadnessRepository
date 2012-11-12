using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using World;
using Common;
using System;

namespace GooseUnrunnableRuns
{
    class PointToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Point point = (Point)value;
                return new Thickness(point.X, point.Y, 0, 0);
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
