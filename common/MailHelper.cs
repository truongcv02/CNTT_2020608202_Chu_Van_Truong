using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
namespace common
{
	public class MailHelper
	{
		public void SendMail(string subject, string content)
		{
			var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
		}	
	}
}
