using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WartungsLOG.LogOn;

namespace WartungsLOG.UWP
{
    public class DummyParentWindowLocator : IParentWindowLocatorService
    {
        public object GetCurrentParentWindow()
        {
            return null;
        }
    }
}
