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

    class DeckTable : Deck
    {
        public static event SendCardEventHandler DealCardEvent;

        private Image trumpCardImage;

        public DeckTable(out Suit trump)
        {
            point.X = 350;
            point.Y = 400;
            countOfCards = 36;
            cards = new Card[36];
            for (Suit i = Suit.Hearts; i <= Suit.Clubs; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cards[((int)i + j) + (int)i * 8] = new Card(i, j, point.X - 150, point.Y);
                }
            }
            Mix();
            trump = cards[0].Suit;
            trumpCardImage = (Bitmap)Properties.Resources.ResourceManager.GetObject($"{(int)cards[0].Suit}" + $"{cards[0].Rank}");
            Player.AscCardsEvent += GiveCard;
            //BotPlayer.CollectDataFieldEvent += ShowDealerHand;
        }
        public override void GiveCard(object sender, SendCardEventArgs e) //выдаёт карту
        {
            if (countOfCards > 0)
            {
                if (e.NumCardInHand > countOfCards)
                    e.NumCardInHand = countOfCards;
                e.Trump = cards[0].Suit;
                for (int i = countOfCards - 1, end = i - e.NumCardInHand; countOfCards > 0 && i > end; i--)
                {
                    e.SendCard = cards[i];
                    e.NumCardInHand--;
                    DealCardEvent?.Invoke(sender, e);
                    countOfCards--;
                    cards[i] = null;
                }
                if (countOfCards == 0) 
                    trumpCardImage = ChangeImageOpacity((Bitmap)trumpCardImage, 140); 
            }
        }
        private void Mix() // функция перемешивания
        { 
            Random random = new Random();
            for (int i = cards.Length-1; i >= 0; i--) 
            {
                int j = random.Next(i + 1);
                if (j != i)
                {
                    Card temp = cards[j];
                    cards[j] = cards[i];
                    cards[i] = temp;
                }
            }
        }
        public override void Draw(Graphics graphics, bool vis) // рисует колоду
        {
            graphics.DrawImage(trumpCardImage, point.X + 40, point.Y - 40, 80, 120);
            for (int i = 1; i < countOfCards; i++)
            {
                graphics.DrawImage(new Bitmap(Properties.Resources.backcard), point.X - i * 2, point.Y, 80, 120);
            }
            if (countOfCards > 0)
                graphics.DrawString(countOfCards.ToString(), new Font(Control.DefaultFont, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(350, 370));
        }
        public void LoadCardsEvents()
        {
            for (int i = 0; i < countOfCards; i++)
            {
                GameTable.DragCardEvent -= cards[i].ComeToPlace;
                GameTable.DragCardEvent += cards[i].ComeToPlace;
            }
        }
        private Bitmap ChangeImageOpacity(Bitmap image, int a) // меняет прозрачность козырной карты (когда колода пуста)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb(a, image.GetPixel(i, j).R, image.GetPixel(i, j).G, image.GetPixel(i, j).B));
                }
            }
            return bmp;
        }
    }
}
