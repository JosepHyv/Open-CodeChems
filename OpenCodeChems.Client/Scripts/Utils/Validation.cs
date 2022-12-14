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
			isValid = isValid && !String.IsNullOrWhiteSpace(password);
			isValid = isValid && (password.Length >= 8);

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
			
			return isValid && probeNumber && probeEspecial && probeLowerCase && probeUpperCase;
		}

		public bool ValidateEmail(string email)
		{
			bool isValid = true;
			isValid = !String.IsNullOrWhiteSpace(email);
			isValid = isValid && (email.Length <= 255);
			isValid = isValid && EmailValidator.Validate(email);
			return isValid;
		}
		
		public bool ValidateIp(string address)
		{
			bool status = true;
			var ipParts = address.Split(".");

			if (ipParts.Length != 4)
			{
				status =  false;
			}

			foreach (var part in ipParts)
			{
				if (part.StartsWith("0") && part.Length > 1)
				{
					status =  false;
				}

				if (part.Length != part.Trim().Length)
				{
					status =  false;
				}

				int number;
				var result = Int32.TryParse(part, out number);
				if (!result || number > 255 || number < 0)
				{
					status =  false;
				}    
			}

			if(!status)
			{
				status = address.ToLower().Equals("localhost");
			}
		
			return status;
		}
		
		public bool ValidatePort(string port)
		{
			if(String.IsNullOrWhiteSpace(port))
			{
				return false;
			}
			foreach(char element in port)
			{
				if(element < '0' || element > '9')
					return false;
			}
			int currentPort = Int32.Parse(port);
			return currentPort >= 1 && currentPort <= 65535;
		}
		public bool ValidateUsernameAndNickname(string username)
		{
			bool isValid = true;
			isValid = username.Length <= 40;
			return isValid;
		}
		public bool ValidateName(string name)
		{
			bool isValid = true;
			isValid = name.Length <= 50;
			string especialCharacters = "*#+-_;.@%&/()=!?¿¡{}[]^<>";
			foreach(char character in name)
			{
  				if(character >= '0' && character <= '9')
				{ 
					isValid = false;
				}
			}
			foreach (char character in name)
			{
				foreach (char especialCharacter in especialCharacters)
				{
					if (character == especialCharacter)
					{
						isValid = true;
					}
						
				}

				if (isValid)
				{
					break;
				}
			}
			return isValid;
		}
		public bool ValidateCodeRegistration(string code)
		{
			bool isValid = true;
			int codeResult = 0;
			isValid = int.TryParse(code, out codeResult);
			return isValid;
		}
	}
}
