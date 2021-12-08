using System;
using System.Collections.Generic;
using System.Text;

namespace WartungsLOG.LogOn
{
    public interface IParentWindowLocatorService
    {
        object GetCurrentParentWindow();
    }
}
