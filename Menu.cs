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
    class Menu
    {
        public event MenuEventHandler ChooseNumberOfPlayersEvent;
        public event MenuEventHandler AddNewPlayerEvent;
        public event MenuEventHandler RemovePlayerEvent;
        public event MenuEventHandler BackEvent;
        public event MenuEventHandler StartEvent;
        public event MenuEventHandler ExitEvent;
        public event MenuEventHandler LoadEvent;

        private int numberOfPlayers;
        private int typeOfMenu;
        private int beginerCount, advancedCount, masterCount;

        private MenuEventArgs message;
        private Image p1;
        private Image p2;
        private Image p3;
        private Image cancel;
        private Image start;


        public Menu()
        {
            numberOfPlayers = 0;
            beginerCount = advancedCount = masterCount = 0;
            typeOfMenu = 1;

            message = new MenuEventArgs();

            p1 = new Bitmap(Properties.Resources.gamefor2);
            p2 = new Bitmap(Properties.Resources.gamefor3);
            p3 = new Bitmap(Properties.Resources.gamefor4);
            cancel = new Bitmap(Properties.Resources.exit);
            start = new Bitmap(Properties.Resources.load);
        }

        public void Highlight(Point point)
        {
            switch (typeOfMenu)
            {
                case 1:
                    {
                        if (point.X > 75 && point.X < 374 && point.Y < 229 && point.Y > 180)
                            p1 = new Bitmap(Properties.Resources.gamefo2rchoose);
                        else if (point.X > 75 && point.X < 374 && point.Y < 289 && point.Y > 240)
                            p2 = new Bitmap(Properties.Resources.gamefo3rchoose);
                        else if (point.X > 75 && point.X < 374 && point.Y < 349 && point.Y > 300)
                            p3 = new Bitmap(Properties.Resources.gamefo4rchoose);
                        else if (point.X > 75 && point.X < 374 && point.Y < 409 && point.Y > 360)//
                            start = new Bitmap(Properties.Resources.loadactive);
                        else if (point.X > 75 && point.X < 374 && point.Y < 469 && point.Y > 420)
                            cancel = new Bitmap(Properties.Resources.exitchoose);
                        else
                        {
                            p1 = new Bitmap(Properties.Resources.gamefor2);
                            p2 = new Bitmap(Properties.Resources.gamefor3);
                            p3 = new Bitmap(Properties.Resources.gamefor4);
                            start = new Bitmap(Properties.Resources.load);
                            cancel = new Bitmap(Properties.Resources.exit);
                        }
                    }
                    break;
                case 2:
                    {
                        if (point.X > 75 && point.X < 374 && point.Y < 229 && point.Y > 180)
                        {
                            switch (beginerCount)
                            {
                                case 0: p1 = new Bitmap(Properties.Resources.beginerh); break;
                                case 1: p1 = new Bitmap(Properties.Resources.beginerx1h); break;
                                case 2: p1 = new Bitmap(Properties.Resources.beginerx2h); break;
                                case 3: p1 = new Bitmap(Properties.Resources.beginerx3h); break;
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 289 && point.Y > 240)
                        {
                            switch (advancedCount)
                            {
                                case 0: p2 = new Bitmap(Properties.Resources.advancedh); break;
                                case 1: p2 = new Bitmap(Properties.Resources.advancedx1h); break;
                                case 2: p2 = new Bitmap(Properties.Resources.advancedx2h); break;
                                case 3: p2 = new Bitmap(Properties.Resources.advancedx3h); break;
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 349 && point.Y > 300)
                        {
                            switch (masterCount)
                            {
                                case 0: p3 = new Bitmap(Properties.Resources.masterh); break;
                                case 1: p3 = new Bitmap(Properties.Resources.masterx1h); break;
                                case 2: p3 = new Bitmap(Properties.Resources.masterx2h); break;
                                case 3: p3 = new Bitmap(Properties.Resources.masterx3h); break;
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 409 && point.Y > 360)//
                            start = new Bitmap(Properties.Resources.playchoose);
                        else if (point.X > 75 && point.X < 374 && point.Y < 469 && point.Y > 420)
                            cancel = new Bitmap(Properties.Resources.backchoose);
                        else
                        {
                            switch (beginerCount)
                            {
                                case 0: p1 = new Bitmap(Properties.Resources.beginer); break;
                                case 1: p1 = new Bitmap(Properties.Resources.beginerx1); break;
                                case 2: p1 = new Bitmap(Properties.Resources.beginerx2); break;
                                case 3: p1 = new Bitmap(Properties.Resources.beginerx3); break;
                            }
                            switch (advancedCount)
                            {
                                case 0: p2 = new Bitmap(Properties.Resources.advanced); break;
                                case 1: p2 = new Bitmap(Properties.Resources.advancedx1); break;
                                case 2: p2 = new Bitmap(Properties.Resources.advancedx2); break;
                                case 3: p2 = new Bitmap(Properties.Resources.advancedx3); break;
                            }
                            switch (masterCount)
                            {
                                case 0: p3 = new Bitmap(Properties.Resources.master); break;
                                case 1: p3 = new Bitmap(Properties.Resources.masterx1); break;
                                case 2: p3 = new Bitmap(Properties.Resources.masterx2); break;
                                case 3: p3 = new Bitmap(Properties.Resources.masterx3); break;
                            }
                            cancel = new Bitmap(Properties.Resources.back);
                            start = new Bitmap(Properties.Resources.play);
                        }
                    }
                    break;
            }
        }

        public void Choose(Point point, MouseButtons mButton)
        {
            switch (typeOfMenu)
            {
                case 1:
                    {
                        if (point.X > 75 && point.X < 374 && point.Y < 229 && point.Y > 180)
                        {
                            message.PlayersNumber = numberOfPlayers = 2;
                            message.IndexOfPlayer = 0;
                            ChooseNumberOfPlayersEvent?.Invoke(this, message);
                            message.IndexOfPlayer++;
                            typeOfMenu = 2;
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 289 && point.Y > 240)
                        {
                            message.PlayersNumber = numberOfPlayers = 3;
                            message.IndexOfPlayer = 0;
                            ChooseNumberOfPlayersEvent?.Invoke(this, message);
                            message.IndexOfPlayer++;
                            typeOfMenu = 2;
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 349 && point.Y > 300)
                        {
                            message.PlayersNumber = numberOfPlayers = 4;
                            message.IndexOfPlayer = 0;
                            ChooseNumberOfPlayersEvent?.Invoke(this, message);
                            message.IndexOfPlayer++;
                            typeOfMenu = 2;
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 409 && point.Y > 360)//
                        {
                            LoadEvent?.Invoke(this, message);
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 469 && point.Y > 420)
                        {
                            ExitEvent?.Invoke(this, message);
                        }
                        if (typeOfMenu == 2)
                        {
                            p1 = new Bitmap(Properties.Resources.beginer);
                            p2 = new Bitmap(Properties.Resources.advanced);
                            p3 = new Bitmap(Properties.Resources.master);
                            cancel = new Bitmap(Properties.Resources.back);
                            start = new Bitmap(Properties.Resources.play);
                        }
                    }
                    break;
                case 2:
                    {
                        if (point.X > 75 && point.X < 374 && point.Y < 229 && point.Y > 180)
                        {
                            switch (mButton)
                            {
                                case MouseButtons.Left:
                                {
                                    if (beginerCount + advancedCount + masterCount < numberOfPlayers - 1)
                                    {
                                        beginerCount++;
                                        message.TypeOfPlayer = 1;
                                        AddNewPlayerEvent?.Invoke(this, message);
                                        message.IndexOfPlayer++;
                                    }
                                }
                                break;
                                case MouseButtons.Right:
                                {
                                    if (beginerCount > 0)
                                    {
                                        beginerCount--;
                                        message.TypeOfPlayer = 1;
                                        RemovePlayerEvent?.Invoke(this, message);
                                    }
                                }
                                break;
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 289 && point.Y > 240)
                        {
                            switch (mButton)
                            {
                                case MouseButtons.Left:
                                    {
                                        if (beginerCount + advancedCount + masterCount < numberOfPlayers - 1)
                                        {
                                            advancedCount++;
                                            message.TypeOfPlayer = 2;
                                            AddNewPlayerEvent?.Invoke(this, message);
                                            message.IndexOfPlayer++;
                                        }
                                    }
                                    break;
                                case MouseButtons.Right:
                                    {
                                        if (advancedCount > 0)
                                        {
                                            advancedCount--;
                                            message.TypeOfPlayer = 2;
                                            RemovePlayerEvent?.Invoke(this, message);
                                        }
                                    }
                                    break;
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 349 && point.Y > 300)
                        {
                            switch (mButton)
                            {
                                case MouseButtons.Left:
                                    {
                                        if (beginerCount + advancedCount + masterCount < numberOfPlayers - 1)
                                        {
                                            masterCount++;
                                            message.TypeOfPlayer = 3;
                                            AddNewPlayerEvent?.Invoke(this, message);
                                            message.IndexOfPlayer++;
                                        }
                                    }
                                    break;
                                case MouseButtons.Right:
                                    {
                                        if (masterCount > 0)
                                        {
                                            masterCount--;
                                            message.TypeOfPlayer = 3;
                                            RemovePlayerEvent?.Invoke(this, message);
                                        }
                                    }
                                    break;
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 409 && point.Y > 360)//
                        { 
                            if (beginerCount+advancedCount+masterCount == numberOfPlayers - 1)
                            {
                                StartEvent?.Invoke(this, message);
                            }
                        }
                        else if (point.X > 75 && point.X < 374 && point.Y < 469 && point.Y > 420)
                        {
                            numberOfPlayers = 0;
                            beginerCount = 0;
                            advancedCount = 0;
                            masterCount = 0;
                            typeOfMenu = 1;
                            message.IndexOfPlayer = 0;
                            message.PlayersNumber = 0;
                            message.TypeOfPlayer = 0;
                            BackEvent?.Invoke(this, message);
                        }
                    }
                    break;
            }
        }

        public void Draw(Graphics graphics)
        {
            switch (typeOfMenu)
            {
                case 1:
                    {
                        graphics.DrawImage(p1, 75, 180);
                        graphics.DrawImage(p2, 75, 240);
                        graphics.DrawImage(p3, 75, 300);
                        graphics.DrawImage(start, 75, 360);
                        graphics.DrawImage(cancel, 75, 420);
                    } 
                    break;
                case 2:
                    {
                        graphics.DrawImage(p1, 75, 180);
                        graphics.DrawImage(p2, 75, 240);
                        graphics.DrawImage(p3, 75, 300);
                        graphics.DrawImage(start, 75, 360);
                        graphics.DrawImage(cancel, 75, 420);
                    } 
                    break;
            }
        }
    }
}
