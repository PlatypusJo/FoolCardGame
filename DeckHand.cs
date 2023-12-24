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

    class DeckHand : DeckReceiver
    {
        public static event SendCardEventHandler GiveCardEvent;

        internal enum Key
        {
            Rank,
            Suit
        }

        public DeckHand(int xcoord, int ycoord, int playerNum) 
        {
            dX = 50; //30 ставим если много карт (скорее всего когда доходим до 10)
            this.playerNum = playerNum; 
            cards = new Card[36];
            point = new Point(xcoord, ycoord);
            countOfCards = 0;
            DeckTable.DealCardEvent += TakeCard;
            GameField.ReturnCardsEvent += TakeCard;
        }
        public override void TakeCard(object sender, SendCardEventArgs e) //принимает карту
        {
            if (e.PlayerNum == playerNum)
            {
                for (int i = 0; i < 36; i++)
                {
                    if (cards[i] is null)
                    {
                        cards[i] = e.SendCard;
                        countOfCards++;
                        if (sender is Human)
                        {
                            Human.ChooseCard += cards[i].CheckPos;
                            AlignTheCards();
                        }
                        break;
                    }
                } 
                if (e.NumCardInHand == 0)
                {
                    SortCardsInHand(e.Trump);
                    SetCoordinates();
                }  
            }
              
        }
        public override void GiveCard(object sender, SendCardEventArgs e) //выдаёт карту
        {
            GiveCardEvent?.Invoke(sender, e);
            if (e.IsCompleted)
            {
                if (sender is Human)
                {
                    Human.ChooseCard -= cards[e.Index].CheckPos;
                }
                cards[e.Index] = null;
                e.SendCard = null;
                e.Index = 0;
                countOfCards--;
                if (sender is BotPlayer)
                {
                    AlignTheCards();
                    SetCoordinates();
                }
            }
        }
        public override void Draw(Graphics graphics, bool vis)
        {
            for (int i = 0, j = 0; i < 36 && j < countOfCards; i++)
            {
                if (!(cards[i] is null))
                {
                    cards[i].Draw(graphics, vis);
                    j++;
                }
            }
        } 
        public bool CheckCardIsOpened(object sender, ChooseEventArgs e) //проверка открытости карты
        {  
            int index = Array.FindIndex(cards, 0, 36, card => card is null ? false : (card.Suit == e.CardInHand.Suit && card.Rank == e.CardInHand.Rank));
            if (dX != 50 && index < 35 && cards[index + 1] is null) 
                e.DX = dX * 2; 
            else 
                e.DX = dX;
            return dX == 50 ? (index == 35 || cards[index + 1] is null) : (index == 35 || index == 34 && cards[35] is null || index < 34 && cards[index + 1] is null && cards[index + 2] is null);
            #region variants
            //1.
            //if (dX != 50) eArgs.DX *= 20 else eArgs.DX = dX;
            //return dX == 50 ? indexOfCard == 35 || cards[indexOfCard + 1] is null : (indexOfCard == 35 || indexOfCard == 34 && cards[35] is null) || (cards[indexOfCard + 1] is null && cards[indexOfCard + 2 > 35 ? 35 : indexOfCard + 2] is null);
            //=================================================================================================================
            //2.
            //bool res = dX == 50 ? indexOfCard == 35 || cards[indexOfCard + 1] is null : indexOfCard == 35 || (cards[indexOfCard + 1] is null && cards[indexOfCard + 2] is null);
            //if (!res) eArgs.DX += 20;
            //return res;
            #endregion
        }
        private void SortCardsInHand(Suit trump_suit) // функция сортировки карт в руке игрока
        {
            QSort(0, countOfCards - 1, Key.Suit, trump_suit);
            for (int i = 0, beg = 0, end; i != countOfCards;)
            {
                if (i + 1 == 36 || cards[i + 1] is null || cards[i].Suit != cards[i + 1]?.Suit)
                {
                    end = i;
                    QSort(beg, end, Key.Rank, trump_suit);
                    beg = ++i;
                }
                else i++;
            }
            //SetCoordinates();
        }
        private void QSort(int left, int right, Key key, Suit trump_suit) //быстрая сортировка
        {
            int i = left, j = right;
            Card tCard = cards[(left + right) / 2];
            do
            {
                if (key == Key.Suit)
                {
                    while (ConvertSuitToInt(cards[i].Suit, trump_suit) > ConvertSuitToInt(tCard.Suit, trump_suit)) i++;
                    while (ConvertSuitToInt(tCard.Suit, trump_suit) > ConvertSuitToInt(cards[j].Suit, trump_suit)) j--;
                }
                else
                {
                    if (key == Key.Rank)
                    {
                        while (cards[i].Rank > tCard.Rank) i++;
                        while (tCard.Rank > cards[j].Rank) j--;
                    }
                }
                if (i <= j)
                {
                    Card buf = cards[i];
                    cards[i] = cards[j];
                    cards[j] = buf;
                    i++;
                    j--;
                }
            } while (i <= j);
            if (left < j) QSort(left, j, key, trump_suit);
            if (i < right) QSort(i, right, key, trump_suit);
        }
        private int ConvertSuitToInt(Suit suit_of_card, Suit trump_suit)
        {
            return suit_of_card == trump_suit ? 4 : (int)suit_of_card;
        } 
        public Card this[int index]
        {
            get
            {
                return cards[index % 36];
            }
        }
    }
    #region useless
    //public int GetNumOfCardsInHand()
    //{
    //    int numberofcards = 0;
    //    for (int i = 0; i < 36; i++)
    //        if (this.cards[i] != null)
    //            numberofcards++;
    //        else
    //        {
    //            //LastCard = i;
    //            break;
    //        }
    //    return numberofcards;
    //}
    #endregion
}
