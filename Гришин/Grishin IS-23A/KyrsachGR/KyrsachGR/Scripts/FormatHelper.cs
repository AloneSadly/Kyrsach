using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyrsachGR.Scripts
{
    public static class FormatHelper
    {
        public static string ShortenName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "Неизвестно";

            string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0]; 
            if (parts.Length == 2) return $"{parts[0]} {parts[1][0]}."; 

            return $"{parts[0]} {parts[1][0]}.{parts[2][0]}.";
        }

        public static string MaskPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "Номер не указан";

            // Оставляем только цифры
            string digits = new string(phone.Where(char.IsDigit).ToArray());

            if (digits.Length == 11) 
            {
                return $"+{digits[0]}({digits[1]}**)-***-**-{digits.Substring(9, 2)}";
            }

            return phone; 
        }
    }
}
