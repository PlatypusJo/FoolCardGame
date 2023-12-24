using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Durak__Fool_
{
    public partial class MainMenu : Form
    {
        private int numberOfPlayers;
        private int typeOfMenu;
        private int beginerCount, advancedCount, masterCount, indexOfPlayer;
        public GameTable GameForm;

        public MainMenu()
        {
            InitializeComponent();
            numberOfPlayers = 0;
            beginerCount = advancedCount = masterCount = indexOfPlayer = 0;
            typeOfMenu = 1;
            GameForm = new GameTable(this);
            this.BackgroundImage = Properties.Resources.gameTableMenu;
        }

        private void TwoPlayersOrBeginner_MouseUp(object sender, MouseEventArgs e)
        {
            if (typeOfMenu == 1)
            {
                numberOfPlayers = 2;
                GameForm.SetNumberOfPlayers(numberOfPlayers, 0);
                indexOfPlayer++;
                typeOfMenu = 2;
                TwoPlayersOrBeginner.Text = "Новичок";
                ThreePlayersOrAmateur.Text = "Игрок";
                FourPlayersOrMaster.Text = "Мастер";
                LoadOrBack.Text = "Назад";
                About.Text = "Начать игру";
                Exit.Visible = false;
                this.BackgroundImage = Properties.Resources.gameTableChoose;
            }
            else if (typeOfMenu == 2)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        {
                            if (beginerCount + advancedCount + masterCount < numberOfPlayers - 1)
                            {
                                beginerCount++;
                                GameForm.AddPlayer(1, indexOfPlayer);
                                indexOfPlayer++;
                                TwoPlayersOrBeginner.Text = "Новичок" + " x " + beginerCount;
                            }
                        }
                        break;
                    case MouseButtons.Right:
                        {
                            if (beginerCount > 0)
                            {
                                beginerCount--;
                                GameForm.RemovePlayer(ref indexOfPlayer, 1);
                                if (beginerCount == 0)
                                {
                                    TwoPlayersOrBeginner.Text = "Новичок";
                                }
                                else
                                {
                                    TwoPlayersOrBeginner.Text = "Новичок" + " x " + beginerCount;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void ThreePlayersOrAmateur_MouseUp(object sender, MouseEventArgs e)
        {
            if (typeOfMenu == 1)
            {
                numberOfPlayers = 3;
                GameForm.SetNumberOfPlayers(numberOfPlayers, 0);
                indexOfPlayer++;
                typeOfMenu = 2;
                TwoPlayersOrBeginner.Text = "Новичок";
                ThreePlayersOrAmateur.Text = "Игрок";
                FourPlayersOrMaster.Text = "Мастер";
                LoadOrBack.Text = "Назад";
                About.Text = "Начать игру";
                Exit.Visible = false;
                this.BackgroundImage = Properties.Resources.gameTableChoose;
            }
            else if (typeOfMenu == 2)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        {
                            if (beginerCount + advancedCount + masterCount < numberOfPlayers - 1)
                            {
                                advancedCount++;
                                GameForm.AddPlayer(2, indexOfPlayer);
                                indexOfPlayer++;
                                ThreePlayersOrAmateur.Text = "Игрок" + " x " + advancedCount;
                            }
                        }
                        break;
                    case MouseButtons.Right:
                        {
                            if (advancedCount > 0)
                            {
                                advancedCount--;
                                GameForm.RemovePlayer(ref indexOfPlayer, 2);
                                if (advancedCount == 0)
                                {
                                    ThreePlayersOrAmateur.Text = "Игрок";
                                }
                                else
                                {
                                    ThreePlayersOrAmateur.Text = "Игрок" + " x " + advancedCount;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void FourPlayersOrMaster_MouseUp(object sender, MouseEventArgs e)
        {
            if (typeOfMenu == 1)
            {
                numberOfPlayers = 4;
                GameForm.SetNumberOfPlayers(numberOfPlayers, 0);
                indexOfPlayer++;
                typeOfMenu = 2;
                TwoPlayersOrBeginner.Text = "Новичок";
                ThreePlayersOrAmateur.Text = "Игрок";
                FourPlayersOrMaster.Text = "Мастер";
                LoadOrBack.Text = "Назад";
                About.Text = "Начать игру";
                Exit.Visible = false;
                this.BackgroundImage = Properties.Resources.gameTableChoose;
            }
            else if (typeOfMenu == 2)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        {
                            if (beginerCount + advancedCount + masterCount < numberOfPlayers - 1)
                            {
                                masterCount++;
                                GameForm.AddPlayer(3, indexOfPlayer);
                                indexOfPlayer++;
                                FourPlayersOrMaster.Text = "Мастер" + " x " + masterCount;
                            }
                        }
                        break;
                    case MouseButtons.Right:
                        {
                            if (masterCount > 0)
                            {
                                masterCount--;
                                GameForm.RemovePlayer(ref indexOfPlayer, 3);
                                if (masterCount == 0)
                                {
                                    FourPlayersOrMaster.Text = "Мастер";
                                }
                                else
                                {
                                    FourPlayersOrMaster.Text = "Мастер" + " x " + masterCount;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void LoadOrBack_Click(object sender, EventArgs e)
        {
            if (typeOfMenu == 1)
            {
                GameForm.LoadSave();
            }
            else if (typeOfMenu == 2)
            {
                numberOfPlayers = 0;
                GameForm.Back();
                indexOfPlayer = 0;
                typeOfMenu = 1;
                TwoPlayersOrBeginner.Text = "Игра вдвоём";
                ThreePlayersOrAmateur.Text = "Игра втроём";
                FourPlayersOrMaster.Text = "Игра вчетвером";
                LoadOrBack.Text = "Загрузить игру";
                About.Text = "О программе";
                Exit.Visible = true;
                this.BackgroundImage = Properties.Resources.gameTableMenu;
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            if (typeOfMenu == 1)
            {
                About about = new About(this);
                this.Hide();
                about.Show();
            }
            else if (typeOfMenu == 2)
            {
                if (beginerCount + advancedCount + masterCount == numberOfPlayers - 1)
                {
                    numberOfPlayers = 0;
                    indexOfPlayer = 0;
                    typeOfMenu = 1;
                    beginerCount = 0;
                    advancedCount = 0;
                    masterCount = 0;
                    TwoPlayersOrBeginner.Text = "Игра вдвоём";
                    ThreePlayersOrAmateur.Text = "Игра втроём";
                    FourPlayersOrMaster.Text = "Игра вчетвером";
                    LoadOrBack.Text = "Загрузить игру";
                    About.Text = "О программе";
                    Exit.Visible = true;
                    this.BackgroundImage = Properties.Resources.gameTableMenu;
                    this.Hide();
                    GameForm.Show();
                    GameForm.StartGame();
                }
                    
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
    }
}
