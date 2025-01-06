using System.Globalization;

namespace Questao2
{
    public static partial class Extensions
    {
        public static Char ToLower(this Char c, CultureInfo culture)
        {
            return Char.ToLower(c, culture);
        }
    }
}
