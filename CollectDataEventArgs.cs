using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak__Fool_
{
    public class CollectDataEventArgs : EventArgs
    {
        public Card[,] field;
        public int Line1 { get; set; }
        public int Line2 { get; set; }
        public int RankTrump { get; set; }
        public int DealerNumberOfCards { get; set; }
        public CollectDataEventArgs() { field = new Card[2, 6]; }
    }

    public delegate void CollectDataEventHandler(object sender, CollectDataEventArgs e);
}
