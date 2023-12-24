using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Durak__Fool_
{
    public class ChooseEventArgs : EventArgs
    {
        public int DX { get; set; }
        public Point Pointer { get; set; }
        public Card CardInHand { get; set; }
    }

    public delegate void ChooseEventHandler(object sender, ChooseEventArgs eArgs);
    public delegate bool CheckCardIsOpenedEventHandler(object sender, ChooseEventArgs eArgs);
}
