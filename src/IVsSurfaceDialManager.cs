using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DialExtension
{
    public interface IVsSurfaceDialManager
    {
        void AddMenuOption(IVsSurfaceDialMenuOption menuOption);
    }
}
