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

    public class GameField
    {
        public static event SendCardEventHandler ReturnCardsEvent; // возвращает карты, подписчик - hand в player
        public static event SendCardEventHandler RememberPlayersCardsEvent; // передаёт карты, которые забирает игрок, profi
        public static event SendCardEventHandler AddToDischargeEvent; // передаёт карты, которые ушли в сброс, profi и amature
        public static event GameEventHandler AnnulDecisionsOtherPlayersEvent;

        protected Card[,] cells;
        private int line1, line2;
        private Point point;
        public GameField()
        {
            line1 = 0;
            line2 = 0;
            cells = new Card[2, 6];
            point = new Point(510, 400);
            GameTable.FinishRoundEvent += FinishRound;
            DeckHand.GiveCardEvent += AddCard;
            BotPlayer.CollectDataFieldEvent += ShowField;
        }
        public void AddCard(object sender, SendCardEventArgs e) //добавляет карты
        {
            if (sender is Human)
            {
                if (!CheckMove(e.Trump, e.SendCard, e.Role, e.NumCardInHand))
                    return;
            }
            if (e.Role == RoleOfPlayer.Attacker)
            {
                if (line1 < 6)
                {
                    if (cells[0, line1] is null)
                    {
                        cells[0, line1] = e.SendCard;
                        cells[0, line1].SetNextLocation(point.X + (line1 * 90), point.Y - 50);
                        cells[0, line1].Chosen = false;
                        line1++;
                        e.IsCompleted = true;
                    }
                }
            }
            if (e.Role == RoleOfPlayer.Defender)
            {
                if (line2 < 6)
                {
                    if (cells[1, line2] is null)
                    {
                        CheckNewCard(e.SendCard);
                        cells[1, line2] = e.SendCard;
                        cells[1, line2].SetNextLocation(point.X + (line2 * 90), point.Y);
                        cells[1, line2].Chosen = false; 
                        line2++;
                        e.IsCompleted = true;    
                    }
                }
            }
            
        }
        public void Draw(Graphics graphics)
        {
            int i = 0, j = 0;
            while (i < line1 || j < line2)
            {
                cells[0, i]?.Draw(graphics, true);
                cells[1, j]?.Draw(graphics, true);
                i++;
                j++;
            }
        }
        public void ShowField(object sender, CollectDataEventArgs e)
        {
            if (sender is BotPlayer)
            {
                Array.Copy(cells, e.field, 12);
                e.Line1 = line1;
                e.Line2 = line2;
            }
            else
            {
                if (sender is GameTable) e.Line1 = line1;
            }
                
        }
        public void FinishRound(object sender, MoveEventArgs e)
        {
            switch (e.Decision)
            {
                case DecisionOfPlayer.Nothing: DiscardCards(sender, e); break;
                case DecisionOfPlayer.TakeCards: ReturnCards(sender, e); break;
            }
        }
        public void LoadCardsEvents()
        {
            for (int i = 0; i < 6; i++)
            {
                if (!(cells[0, i] is null))
                {
                    GameTable.DragCardEvent -= cells[0, i].ComeToPlace;
                    GameTable.DragCardEvent += cells[0, i].ComeToPlace;
                }
                if (!(cells[1, i] is null))
                {
                    GameTable.DragCardEvent -= cells[1 ,i].ComeToPlace;
                    GameTable.DragCardEvent += cells[1, i].ComeToPlace;
                }
            }
        }
        private void ReturnCards(object sender, GameEventArgs e) //игрок берёт карты
        {
            SendCardEventArgs message = new SendCardEventArgs()
            {
                PlayerNum = e.PlayerNum,
                Trump = e.Trump,
                NumCardInHand = line1 + line2
            };
            while (line1 > 0 || line2 > 0)
            {
                if (line1 > 0)
                {
                    message.SendCard = cells[0, line1 - 1];
                    message.NumCardInHand--;
                    ReturnCardsEvent?.Invoke(sender, message);
                    RememberPlayersCardsEvent?.Invoke(this, message);
                    cells[0, line1 - 1] = null;
                    line1--;
                }
                if (line2 > 0)
                {
                    message.SendCard = cells[1, line2 - 1];
                    message.NumCardInHand--;
                    ReturnCardsEvent?.Invoke(sender, message);
                    RememberPlayersCardsEvent?.Invoke(this, message);
                    cells[1, line2 - 1] = null;
                    line2--;
                }
                message.SendCard = null;
            }
        }
        private void DiscardCards(object sender, GameEventArgs e) //сброс
        {
            SendCardEventArgs message = new SendCardEventArgs() { PlayerNum = e.PlayerNum };
            while (line1 > 0 || line2 > 0)
            {
                if (line1 > 0)
                {
                    GameTable.DragCardEvent -= cells[0, line1 - 1].ComeToPlace;
                    message.SendCard = cells[0, line1 - 1];
                    AddToDischargeEvent?.Invoke(this, message);
                    cells[0, line1 - 1] = null;
                    line1--;
                }
                if (line2 > 0)
                {
                    GameTable.DragCardEvent -= cells[1, line2 - 1].ComeToPlace;
                    message.SendCard = cells[1, line2 - 1];
                    AddToDischargeEvent?.Invoke(this, message);
                    cells[1, line2 - 1] = null;
                    line2--;
                }
                message.SendCard = null;
            }
        }
        private void CheckNewCard(Card card)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!(cells[0, i] is null))
                {
                    if (cells[0, i].Rank == card.Rank)
                        return;
                    //else
                    //    return;
                }
                if (!(cells[1, i] is null))
                {
                    if (cells[1, i].Rank == card.Rank)
                        return;
                    //else
                    //    return;
                }
            }
            AnnulDecisionsOtherPlayersEvent?.Invoke(this, new GameEventArgs());
        }
        private bool CheckMove(Suit trump, Card card, RoleOfPlayer role, int NumCardInOponentHand) //сравнивает карты
        {
            if (role == RoleOfPlayer.Attacker)
            {
                if (line1 < 6 && line1 + 1 <= line2 + NumCardInOponentHand)
                {
                    if (line1 == 0)
                        return true;
                    if (cells[0, line1] is null)
                        for (int i = 0; i < 6; i++)
                        {
                            if (!(cells[0, i] is null))
                                if (cells[0, i].Rank == card.Rank)
                                    return true;
                            if (!(cells[1, i] is null))
                                if (cells[1, i].Rank == card.Rank)
                                    return true;
                        }
                }
            }
            if (role == RoleOfPlayer.Defender)
            {
                if (line2 < 6)
                {
                    if (cells[1, line2] is null)
                    {
                        return card.Suit == cells[0, line1 - 1].Suit && card.Rank > cells[0, line1 - 1].Rank ||
                            card.Suit == trump && (card.Suit != cells[0, line1 - 1].Suit ||
                            card.Suit == cells[0, line1 - 1].Suit && card.Rank > cells[0, line1 - 1].Rank);
                    }
                }
            }
            return false;
        } 
        public int Line1
        {
            get => line1;
        }
        public int Line2
        {
            get => line2;
        }
    }
}
