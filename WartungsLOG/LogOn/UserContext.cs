using System;
using System.Collections.Generic;
using System.Text;

namespace WartungsLOG.LogOn
{
    public class UserContext
    {
        public string Name { get; internal set; }
        public string UserIdentifier { get; internal set; }
        public bool IsLoggedOn { get; internal set; }
        public string Email { get; internal set; }
        
    }
}
