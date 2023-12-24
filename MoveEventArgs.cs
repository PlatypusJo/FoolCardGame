using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak__Fool_
{
    public class MoveEventArgs : GameEventArgs
    {
        public int OponentNum { get; set; }
        public int OponentQuantity { get; set; }
        public int MainPlayerNum { get; set; }
        public DecisionOfPlayer Decision { get; set; }
    }

    public delegate void MoveEventHandler(object sender, MoveEventArgs e);
}
