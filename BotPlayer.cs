using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;

namespace Durak__Fool_
{
    [Serializable]
    public struct CardV
    {
        public int rank;
        public Suit suit;
        public CardV(int rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }
    };

    [Serializable]
    public struct Oponent
    {
        public int number;
        public int cardsQuantity;
        public CardV[] cards;
        public Oponent(int index)
        {
           number = index;
           cardsQuantity = 0;
           cards = new CardV[36];
            for (int i = 0; i < 36; i++)
            {
                cards[i] = new CardV(-1, Suit.NotExist);
            }
            
        }
    };

    //[Serializable]
    //public struct Row
    //{
    //    public int rank;
    //    public int count;
    //    public bool hasTrump;
    //    public Row(int rank, int count)
    //    {
    //        this.rank = rank;
    //        this.count = count;
    //        hasTrump = false;
    //    }
    //};

    [Serializable]

    class BotPlayer : Player
    {
        public static event CollectDataEventHandler CollectDataFieldEvent;

        public BotPlayer(int index, int count) : base(index, count)
        {
            show = false;
        }

        public override void Decide(DecisionOfPlayer decision) 
        { 
            this.decision = DecisionOfPlayer.Nothing;
        }

        protected void CollectDataField(object sender, CollectDataEventArgs e)
        {
            CollectDataFieldEvent?.Invoke(sender, e);
        }

        //protected void FindRows()
        //{
        //    bool isExist = false;
        //    for (int i = 0; i < hand.NumberOfCards; i++)
        //    {
        //        if (!(hand[i] is null) && !(isExist = rows.Exists(x => x.rank == hand[i].Rank)))
        //        {
        //            for (int j = i + 1; j < hand.NumberOfCards; j++)
        //            {
        //                if (!(hand[j] is null) && hand[i].Rank == hand[j].Rank)
        //                {
        //                    if (!isExist)
        //                    {
        //                        rows.Add(new Row(hand[i].Rank, 2));
        //                        isExist = true;
        //                    }
        //                    else
        //                    {
        //                        int number = rows.FindIndex(x => x.rank == hand[i].Rank);
        //                        Row buf = rows[number];
        //                        buf.count++;
        //                        rows[number] = buf;
        //                        if (rows[number].count == 4)
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void FindRows(List<int> nums)
        //{
        //    bool isExist = false;
        //    for (int i = 0; i < nums.Count; i++)
        //    {
        //        if (!(hand[nums[i]] is null) && !(isExist = rows.Exists(x => x.rank == hand[nums[i]].Rank)))
        //        {
        //            for (int j = i + 1; j < nums.Count; j++)
        //            {
        //                if (!(hand[nums[j]] is null) && hand[nums[i]].Rank == hand[nums[j]].Rank)
        //                {
        //                    if (!isExist)
        //                    {
        //                        rows.Add(new Row(hand[nums[i]].Rank, 2));
        //                        isExist = true;
        //                    }
        //                    else
        //                    {
        //                        int number = rows.FindIndex(x => x.rank == hand[nums[i]].Rank);
        //                        Row buf = rows[number];
        //                        buf.count++;
        //                        rows[number] = buf;
        //                        if (rows[number].count == 4)
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void SelectCards(List<int> nums, int line1, int line2, Card[,] fields)
        //{
        //    for (int i = 0, j = 0; j < line1 || j < line2; i++)
        //    {
        //        if (i == hand.NumberOfCards)
        //        {
        //            j++;
        //            i = -1;
        //            continue;
        //        }
        //        if (!(fields[0, j] is null) && j < line1)
        //        {
        //            if (!(hand[i] is null) && hand[i].Rank == fields[0, j].Rank && !nums.Exists(x => x == i))
        //            {
        //                nums.Add(i);
        //                i = -1;
        //                j++;
        //                continue;
        //            }
        //        }
        //        if (!(fields[1, j] is null) && j < line2)
        //        {
        //            if (!(hand[i] is null) && hand[i].Rank == fields[1, j].Rank && !nums.Exists(x => x == i))
        //            {
        //                nums.Add(i);
        //                i = -1;
        //                j++;
        //                continue;
        //            }
        //        }
        //    }
        //}

        public override void Draw(Graphics g)
        {
            #region Omnomnom
            //if (role == RoleOfPlayer.Attacker)
            //{
            //    if (decision == DecisionOfPlayer.Nothing)
            //    {
            //        g.DrawString("Я хожу!", new Font("TimesNewRoman", 16), new SolidBrush(Color.Black), new Point(location.X, location.Y - 25));
            //    }
            //    if (decision == DecisionOfPlayer.LetTakeCards)
            //    {
            //        g.DrawString("Пусть берёт!", new Font("TimesNewRoman", 16), new SolidBrush(Color.Black), new Point(location.X, location.Y - 25));
            //    }
            //    if (decision == DecisionOfPlayer.Beat)
            //    {
            //        g.DrawString("Бито!", new Font("TimesNewRoman", 16), new SolidBrush(Color.Black), new Point(location.X, location.Y - 25));
            //    }
            //}
            //if (role == RoleOfPlayer.Defender)
            //{
            //    if (decision == DecisionOfPlayer.Nothing)
            //    {
            //        g.DrawString("Я кроюсь!", new Font("TimesNewRoman", 16), new SolidBrush(Color.Black), new Point(location.X, location.Y - 25));
            //    }
            //    if (decision == DecisionOfPlayer.TakeCards)
            //    {
            //        g.DrawString("Я беру!", new Font("TimesNewRoman", 16), new SolidBrush(Color.Black), new Point(location.X, location.Y - 25));
            //    }
            //}
            #endregion
            base.Draw(g);
        }
    }
}
