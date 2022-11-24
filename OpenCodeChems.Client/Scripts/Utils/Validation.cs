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
            isValid = isValid & (password.Length >= 8);

            string especialCharacters = "*#+-_;.@%&/()=!?¿¡{}[]^<>";

            bool probeEspecial = false, probeNumber = false, probeUpperCase = false, probeLowerCase = false;
            foreach (char character in password)
            {
                foreach (char especialCharacter in especialCharacters)
                {
                    if (character == especialCharacter)
                    {
                        probeEspecial = true;
                    }
                        
                }

                if (probeEspecial)
                {
                    break;
                }
            }
            
            foreach(char character in password)
            {
                probeNumber |= (character >= '0' && character <= '9');
                probeUpperCase |= (character >= 'A' && character <= 'Z');
                probeLowerCase |= (character >= 'a' && character <= 'z');
                
            }
            
            return isValid & probeNumber & probeEspecial & probeLowerCase & probeUpperCase;
        }

        public bool ValidateEmail(string email)
        {
            bool isValid = true;
            isValid = isValid & (email.Length <= 255);
            isValid = isValid & EmailValidator.Validate(email);
            return isValid;
        }

        
    }
}
