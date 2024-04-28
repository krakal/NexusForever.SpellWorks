using System.Globalization;
using System.Windows.Data;

namespace NexusForever.SpellWorks
{
    public class EnumFlagToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return "";

            return $"0x{(int)value:x4} - {Enum.GetName(value.GetType(), value)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
