using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailValidation;


namespace OpenCodeChems.Client.Resources
{
    public class Validation
    {
        public bool ValidatePassword(string password)
        {
            bool isValid = true;
            isValid = isValid & !String.IsNullOrWhiteSpace(password);
            isValid = isValid & (password.Length >= 8);
            return isValid;
        }

        public bool ValidateEmail(string email)
        {
            bool isValid = true;
            isValid = isValid & !String.IsNullOrWhiteSpace(email);
            isValid = isValid & (email.Length <= 255);
            isValid = isValid & EmailValidator.Validate(email);
            return isValid;
        }

        
    }
}
