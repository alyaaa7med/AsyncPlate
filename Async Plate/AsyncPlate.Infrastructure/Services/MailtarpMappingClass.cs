using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Services
{
    //it is for the option pattern in the email service
    //this class is used to fill the settings from appsettings.json
    //without it ,i will access the settings from the configuration is not a good practice
    //and i am now expose to errors like typos and missing settings
    public class MailtarpMappingClass
    {
        public string ApiToken { get; set; } = string.Empty;
        public int InboxId { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
    
}
}
