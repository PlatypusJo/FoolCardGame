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

    class Profi : BotPlayer
    {
        public static event GameEventHandler CollectDataPlayersEvent;

        protected List<Oponent> oponents;

        public Profi(int index, int count) : base(index, count)
        {
            GameField.RememberPlayersCardsEvent += RememberPlayersHand;
            GameField.AddToDischargeEvent += AddToDischarge;
            oponents = new List<Oponent>();
            for (int i = index + 1, j = 0; j < count; j++, i++)
            {
                oponents.Add(new Oponent(i % count));
            }
            GameTable.GiveInfoAboutPlayers += CollectDataPlayers;
        }

        public override void Move(object sender, MoveEventArgs e)
        {
            #region NOTGOOD
            if (e.PlayerNum == playerNum)
            {
                CollectDataPlayersEvent?.Invoke(this, new GameEventArgs { PlayerNum = playerNum });
                SendCardEventArgs message = new SendCardEventArgs()
                {
                    PlayerNum = e.PlayerNum,
                    Trump = e.Trump,
                    Role = role
                };
                if (role == RoleOfPlayer.Attacker)
                {
                    CollectDataEventArgs data = new CollectDataEventArgs();
                    CollectDataField(this, data);
                    if (data.Line1 < 6 && e.NumCardInHand > 0 && hand.NumberOfCards > 0 && data.Line1 + 1 <= data.Line2 + e.NumCardInHand)
                    {
                        if (data.Line1 == 0)
                        {
                            message.SendCard = hand[hand.NumberOfCards - 1];
                            message.Index = hand.NumberOfCards - 1;
                            GiveCard(this, message);
                            if (message.IsCompleted)
                                e.IsCompleted = true;
                            return;
                        }
                        else
                        {
                            for (int j = hand.NumberOfCards - 1; j >= 0; j--)
                            {
                                for (int k = 0; k < 6; k++)
                                {
                                    if (!(data.field[0, k] is null))
                                    {
                                        if (data.field[0, k].Rank == hand[j].Rank)
                                        {
                                            message.SendCard = hand[j];
                                            message.Index = j;
                                            GiveCard(this, message);
                                            if (message.IsCompleted)
                                                e.IsCompleted = true;
                                            return;
                                        }
                                    }
                                    if (!(data.field[1, k] is null))
                                    {
                                        if (data.field[1, k].Rank == hand[j].Rank)
                                        {
                                            message.SendCard = hand[j];
                                            message.Index = j;
                                            GiveCard(this, message);
                                            if (message.IsCompleted)
                                                e.IsCompleted = true;
                                            return;
                                        }
                                    }
                                }
                            }
                            if (!e.IsCompleted)
                            {
                                if (e.Decision == DecisionOfPlayer.TakeCards)
                                    decision = DecisionOfPlayer.LetTakeCards;
                                if (e.Decision == DecisionOfPlayer.Nothing)
                                    decision = DecisionOfPlayer.Beat;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (e.Decision == DecisionOfPlayer.TakeCards)
                            decision = DecisionOfPlayer.LetTakeCards;
                        if (e.Decision == DecisionOfPlayer.Nothing)
                            decision = DecisionOfPlayer.Beat;
                        return;
                    }
                }
                if (role == RoleOfPlayer.Defender && decision != DecisionOfPlayer.TakeCards)
                {
                    CollectDataEventArgs data = new CollectDataEventArgs();
                    CollectDataField(this, data);
                    #region FORBETTERTIME
                    //Card min = hand[0];
                    //for (int j = 0; j < hand.NumberOfCards; j++)
                    //{
                    //    if (hand[j].Suit != e.Trump && hand[j].Suit != Suit.NotExist)
                    //    {
                    //        if (min.Suit == e.Trump)
                    //        {
                    //            min = hand[j];
                    //            message.Index = j;
                    //        }
                    //        else
                    //        {
                    //            if (hand[j].Rank < min.Rank)
                    //            {
                    //                min = hand[j];
                    //                message.Index = j;
                    //            };
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (min.Suit != Suit.NotExist && min.Suit == e.Trump && hand[j].Rank < min.Rank)
                    //        {
                    //            min = hand[j];
                    //            message.Index = j;
                    //        }
                    //    }
                    //}
                    //message.SendCard = min;
                    #endregion
                    if (data.Line1 < 6)
                        for (int i = 0; i < data.Line1; i++)
                        {
                            if (!(data.field[0, i] is null) && data.field[1, i] is null)
                            {
                                for (int j = 0; j < hand.NumberOfCards; j++)
                                {
                                    if (!(hand[j] is null) && hand[j].Suit == data.field[0, i].Suit && hand[j].Rank > data.field[0, i].Rank ||
                                    hand[j].Suit == e.Trump && (hand[j].Suit != data.field[0, i].Suit ||
                                    hand[j].Suit == data.field[0, i].Suit && hand[j].Rank > data.field[0, i].Rank))
                                    {
                                        message.Index = j;
                                        message.SendCard = hand[j];
                                        GiveCard(this, message);
                                        if (message.IsCompleted)
                                            e.IsCompleted = true;
                                        return;
                                    }
                                }
                            }
                        }
                    else
                    {
                        if (data.field[1, data.Line1 - 1] is null)
                        {
                            decision = DecisionOfPlayer.TakeCards;
                            AnnulDecisions(this, new GameEventArgs());
                            return;
                        }
                        else
                            return;
                    }
                    decision = DecisionOfPlayer.TakeCards;
                    AnnulDecisions(this, new GameEventArgs());
                }
                #region Useless
                //for (int i = 0; i < 6 && !e.mademove; i++)
                //{
                //    if (e.gamefield[0, i] != null)
                //        if (!(e.gamefield[1, i] is null))
                //            continue;
                //        else
                //        {
                //            for (int j = 0; j < 36; j++)
                //                if (!(hand[j] is NullCard))
                //                {
                //                    if ((hand[j].Suit == e.trump && e.gamefield[0, i].Suit != e.trump) ||
                //                        (hand[j].Suit == e.trump && e.gamefield[0, i].Suit == e.trump && hand[j].Rank > e.gamefield[0, i].Rank) ||
                //                        (e.gamefield[0, i].Suit == hand[j].Suit && hand[j].Rank > e.gamefield[0, i].Rank))
                //                    {
                //                        e.role = role;
                //                        e.index = j;
                //                        e.sendcard = hand[j];
                //                        hand.GiveCard(this, e);
                //                        break;
                //                    }
                //                }
                //        }
                //}
                #endregion
            }
            #endregion
        }
        public void RememberPlayersHand(object sender, SendCardEventArgs e)
        {

        }
        public void AddToDischarge(object sender, SendCardEventArgs e)
        {

        }
        public override void LeaveGameTable()
        {
            base.LeaveGameTable();
            GameField.RememberPlayersCardsEvent -= RememberPlayersHand;
            GameField.AddToDischargeEvent -= AddToDischarge;
            for (int i = 0, j = 0; j < hand.NumberOfCards && i < 36; i++)
            {
                if (!(hand[i] is null))
                {
                    j++;
                    GameTable.DragCardEvent -= hand[i].ComeToPlace;
                }
            }
        }
        public override void LoadSpecAndCardsEvents()
        {
            base.LoadSpecAndCardsEvents();
            GameField.RememberPlayersCardsEvent -= RememberPlayersHand;
            GameField.AddToDischargeEvent -= AddToDischarge;
            GameField.RememberPlayersCardsEvent += RememberPlayersHand;
            GameField.AddToDischargeEvent += AddToDischarge;
        }
        private void CollectDataPlayers(object sender, MoveEventArgs e)
        {
            if (e.PlayerNum == playerNum)
            {
                for (int i = 0; i < oponents.Count; i++)
                {
                    if (oponents[i].number == e.OponentNum)
                    {
                        Oponent buf = oponents[i];
                        buf.cardsQuantity = e.NumCardInHand;
                        oponents[i] = buf;
                        break;
                    }
                }
            }
        }
    }
}
