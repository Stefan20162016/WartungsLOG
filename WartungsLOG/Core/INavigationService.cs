using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WartungsLOG.Core
{
    public interface INavigationService
    {
        Task GoToAsync(string location);
        Task GoToAsync(string location, bool animate);
    }
}
