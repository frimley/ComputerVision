using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSight
{
    interface IViewMain
    {
        bool Running { get; set; }
        double FrameRate { get; set; }
        int FrameSendRate { get; set; }
        string Resolution { get; set; }
    }
}