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

    class Human : Player
    {
        public static event ChooseEventHandler ChooseCard;

        public Human(int index, int count) : base(index, count)
        {
            show = true;
            role = RoleOfPlayer.Attacker;
            GameTable.ChooseCardEvent += CheckMouseLocation;
            Card.CheckCardIsOpenedEvent += hand.CheckCardIsOpened;
        }
        public override void Move(object sender, MoveEventArgs e)
        {
            if (e.PlayerNum == playerNum)
            {
                if (e.NumCardInHand > 0)
                    for (int i = 0; i < 36 && hand.NumberOfCards > 0; i++)
                    {
                        if (!(hand[i] is null) && hand[i].Chosen)
                        {
                            SendCardEventArgs message = new SendCardEventArgs
                            {
                                Role = role,
                                PlayerNum = e.PlayerNum,
                                SendCard = hand[i],
                                Index = i,
                                Trump = e.Trump,
                                NumCardInHand = e.NumCardInHand
                            };
                            GiveCard(this, message);
                            e.IsCompleted = message.IsCompleted;
                            break;
                        }
                    }
            }
        }
        public override void Decide(DecisionOfPlayer decision)
        {
            this.decision = decision;
            if (decision == DecisionOfPlayer.TakeCards)
                AnnulDecisions(this, new GameEventArgs());
        }
        public void CheckMouseLocation(object sender, ChooseEventArgs e)
        {
            if (e.Pointer.X >= 0 && e.Pointer.X <= 1600 && e.Pointer.Y >= 580 && e.Pointer.Y <= 900)
            {
                ChooseCard?.Invoke(this, e);
            }
        }
        public override void LeaveGameTable()
        {
            base.LeaveGameTable();
            GameTable.ChooseCardEvent -= CheckMouseLocation;
            Card.CheckCardIsOpenedEvent -= hand.CheckCardIsOpened;
            for (int i = 0, j = 0; j < hand.NumberOfCards && i < 36; i++)
            {
                if (!(hand[i] is null))
                {
                    j++;
                    ChooseCard -= hand[i].CheckPos;
                    GameTable.DragCardEvent -= hand[i].ComeToPlace;
                }
            }
        }
        public override void LoadSpecAndCardsEvents()
        {
            base.LoadSpecAndCardsEvents();
            for (int i = 0, j = 0; j < hand.NumberOfCards && i < 36; i++)
            {
                if (!(hand[i] is null))
                {
                    j++;
                    ChooseCard -= hand[i].CheckPos;
                    ChooseCard += hand[i].CheckPos;
                }
            }
            Card.CheckCardIsOpenedEvent -= hand.CheckCardIsOpened;
            GameTable.ChooseCardEvent -= CheckMouseLocation;
            Card.CheckCardIsOpenedEvent += hand.CheckCardIsOpened;
            GameTable.ChooseCardEvent += CheckMouseLocation;
        }
    }
}
