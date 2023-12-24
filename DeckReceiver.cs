using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak__Fool_
{
    [Serializable]

    abstract class DeckReceiver : Deck
    {
        protected int playerNum;
        protected int dX;

        public abstract void TakeCard(object sender, SendCardEventArgs eArgs);
        protected void AlignTheCards() // выравнивает карты
        {
            for (int i = 0, j = 0, countEmpty = 0; i < 36 && j < countOfCards; i++)
            {
                if (cards[i] is null)
                {
                    countEmpty++;
                }
                else
                {
                    if (countEmpty > 0)
                    {
                        cards[i - countEmpty] = cards[i];
                        cards[i] = null;
                    }
                    j++;
                }
            }
        }
        protected void SetCoordinates() // устанавливает координаты карт
        {
            if (countOfCards >= 10)
                dX = 30;
            else
                dX = 50;
            int beg, end = 0;
            if ((beg = point.X - dX * (countOfCards / 2)) < 0 || (end = point.X + dX * (countOfCards / 2)) > 1450)
            {
                int left, right;
                while (beg < 0 || end > 1450)
                {
                    if ((left = beg) < 0)
                    {
                        beg = (beg - left) + 40;//лучше прибавлять 20
                    }
                    if ((right = end) > 1450)
                    {
                        end -= (right - 1450);
                        beg = end - dX * countOfCards;
                    }
                }
            }
            for (int i = 0; i < countOfCards; i++)
            {
                //cards[i].X = point.X + i * dX;
                //cards[i].Y = point.Y;
                cards[i].SetNextLocation(beg + i * dX, point.Y);
            }
        }
    }
}
