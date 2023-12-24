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
    
    abstract class Player
    {
        public event SendCardEventHandler LayCardEvent;
        public static event SendCardEventHandler AscCardsEvent;
        public static event GameEventHandler AnnulDecisionOtherPlayersEvent;

        protected Point location;
        protected int playerNum;
        protected bool show;
        protected DeckHand hand;
        protected RoleOfPlayer role;
        protected DecisionOfPlayer decision;

        public Player(int index, int count)
        {           
            playerNum = index;
            switch (index)
            {
                case 0:
                    location = new Point(750, 600);
                    break;
                case 1:
                    if (count == 2) location = new Point(750, 40); else location = new Point(190, 200); // x = 40
                    break;
                case 2:
                    if (count == 3) location = new Point(1400, 200); else location = new Point(750, 40); //x = 1250
                    break;
                case 3:
                    location = new Point(1400, 200);
                    break;   
            }
            hand = new DeckHand(location.X, location.Y, index);
            LayCardEvent += hand.GiveCard;
            GameTable.PlayersMoveEvent += Move;
        }
        public abstract void Decide(DecisionOfPlayer decision = DecisionOfPlayer.Nothing);
        public virtual void Move(object sender, MoveEventArgs eArgs) { }
        public virtual void Draw(Graphics g)
        {
            hand.Draw(g, this.show);
        }
        public virtual void LeaveGameTable()
        {
            LayCardEvent -= hand.GiveCard;
            GameTable.PlayersMoveEvent -= Move;
            DeckTable.DealCardEvent -= hand.TakeCard;
            GameField.ReturnCardsEvent -= hand.TakeCard;
        }
        public virtual void LoadSpecAndCardsEvents()
        {
            for (int i = 0, j = 0; j < hand.NumberOfCards && i < 36; i++)
            {
                if (!(hand[i] is null))
                {
                    j++;
                    GameTable.DragCardEvent -= hand[i].ComeToPlace;
                    GameTable.DragCardEvent += hand[i].ComeToPlace;
                }
            }
        }
        public void IsNeedCards()
        {
            if (hand.NumberOfCards < 6)
            {
                SendCardEventArgs message = new SendCardEventArgs
                {
                    NumCardInHand = 6 - hand.NumberOfCards,
                    PlayerNum = playerNum,
                };
                AscCardsEvent?.Invoke(this, message);
            }
        }
        public int FindMinRankOfTrumpSuit(Suit trump)
        {
            int min = 9;
            for (int i = 0; hand[i].Suit == trump; i++)
            {
                if (hand[i].Rank >= 0 && hand[i].Rank < min)
                    min = hand[i].Rank;  
            }
            return min;
        }
        protected void GiveCard(object sender, SendCardEventArgs e)
        {
            LayCardEvent?.Invoke(sender, e);
        }
        protected void AnnulDecisions(object sender, GameEventArgs e)
        {
            AnnulDecisionOtherPlayersEvent?.Invoke(sender, e);
        }
        public int Number
        {
            get => playerNum;
        }
        public DeckReceiver Hand
        {
            get 
            { 
                return this.hand; 
            }
        }
        public RoleOfPlayer Role
        {
            set => role = value;
            get => role;
        }
        public DecisionOfPlayer Decision
        {
            get => decision;
        }
    }
}
