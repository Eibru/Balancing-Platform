using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using BalancingPlatform.GUI.Utility;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.GUI.Converters;
public class BitmapBitmapValueConverter : IValueConverter{
    public static readonly BitmapBitmapValueConverter Instance = new BitmapBitmapValueConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if(value == null) return null;



        var bitmap = (System.Drawing.Bitmap)value;
        using (MemoryStream ms = new MemoryStream()) {
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            return new Avalonia.Media.Imaging.Bitmap(ms);
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return new BindingNotification(new NotSupportedException(), BindingErrorType.Error);
    }
}
