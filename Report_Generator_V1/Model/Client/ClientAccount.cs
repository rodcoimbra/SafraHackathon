using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Generator_V1.Model.Client
{
    public class ClientAccount
    {
        public string TokenAccount { get; set; }

        public ClientAccount(string tokenCode)
        {
            this.TokenAccount = tokenCode;
        }

    }

    
    
}
