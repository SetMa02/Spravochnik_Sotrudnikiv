using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace LABA1_MM.ValidationRules
{
    public class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = string.Empty;
            if(value != null)
            {
                email = value.ToString();
            }
            else
            {
                return new ValidationResult(false, " Адрес электронной почты не задан! ");
            }
            if(email.Contains("@") && email.Contains("."))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Адрес электронной почты должен содержать символы @ и (.) точки /n" + 
                    "Шаблон адреса example@mail.ru");
            }
        }
    }
}
