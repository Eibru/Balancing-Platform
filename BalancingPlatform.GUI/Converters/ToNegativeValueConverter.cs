using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.GUI.Converters;
public class ToNegativeValueConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value == null) return null;
        int val = -System.Convert.ToInt32((uint)value);

        return val;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value == null) return null;
        int val = -System.Convert.ToInt32((uint)value);

        return val;

    }
}
