using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak__Fool_
{
    public class SendCardEventArgs : GameEventArgs
    {
        public Card SendCard { get; set; }
        public int Index { get; set; }
        public RoleOfPlayer Role { get; set; }
    }

    public delegate void SendCardEventHandler(object sender, SendCardEventArgs e);
}
