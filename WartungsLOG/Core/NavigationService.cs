using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WartungsLOG.Core
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string location)
        {
            await Shell.Current.GoToAsync(location);
        }

        public async Task GoToAsync(string location, bool animate)
        {
            await Shell.Current.GoToAsync(location, animate);
        }
    }
}
