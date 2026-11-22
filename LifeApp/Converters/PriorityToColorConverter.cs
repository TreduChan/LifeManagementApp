using System.Globalization;

namespace LifeManagement.Converters
{
    // A converter turns a bound value into something else the UI needs.
    // Here: the Priority number (int) becomes a text colour, so important todos stand out.
    // The ViewModel and the model stay free of colours; this is purely a display decision.
    public class PriorityToColorConverter : IValueConverter
    {
        // Model → UI: called for every bound Priority
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int priority)
                return Colors.Gray;

            if (priority >= 4)
                return Colors.Red;      // 4–5: important
            if (priority == 3)
                return Colors.Orange;   // 3: normal
            return Colors.Gray;         // 1–2: low
        }

        // UI → model: not needed, the colour is only shown (one-way binding)
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
