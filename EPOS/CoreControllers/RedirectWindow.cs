using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPOS.CoreControllers
{
    public abstract class RedirectPage :  System.Windows.Controls.Page
    {
        public abstract string PageTitle();
    }
}
