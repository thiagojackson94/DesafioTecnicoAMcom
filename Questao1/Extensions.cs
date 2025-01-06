using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1
{
    public static partial class Extensions
    {
        public static Char ToLower(this Char c, CultureInfo culture)
        {
            return Char.ToLower(c, culture);
        }
    }
}
