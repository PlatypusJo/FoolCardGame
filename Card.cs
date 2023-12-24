using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Durak__Fool_
{
    [Serializable]
    public class Card
    {
        public static event CheckCardIsOpenedEventHandler CheckCardIsOpenedEvent;

        private Suit suit;
        private int rank;
        private int dX, dY;//хранят коэффициент смещения карты
        private Image face, back;
        private Point curLocation, nextLocation; //хранят текущую и следующую позиции
        private Rectangle rect;
        private bool chosen;

        public Card(Suit suit, int rank, int x, int y)
        {
            this.suit = suit;
            this.rank = rank;
            chosen = false;
            nextLocation.X = curLocation.X = x;
            nextLocation.Y = curLocation.Y = y;
            rect = new Rectangle(0, 0, 80, 120);
            back = new Bitmap(Properties.Resources.backcard);
            face = (Bitmap)Properties.Resources.ResourceManager.GetObject($"{(int)suit}" + $"{rank}");
            GameTable.DragCardEvent += ComeToPlace;
        }
        public void Draw(Graphics graphics, bool vis)
        {
            if (vis)
                graphics.DrawImage(face, curLocation.X, curLocation.Y, rect.Width, rect.Height);
            else
                graphics.DrawImage(back, curLocation.X, curLocation.Y, rect.Width, rect.Height);
        }
        public void CheckPos(object sender, ChooseEventArgs eArgs)
        {
            eArgs.CardInHand = this;
            if (CheckCardIsOpenedEvent?.Invoke(this, eArgs) ?? false)
            {
                if (!chosen && eArgs.Pointer.X >= curLocation.X && eArgs.Pointer.X <= curLocation.X + rect.Width
                    && eArgs.Pointer.Y >= curLocation.Y && eArgs.Pointer.Y <= curLocation.Y + rect.Height)
                {
                    chosen = true;
                     curLocation.Y -= 20;
                     nextLocation.Y -= 20;
                }
                else
                {
                    if (chosen && !(eArgs.Pointer.X >= curLocation.X && eArgs.Pointer.X <= curLocation.X + rect.Width
                    && eArgs.Pointer.Y >= curLocation.Y && eArgs.Pointer.Y <= curLocation.Y + 20 + rect.Height))
                    {
                        chosen = false;
                        curLocation.Y += 20;
                        nextLocation.Y += 20;
                        
                    }
                }
            }
            else
            {
                if (!chosen && eArgs.Pointer.X >= curLocation.X && eArgs.Pointer.X <= curLocation.X + eArgs.DX - 1
                    && eArgs.Pointer.Y >= curLocation.Y && eArgs.Pointer.Y <= curLocation.Y + rect.Height)
                {
                    chosen = true;
                    curLocation.Y -= 20;
                    nextLocation.Y -= 20; 
                }
                else
                {
                    if (chosen && !(eArgs.Pointer.X >= curLocation.X && eArgs.Pointer.X <= curLocation.X + eArgs.DX - 1
                    && eArgs.Pointer.Y >= curLocation.Y && eArgs.Pointer.Y <= curLocation.Y + 20 + rect.Height))
                    {
                        chosen = false;
                        curLocation.Y += 20;
                        nextLocation.Y += 20;
                        
                    }
                }
            }
        }
        public void ComeToPlace(object sender, ref bool e)
        {
            #region Nothing
            //int dX = ((finX - point.X) / Math.Abs(finX - point.X == 0 ? 1 : finX - point.X)) * 10;
            //int dY = ((finY - point.Y) / Math.Abs(finY - point.Y == 0 ? 1 : finY - point.Y)) * 10;
            //int dX = (finX - point.X) / 5; // 2, 5, 10 - от этого зависит скорость анимации
            //int dY = (finY - point.Y) / 5;
            //while (point.X != finX || point.Y != finY)
            //{
            //    if (point.X != finX)
            //        point.X += dX;
            //    if (point.Y != finY)
            //        point.Y += dY;
            //    DragCardEvent?.Invoke(this, new EventArgs());    
            //}
            #endregion
            if (curLocation.X != nextLocation.X || curLocation.Y != nextLocation.Y)
            {
                e = true;
                if (curLocation.X != nextLocation.X)
                    curLocation.X += dX;
                if (curLocation.Y != nextLocation.Y)
                    curLocation.Y += dY;
            }
            else
            {
                if (dX != 0 || dY != 0)
                {
                    e = false;
                    dX = 0;
                    dY = 0;
                }   
            }
        }
        public void SetNextLocation(int finX, int finY)
        {
            curLocation.X = (curLocation.X / 10) * 10;
            curLocation.Y = (curLocation.Y / 10) * 10;
            nextLocation.X = finX;
            nextLocation.Y = finY;
            dX = (finX - curLocation.X) / 10;
            dY = (finY - curLocation.Y) / 10;
        }
        public Suit Suit
        {
            get => this.suit;
        }
        public int Rank
        {
            get => this.rank;
        }
        public bool Chosen
        {
            get => this.chosen;
            set => chosen = value;
        }
    }
}
