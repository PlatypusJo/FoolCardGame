using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Durak__Fool_
{
    [Serializable]

    abstract class Deck
    {
        protected int countOfCards;
        protected Point point;
        protected Card[] cards;

        abstract public void GiveCard(object sender, SendCardEventArgs e);
        abstract public void Draw(Graphics graphics, bool vis);
        public int NumberOfCards
        {
            get { return countOfCards; }
        }
    }
}
