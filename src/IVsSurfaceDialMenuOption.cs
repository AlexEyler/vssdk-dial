using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialExtension
{
    public interface IVsSurfaceDialMenuOption
    {
        void OnActivated();
        void OnDeactivated();
        void OnDialRotated(double rotationInDegrees);
        void OnDialClicked();
        string OptionText { get; }
    }
}
