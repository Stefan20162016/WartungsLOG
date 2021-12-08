using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using WartungsLOG.LogOn;

namespace WartungsLOG.iOS
{
    public class DummyParentWindowLocator : IParentWindowLocatorService
    {
        public object GetCurrentParentWindow()
        {
            return null;
        }
    }
}