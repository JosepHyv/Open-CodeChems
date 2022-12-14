using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OpenCodeChems.Server.Utils
{
	/// <summary>
	/// Validations of ip and host
	/// </summary>
	public class Validation
	{
		/// <summary>
		/// validate that ip is a correct format
		/// </summary>
		/// <param name="address">ip address provided</param>
		/// <returns></returns>
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
		/// <summary>
		/// Validate that port is correct format
		/// </summary>
		/// <param name="port">port provided</param>
		/// <returns></returns>
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
	}
}
