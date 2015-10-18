using CFAStudyPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace CFAStudyPlanner.Resources
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value.GetType();

            if (valueType == typeof(ObservableCollection<Reading>))
            {
                var readings = ((IEnumerable<Reading>)value).ToList();
                var completed = readings.FindAll(x => x.Completed).Count;
                if (completed == 0) return new SolidColorBrush(Colors.Gray);
                return (completed == readings.Count) ?
                    new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.Coral);
            }
            else
            {
                return (bool)value ? new SolidColorBrush(Colors.MediumSeaGreen) : new SolidColorBrush(Colors.Salmon);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
