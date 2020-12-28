using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthDiary.Common
{
    public static class StringExtensions
    {
        public static string ToThumbnailPath(this string path)
        {
            if (path is null)
            {
                return "";
            }
            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            return Path.Combine(dir, $"{fileName}-thumbnail{extension}");
        }

        public static string GenerateRandomFileName(this string ext)
        {
            var sb = new StringBuilder();
            var parts = Guid.NewGuid().ToString().Split('-');
            foreach (var part in parts)
            {
               sb.Append( Convert.ToInt64(part,16).ConvertToBase(62));
            }
            sb.Append(DateTime.UtcNow.Ticks.ConvertToBase(62));
            return sb.ToString() + ext;
        }

        private static string ConvertToBase(this long decimalNumber, int radix)
        {
            const int BitsInLong = 64;
            const string Digits = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber /= radix;
            }

            string result = new string(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}
