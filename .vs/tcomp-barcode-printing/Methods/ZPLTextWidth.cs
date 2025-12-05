using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcomp_barcode_printing.Methods
{
    public class ZPLTextWidth
    {
        public int CalculateZplTextWidth(string text, int fontWidth,int labelWidth, int rightMargin = 0)
        {
            if (string.IsNullOrEmpty(text)) return 0;

            double narrow = 0.40;   // i, l, punctuation
            double digit = 0.55;   // 0-9
            double normal = 0.62;   // average letter
            double wide = 0.85;   // M, W etc
            double space = 0.45;
            double slash = 0.45;

            double total = 0.0;
            foreach (char ch in text)
            {
                if (ch == ' ') total += space;
                else if (ch == '/' || ch == 'x' || ch == 'X' || ch == '*') total += slash;
                else if (char.IsDigit(ch)) total += digit;
                else if ("il.,:'\"|!()".IndexOf(ch) >= 0) total += narrow;
                else if ("MW@&%#".IndexOf(ch) >= 0) total += wide;
                else total += normal;
            }

            int raw = (int)Math.Round(total * fontWidth);
            int maxWidth = Math.Max(0, labelWidth - rightMargin);
            return Math.Min(raw, maxWidth);
        }
    }
}
