using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak__Fool_
{
    public class MenuEventArgs : EventArgs
    {
        public int TypeOfPlayer { get; set; }
        public int PlayersNumber { get; set; }
        public int IndexOfPlayer { get; set; }
    }

    public delegate void MenuEventHandler(object sender, MenuEventArgs e); 
}
