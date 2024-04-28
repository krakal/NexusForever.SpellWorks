using System.Globalization;
using System.Windows.Data;

namespace NexusForever.SpellWorks
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return "";

            string name = Enum.GetName(value.GetType(), value);
            if (name == null)
                return $"{(int)value}";
            else
                return $"{(int)value} - {name}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
