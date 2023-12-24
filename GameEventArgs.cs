using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak__Fool_
{
    public class GameEventArgs : EventArgs
    {
        public int PlayerNum { get; set; }
        public int NumCardInHand { get; set; }
        public Suit Trump { get; set; }
        public bool IsCompleted { get; set; }
    }

    public delegate void GameEventHandler(object sender, GameEventArgs e);
}
