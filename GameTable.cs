using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Durak__Fool_
{
    public enum Suit
    {
        NotExist = -1,
        Hearts,
        Diamonds,
        Spades,
        Clubs   
    }

    public enum RoleOfPlayer
    {
        Attacker,
        Defender
    }

    public enum DecisionOfPlayer
    { 
        Nothing,
        TakeCards,
        Beat,
        LetTakeCards
    }

    public delegate void DragCardEventHandler(object sender, ref bool e);

    public partial class GameTable : Form
    {
        public static event DragCardEventHandler DragCardEvent;
        public static event MoveEventHandler FinishRoundEvent;
        public static event MoveEventHandler PlayersMoveEvent;
        public static event ChooseEventHandler ChooseCardEvent;
        public static event MoveEventHandler GiveInfoAboutPlayers;

        BinaryFormatter formatter = new BinaryFormatter();
        Random rand = new Random();

        Suit trumpSuit;
        Deck dealer;
        GameField field;
        List<Player> players;
        MainMenu menu;
        bool humanDefender, isWait;
        int numberOfPlayers, curPlayer, prePlayer, mainPlayer;

        public GameTable(MainMenu mainMenu)
        {
            InitializeComponent();
            menu = mainMenu;
            players = new List<Player>();
            this.BackgroundImage = Properties.Resources.gameTableIm;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            //InitMenu();
        }
        
        public void Init()
        {
            //Игровые объекты
            this.MouseMove += CheckMouse;
            this.MouseClick += MClick;
            this.Paint += OnPaint;
            this.KeyUp += ExitToMenu;
            isWait = false;
            field = new GameField();
            dealer = new DeckTable(out trumpSuit);
            GameField.AnnulDecisionsOtherPlayersEvent += AnnulDecisionsOfPlayers;
            Player.AnnulDecisionOtherPlayersEvent += AnnulDecisionsOfPlayers;
            Profi.CollectDataPlayersEvent += GiveDataAboutPlayers;
            #region ForTheFuture
            //ShowGameFieldEvent += field.ShowField;
            //FinishRoundEvent += field.FinishRound;
            //BotPlayer.CollectDataFieldEvent += field.ShowField;
            //for (int i = 0; i < players.Count; i++)
            //{
            //    field.ReturnCardsEvent += players[i].Hand.TakeCard;
            //    dealer.GiveCardEvent += players[i].Hand.TakeCard;
            //    players[i].AscCardsEvent += dealer.GiveCard;
            //    players[i].Hand.GiveCardEvent += field.AddCard;
            //    PlayersMoveEvent += players[i].Move;
            //    if (players[i] is Human)
            //    {
            //        ChooseCardEvent += ((Human)players[i]).CheckMouseLocation;
            //    }
            //    if (players[i] is Amateur)
            //    {
            //        field.AddToDischargeEvent += ((Amateur)players[i]).AddToDischarge;
            //    }
            //    if (players[i] is Profi)
            //    {
            //        field.RememberPlayersCardsEvent += ((Profi)players[i]).RememberPlayersHand;
            //        field.AddToDischargeEvent += ((Profi)players[i]).AddToDischarge;
            //        for (int j = i; j < i + players.Count; j++)
            //        {
            //            ((Profi)players[i]).CollectDataPlayersEvent += players[j % players.Count].TellProfiAboutYourself;
            //        }
            //    }
            //}
            #endregion
            switch (numberOfPlayers)
            {
                case 2: MessageBox2.Visible = true; break;
                case 3: MessageBox1.Visible = MessageBox3.Visible = true; break;
                case 4: MessageBox1.Visible = MessageBox2.Visible = MessageBox3.Visible = true; break;
            }
            //Для игры
            timer1.Enabled = true;
            mainPlayer = rand.Next(players.Count);
            prePlayer = (mainPlayer + players.Count - 1) % players.Count;
        }

        private void ExitToMenu(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (!pictureBox1.Visible)
                {
                    timer1.Enabled = false;
                    pictureBox1.Visible = true;
                    pictureBox1.BackgroundImage = Properties.Resources.Pause;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                    ToMenuButton.Visible = true;
                    SaveToMenu.Visible = true;
                }
                else
                {
                    timer1.Enabled = true;
                    pictureBox1.Visible = false;
                    pictureBox1.BackgroundImage = Properties.Resources.Pause;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                    ToMenuButton.Visible = false;
                    SaveToMenu.Visible = false;
                }
                
            }
        }

        private void Tick(object sender, EventArgs e)
        {
            DragCardEvent?.Invoke(this, ref isWait);
            ShowButton();
            OutputMessages();
            ShowTip();
            Invalidate();  
            if (!isWait)
            {
                Play();
                //CheckFinish();
                CheckPassMove();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            dealer.Draw(graphics, true);
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Draw(graphics);
            } 
            field.Draw(graphics);
        }

        private void MakeSave()
        {
            DateTime curDate = DateTime.Now;
            string path = "Партия";
            path += "_" + curDate.Year;
            path += "-" + curDate.Month;
            path += "-" + curDate.Day;
            path += "_" + curDate.Hour;
            path += "-" + curDate.Minute;
            path += "-" + curDate.Second;
            path += ".fool";
            FileStream fileStream = new FileStream(path, FileMode.Create);

            formatter.Serialize(fileStream, field);
            formatter.Serialize(fileStream, dealer);

            formatter.Serialize(fileStream, trumpSuit);
            formatter.Serialize(fileStream, humanDefender);
            formatter.Serialize(fileStream, isWait);
            formatter.Serialize(fileStream, numberOfPlayers);
            formatter.Serialize(fileStream, curPlayer);
            formatter.Serialize(fileStream, prePlayer);
            formatter.Serialize(fileStream, mainPlayer);

            int length = players.Count;
            formatter.Serialize(fileStream, length);
            Player[] playersSave = new Player[length]; 
            for (int i = 0; i < playersSave.Length; i++)
            {
                playersSave[i] = players[i];
            }
            formatter.Serialize(fileStream, playersSave);
            fileStream.Close();
        }

        public void LoadSave()
        {
            string path; 
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "Fool games (*.fool)|*.fool";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    menu.Hide();
                    this.Show();
                    path = openFileDialog.FileName;
                    pictureBox1.Visible = false;
                    label1.Visible = true;
                    this.MouseMove += CheckMouse;
                    this.MouseClick += MClick;
                    this.Paint += OnPaint;
                    this.KeyUp += ExitToMenu;
                    isWait = false;

                    FileStream fileStream = new FileStream(path, FileMode.Open);

                    field = (GameField)formatter.Deserialize(fileStream);
                    dealer = (DeckTable)formatter.Deserialize(fileStream);

                    trumpSuit = (Suit)formatter.Deserialize(fileStream);
                    humanDefender = (bool)formatter.Deserialize(fileStream);
                    isWait = (bool)formatter.Deserialize(fileStream);
                    numberOfPlayers = (int)formatter.Deserialize(fileStream);
                    curPlayer = (int)formatter.Deserialize(fileStream);
                    prePlayer = (int)formatter.Deserialize(fileStream);
                    mainPlayer = (int)formatter.Deserialize(fileStream);

                    int length = (int)formatter.Deserialize(fileStream);
                    Player[] playersSave = new Player[length];
                    players = new List<Player>();
                    playersSave = (Player[])formatter.Deserialize(fileStream);
                    for (int i = 0; i < length; i++)
                    {
                        players.Add(playersSave[i]);
                    }
                    fileStream.Close();

                    for (int i = 0; i < players.Count; i++)
                    {
                        players[i].LeaveGameTable();
                    }
                    GameField.AnnulDecisionsOtherPlayersEvent -= AnnulDecisionsOfPlayers;
                    Player.AnnulDecisionOtherPlayersEvent -= AnnulDecisionsOfPlayers;
                    FinishRoundEvent -= field.FinishRound;
                    DeckHand.GiveCardEvent -= field.AddCard;
                    BotPlayer.CollectDataFieldEvent -= field.ShowField;
                    Player.AscCardsEvent -= dealer.GiveCard;

                    GameField.AnnulDecisionsOtherPlayersEvent += AnnulDecisionsOfPlayers;
                    Player.AnnulDecisionOtherPlayersEvent += AnnulDecisionsOfPlayers;
                    FinishRoundEvent += field.FinishRound;
                    BotPlayer.CollectDataFieldEvent += field.ShowField;
                    Player.AscCardsEvent += dealer.GiveCard;
                    DeckHand.GiveCardEvent += field.AddCard;
                    field.LoadCardsEvents();
                    ((DeckTable)dealer).LoadCardsEvents();
                    for (int i = 0; i < players.Count; i++)
                    {
                        players[i].LayCardEvent += players[i].Hand.GiveCard;
                        GameField.ReturnCardsEvent += players[i].Hand.TakeCard;
                        DeckTable.DealCardEvent += players[i].Hand.TakeCard;
                        PlayersMoveEvent += players[i].Move;
                        players[i].LoadSpecAndCardsEvents();
                    }
                    switch (numberOfPlayers)
                    {
                        case 2: MessageBox2.Visible = true; break;
                        case 3: MessageBox1.Visible = MessageBox3.Visible = true; break;
                        case 4: MessageBox1.Visible = MessageBox2.Visible = MessageBox3.Visible = true; break;
                    }
                    timer1.Enabled = true;
                }      
            }
        }
            
        private void SaveToMenu_Click(object sender, EventArgs e)
        {
            MakeSave();
            pictureBox1.Visible = false;
            label1.Visible = false;
            switch (numberOfPlayers)
            {
                case 2: MessageBox2.Visible = false; break;
                case 3: MessageBox1.Visible = MessageBox3.Visible = false; break;
                case 4: MessageBox1.Visible = MessageBox2.Visible = MessageBox3.Visible = false; break;
            }
            ToMenuButton.Visible = false;
            SaveToMenu.Visible = false;
            this.MouseMove -= CheckMouse;
            this.MouseClick -= MClick;
            this.Paint -= OnPaint;
            this.KeyUp -= ExitToMenu;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].LeaveGameTable();
            }
            players.Clear();
            GameField.AnnulDecisionsOtherPlayersEvent -= AnnulDecisionsOfPlayers;
            Player.AnnulDecisionOtherPlayersEvent -= AnnulDecisionsOfPlayers;
            FinishRoundEvent -= field.FinishRound;
            DeckHand.GiveCardEvent -= field.AddCard;
            BotPlayer.CollectDataFieldEvent -= field.ShowField;
            Player.AscCardsEvent -= dealer.GiveCard;
            this.Hide();
            menu.Show();
        }

        private void ToMenuButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            label1.Visible = false;
            switch (numberOfPlayers)
            {
                case 2: MessageBox2.Visible = false; break;
                case 3: MessageBox1.Visible = MessageBox3.Visible = false; break;
                case 4: MessageBox1.Visible = MessageBox2.Visible = MessageBox3.Visible = false; break;
            }
            ToMenuButton.Visible = false;
            SaveToMenu.Visible = false;
            this.MouseMove -= CheckMouse;
            this.MouseClick -= MClick;
            this.Paint -= OnPaint;
            this.KeyUp -= ExitToMenu;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].LeaveGameTable();
            }
            players.Clear();
            GameField.AnnulDecisionsOtherPlayersEvent -= AnnulDecisionsOfPlayers;
            Player.AnnulDecisionOtherPlayersEvent -= AnnulDecisionsOfPlayers;
            FinishRoundEvent -= field.FinishRound;
            DeckHand.GiveCardEvent -= field.AddCard;
            BotPlayer.CollectDataFieldEvent -= field.ShowField;
            Player.AscCardsEvent -= dealer.GiveCard;
            this.Hide();
            menu.Show();
        }
        // События игрока
        private void BeatButton_Click(object sender, EventArgs e)
        { 
            if (players[curPlayer] is Human && field.Line1 > 0)
            {
                players[curPlayer].Decide(DecisionOfPlayer.Beat);
                if ((curPlayer = (curPlayer + 1) % players.Count) == (mainPlayer + 1) % players.Count) curPlayer = (curPlayer + 1) % players.Count;
            }
        }
        private void TakeButton_Click(object sender, EventArgs e)
        {
            if (players[curPlayer] is Human)
            {
                players[curPlayer].Decide(DecisionOfPlayer.TakeCards);
                CheckFinish();
                if ((curPlayer = (curPlayer + 1) % players.Count) == (mainPlayer + 1) % players.Count) curPlayer = (curPlayer + 1) % players.Count;
            }
        }
        private void LetTakeButton_Click(object sender, EventArgs e)
        {
            if (players[curPlayer] is Human)
            {
                players[curPlayer].Decide(DecisionOfPlayer.LetTakeCards);
                if ((curPlayer = (curPlayer + 1) % players.Count) == (mainPlayer + 1) % players.Count) curPlayer = ++curPlayer % players.Count;
            }
        }

        private void MClick(object sender, MouseEventArgs e)
        {
            if (players[curPlayer] is Human)
            {
                MoveEventArgs message = new MoveEventArgs
                {
                    Trump = trumpSuit,
                    NumCardInHand = players[(mainPlayer + 1) % players.Count].Hand.NumberOfCards,
                    PlayerNum = players[curPlayer].Number
                };
                PlayersMoveEvent?.Invoke(this, message);
                CheckFinish();
                if (message.IsCompleted && players[curPlayer].Role == RoleOfPlayer.Defender)
                {
                    curPlayer = prePlayer;
                    prePlayer = (mainPlayer + 1) % players.Count;
                    return;
                }
                else
                {
                    if (message.IsCompleted && players[(mainPlayer + 1) % players.Count].Decision != DecisionOfPlayer.TakeCards && players[curPlayer].Role != RoleOfPlayer.Defender)
                    {
                        prePlayer = curPlayer;
                        curPlayer = (mainPlayer + 1) % players.Count;
                    }
                }
            }
        }

        private void CheckMouse(object sender, MouseEventArgs e)
        {
            if (players[curPlayer] is Human && !isWait)
            {
                ChooseEventArgs message = new ChooseEventArgs { Pointer = e.Location };
                ChooseCardEvent?.Invoke(players[curPlayer], message);
            }
        }

        //==================================================================
        #region nothing
        //private void DragCardToNewPlace(object sender, EventArgs e)
        //{
        //    #region useless
        //    //Bitmap fon1 = new Bitmap(Image.FromFile(@"D:\Программы Visual studio\FoolPROJECT\поле.png"));
        //    //Bitmap fon2 = new Bitmap(Image.FromFile(@"D:\Программы Visual studio\FoolPROJECT\поле.png"));
        //    //Graphics g1 = this.CreateGraphics();
        //    //Graphics g2 = Graphics.FromImage(fon2);
        //    #endregion
        //    maindeck.Draw(g2, true);
        //    field.Draw(g2);
        //    for (int i = 0; i < numberOfPlayers; i++)
        //    {
        //        players[i].Draw(g2);
        //    }
        //    g1.DrawImage(fon2, 0, 0);
        //    g2.DrawImage(fon1, 0, 0);
        //}
        #endregion
        
        // Функции игрового стола
        private void Play()
        {
            if (!(players[curPlayer] is Human))
            {
                MoveEventArgs message = new MoveEventArgs()
                {
                    PlayerNum = players[curPlayer].Number,
                    NumCardInHand = players[(mainPlayer + 1) % players.Count].Hand.NumberOfCards,
                    Decision = players[(mainPlayer + 1) % players.Count].Decision,
                    Trump = trumpSuit
                };
                PlayersMoveEvent?.Invoke(this, message);
                CheckFinish();
                if (curPlayer == (mainPlayer + 1) % players.Count)
                {
                    curPlayer = prePlayer;
                    prePlayer = (mainPlayer + 1) % players.Count;
                }
                else if (message.IsCompleted && players[(mainPlayer + 1) % players.Count].Decision != DecisionOfPlayer.TakeCards)
                {
                    if (players[(mainPlayer + 1) % players.Count] is Human)
                    {
                        prePlayer = curPlayer;
                        curPlayer = (mainPlayer + 1) % players.Count;
                        return;
                    } 
                    prePlayer = curPlayer;
                    curPlayer = (mainPlayer + 1) % players.Count;
                }
                if (players[curPlayer % players.Count].Decision != DecisionOfPlayer.Nothing && curPlayer != (mainPlayer + 1) % players.Count)
                {
                    if ((curPlayer = (curPlayer + 1) % players.Count) == (mainPlayer + 1) % players.Count) curPlayer = (curPlayer + 1) % players.Count;
                }
            }
        }

        // Обработчики событий игрового стола
        private void AnnulDecisionsOfPlayers(object sender, GameEventArgs e)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (i % players.Count != (mainPlayer + 1) % players.Count)
                    players[i % players.Count].Decide();
            }
        }

        private void GiveDataAboutPlayers(object sender, GameEventArgs e)
        {
            MoveEventArgs mes = new MoveEventArgs() { PlayerNum = e.PlayerNum };
            for (int i=0;i< players.Count; i++)
            {
                if (players[i].Number != e.PlayerNum)
                {
                    mes.OponentNum = players[i].Number;
                    mes.NumCardInHand = players[i].Hand.NumberOfCards;
                    GiveInfoAboutPlayers?.Invoke(this, mes);
                }
            }
        }

        //=====================================================
        private void CheckPassMove()
        {
            int countFin = 0;
            if (players[(mainPlayer + 1) % players.Count].Decision == DecisionOfPlayer.TakeCards)
            {
                for (int i = ((mainPlayer + 1) % players.Count) + 1; i < players.Count + (mainPlayer + 1) % players.Count; i++)
                {
                    if (players[i % players.Count].Decision == DecisionOfPlayer.LetTakeCards)
                        countFin++;
                }
            }
            if (players[(mainPlayer + 1) % players.Count].Decision == DecisionOfPlayer.Nothing)
            {
                for (int i = ((mainPlayer + 1) % players.Count) + 1; i < players.Count + (mainPlayer + 1) % players.Count; i++)
                {
                    if (players[i % players.Count].Decision == DecisionOfPlayer.Beat)
                        countFin++;
                }
            }
            if (countFin == players.Count - 1)
            {
                MoveEventArgs message = new MoveEventArgs()
                {
                    Decision = players[(mainPlayer + 1) % players.Count].Decision,
                    PlayerNum = players[(mainPlayer + 1) % players.Count].Number,
                    Trump = trumpSuit
                };
                Thread.Sleep(450);
                FinishRoundEvent?.Invoke(players[(mainPlayer + 1) % players.Count], message);
                if (dealer.NumberOfCards > 0)
                    ToDealCards();
                ChangeRolesAndPlayers();
            }
        }

        //private void Useless()
        //{
        //    //if (dealer.NumberOfCards == 0)
        //    //{
        //    //    for (int i = curPlayer, j = players.Count; j > 0; j--)
        //    //    {
        //    //        if (players[i % players.Count].Hand.NumberOfCards == 0)
        //    //        {
        //    //            if (i % players.Count != prePlayer)
        //    //            {
        //    //                OutputLastWords(i % players.Count);
        //    //                if (i % players.Count < prePlayer)
        //    //                {
        //    //                    prePlayer--;
        //    //                    players[i % players.Count].LeaveGameTable();
        //    //                    players.RemoveAt(i % players.Count);
        //    //                    //i = curPlayer = (curPlayer + 1 % players.Count) ;
        //    //                    i = curPlayer % players.Count == prePlayer ? curPlayer = (curPlayer + 1) % players.Count  : curPlayer %= players.Count;
        //    //                }
        //    //                else
        //    //                {
        //    //                    players[i % players.Count].LeaveGameTable();
        //    //                    players.RemoveAt(i % players.Count);
        //    //                    //i = curPlayer %= players.Count;
        //    //                    i = curPlayer % players.Count == prePlayer ? curPlayer = (curPlayer + 1) % players.Count : curPlayer %= players.Count;
        //    //                } 
        //    //            }
        //    //            else
        //    //            {
        //    //                if (players[prePlayer].Hand.NumberOfCards == 0)
        //    //                {
        //    //                    OutputLastWords(prePlayer);
        //    //                    players[prePlayer].LeaveGameTable();
        //    //                    players.RemoveAt(prePlayer);
        //    //                    //i = curPlayer %= players.Count;
        //    //                    i = curPlayer % players.Count == prePlayer ? curPlayer = (curPlayer + 1) % players.Count : curPlayer %= players.Count;
        //    //                    ChangeRolesAndPlayers();
        //    //                    MoveEventArgs message = new MoveEventArgs()
        //    //                    {
        //    //                        Decision = players[prePlayer].Decision,
        //    //                        PlayerNum = players[prePlayer].Number,
        //    //                        Trump = trumpSuit
        //    //                    };
        //    //                    Thread.Sleep(450);
        //    //                    FinishRoundEvent?.Invoke(players[prePlayer], message);
        //    //                    return;
        //    //                }
        //    //            } 
        //    //        }
        //    //        else
        //    //            i++;
        //    //    }
        //    //}
        //}

        private void CheckFinish()
        {
            if (dealer.NumberOfCards == 0)
            {
                if (players[curPlayer].Hand.NumberOfCards == 0)
                {
                    if (players[curPlayer] is Human)
                    {
                        if (numberOfPlayers - players.Count == 0)
                        {
                            timer1.Enabled = false;
                            pictureBox1.Visible = true;
                            pictureBox1.BackgroundImage = Properties.Resources.Win;
                            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                            ToMenuButton.Visible = true;
                        }
                        else if (players.Count > 1)
                        {
                            timer1.Enabled = false;
                            pictureBox1.Visible = true;
                            pictureBox1.BackgroundImage = Properties.Resources.Out;
                            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                            ToMenuButton.Visible = true;
                        }
                        else if (players.Count == 1)
                        {
                            if (field.Line1 - field.Line2 > players[curPlayer].Hand.NumberOfCards || field.Line1 - field.Line2 == players[curPlayer].Hand.NumberOfCards && players[curPlayer].Decision == DecisionOfPlayer.TakeCards)
                            {
                                timer1.Enabled = false;
                                pictureBox1.Visible = true;
                                pictureBox1.BackgroundImage = Properties.Resources.Lose;
                                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                                ToMenuButton.Visible = true;
                            }
                            
                        }
                    }
                    if (curPlayer > mainPlayer && curPlayer != (mainPlayer + 1) % players.Count)
                    {
                        OutputLastWords(curPlayer);
                        players[curPlayer].LeaveGameTable();
                        players.RemoveAt(curPlayer);
                        curPlayer = (curPlayer + 1) % players.Count;
                    }
                    else if (curPlayer < mainPlayer && curPlayer != (mainPlayer + 1) % players.Count)
                    {
                        OutputLastWords(curPlayer);
                        players[curPlayer].LeaveGameTable();
                        players.RemoveAt(curPlayer);
                        curPlayer = (curPlayer + 1) % players.Count;
                        mainPlayer--;
                    }
                    else if (curPlayer == (mainPlayer + 1) % players.Count)
                    {
                        OutputLastWords(curPlayer);
                        players[curPlayer].LeaveGameTable();
                        players.RemoveAt(curPlayer);
                        ChangeRolesAndPlayers();
                        MoveEventArgs message = new MoveEventArgs()
                        {
                            Decision = players[(mainPlayer + 1) % players.Count].Decision,
                            PlayerNum = players[(mainPlayer + 1) % players.Count].Number,
                            Trump = trumpSuit
                        };
                        Thread.Sleep(450);
                        FinishRoundEvent?.Invoke(players[(mainPlayer + 1) % players.Count], message);
                        return;
                    }
                }
            }
        }

        private void ToDealCards()
        {
            for (int i = mainPlayer; i < mainPlayer + players.Count && dealer.NumberOfCards > 0; i++)
            {
                if (i % players.Count != (mainPlayer + 1) % players.Count)
                    players[i % players.Count].IsNeedCards();
            }
            if (dealer.NumberOfCards > 0)
                players[(mainPlayer + 1) % players.Count].IsNeedCards();
        }

        private void ChangeRolesAndPlayers()
        {
            if (players[(mainPlayer + 1) % players.Count].Decision == DecisionOfPlayer.TakeCards)
                mainPlayer = ((mainPlayer + 1) % players.Count + 1) % players.Count;
            else
                mainPlayer = (mainPlayer + 1) % players.Count;
            curPlayer = mainPlayer;
            prePlayer = (mainPlayer + 1) % players.Count;
            for (int i = 0; i < players.Count; i++)
            {
                if (i == (mainPlayer + 1) % players.Count)
                    players[i].Role = RoleOfPlayer.Defender;
                else
                    players[i].Role = RoleOfPlayer.Attacker;
                players[i].Decide();
            }
        }

        private void WhoIsFirst()
        {
            int min = 9;
            for (int findMin, i = 0; i < players.Count; i++)
            {
                if ((findMin = players[i].FindMinRankOfTrumpSuit(trumpSuit)) < min)
                {
                    min = findMin;
                    mainPlayer = i;
                }
            }
            if (min == 9)
                mainPlayer = rand.Next(players.Count);
            curPlayer = mainPlayer;
            prePlayer = (mainPlayer + 1) % players.Count;
            players[prePlayer].Role = RoleOfPlayer.Defender;
            for (int i = curPlayer; i < players.Count + curPlayer; i++)
            {
                if (i % players.Count != prePlayer)
                    players[i % players.Count].Role = RoleOfPlayer.Attacker;
            }
        }

        private void ShowButton()
        {
            if (players[curPlayer] is Human)
            {
                if (players[curPlayer].Role != RoleOfPlayer.Defender && players[curPlayer].Decision == DecisionOfPlayer.Nothing)
                {
                    if (players[(mainPlayer + 1) % players.Count].Decision == DecisionOfPlayer.TakeCards)
                    {
                        BeatButton.Enabled = false;
                        BeatButton.Visible = false;
                        LetTakeButton.Enabled = true;
                        LetTakeButton.Visible = true;
                    }
                    else
                    {
                        LetTakeButton.Enabled = false;
                        LetTakeButton.Visible = false;
                        BeatButton.Enabled = true;
                        BeatButton.Visible = true;
                    }
                }
                if (players[curPlayer].Role == RoleOfPlayer.Defender && players[prePlayer].Decision == DecisionOfPlayer.Nothing)
                {
                    BeatButton.Enabled = false;
                    BeatButton.Visible = false;
                    LetTakeButton.Enabled = false;
                    LetTakeButton.Visible = false;
                    TakeButton.Enabled = true;
                    TakeButton.Visible = true;
                }
            }
            else
            {
                LetTakeButton.Enabled = false;
                LetTakeButton.Visible = false;
                BeatButton.Enabled = false;
                BeatButton.Visible = false;
                TakeButton.Enabled = false;
                TakeButton.Visible = false;
            }
        }

        private void ShowTip()
        {
            if (players[0].Role == RoleOfPlayer.Attacker)
            {
                if (field.Line1 == 0)
                {
                    label1.Text = "Подкиньте любую карту";
                }
                else
                {
                    label1.Text = "Подкидывайте карты того же достоинства, что и на столе";
                }
            }
            if (players[0].Role == RoleOfPlayer.Defender)
            {
                if (players[0].Decision == DecisionOfPlayer.Nothing)
                {
                    label1.Text = "Отбивайтесь!";
                }
            }
        }

        private void OutputMessages()
        {
            string message = " ";
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] is BotPlayer)
                {
                    if (players[i].Number == players[curPlayer].Number)
                    {
                        message = "Я хожусь";
                    }
                    if (players[i].Role == RoleOfPlayer.Defender)
                    {
                        switch (players[i].Decision)
                        {
                            case DecisionOfPlayer.Nothing: message = "Я кроюсь"; break;
                            case DecisionOfPlayer.TakeCards: message = "Беру"; break;
                        }
                    }
                    if (players[i].Role == RoleOfPlayer.Attacker)
                    {
                        switch (players[i].Decision)
                        {
                            case DecisionOfPlayer.Beat: message = "Бито"; break;
                            case DecisionOfPlayer.LetTakeCards: message = "Пусть берёт"; break;
                        }
                    }
                    switch (numberOfPlayers)
                    {
                        case 2: MessageBox2.Text = message; break;
                        case 3:
                            {
                                if (players[i].Number == 1) 
                                    MessageBox1.Text = message; 
                                else 
                                    MessageBox3.Text = message; 
                            }
                            break;
                        case 4:
                            {
                                if (players[i].Number == 1)
                                    MessageBox1.Text = message;
                                else if (players[i].Number == 2)
                                    MessageBox2.Text = message;
                                else
                                    MessageBox3.Text = message;
                            }
                            break;
                    }
                    message = " ";
                }
            }
        }

        private void OutputLastWords(int playerNum)
        {
            string message = " ";
            if (numberOfPlayers - players.Count == 0)
                message = "Я выиграл";
            else if (numberOfPlayers - players.Count > 0)
                message = "Я вышел";
            switch (numberOfPlayers)
            {
                case 2: MessageBox2.Text = message; break;
                case 3:
                    {
                        if (players[playerNum].Number == 1)
                            MessageBox1.Text = message;
                        else
                            MessageBox3.Text = message;
                    }
                    break;
                case 4:
                    {
                        if (players[playerNum].Number == 1)
                            MessageBox1.Text = message;
                        else if (players[playerNum].Number == 2)
                            MessageBox2.Text = message;
                        else
                            MessageBox3.Text = message;
                    }
                    break;
            }
        }
        //Меню
        public void StartGame()
        {
            pictureBox1.Visible = false;
            label1.Visible = true;
            Init();
            ToDealCards();
            WhoIsFirst();
        }

        public void SetNumberOfPlayers(int num, int index)
        {
            numberOfPlayers = num;
            players.Add(new Human(index, numberOfPlayers));
            //pictureBox1.BackgroundImage = Properties.Resources.chooseplayers;
        }

        public void AddPlayer(int type, int index)
        {
            switch (type)
            {
                case 1:
                    {
                        players.Add(new Beginner(index, numberOfPlayers));
                    }
                    break;
                case 2:
                    {
                        players.Add(new Amateur(index, numberOfPlayers));
                    }
                    break;
                case 3:
                    {
                        players.Add(new Profi(index, numberOfPlayers));
                    }
                    break;
            }
        }

        private void GameTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void RemovePlayer(ref int index, int type)
        {
            for (int i = players.Count - 1; i >= 0; i--)
            {
                switch (type)
                {
                    case 1:
                        {
                            if (players[i] is Beginner)
                            {
                                CheckDeletePlayer(ref i);
                                //players.RemoveAt(i);
                                index = i;
                                return ;
                            }
                        }
                        break;
                    case 2:
                        {
                            if (players[i] is Amateur)
                            {
                                CheckDeletePlayer(ref i);
                                //players.RemoveAt(i);
                                index = i;
                                return  ;
                            }
                        }
                        break;
                    case 3:
                        {
                            if (players[i] is Profi)
                            {
                                CheckDeletePlayer(ref i);
                                //players.RemoveAt(i);
                                index = i;
                                return ;
                            }
                        }
                        break;
                }
            }
        }

        private void CheckDeletePlayer(ref int index)
        {
            if (index + 1 == players.Count)
            {
                players[index].LeaveGameTable();
                players.RemoveAt(index);
            }
            else
            {
                players[index].LeaveGameTable();
                players.RemoveAt(index);
                for (;index < players.Count;index++)
                {
                    if(players[index] is Beginner)
                    {
                        players[index].LeaveGameTable();
                        players[index] = new Beginner(index, numberOfPlayers);
                    }
                    if (players[index] is Amateur)
                    {
                        players[index].LeaveGameTable();
                        players[index] = new Amateur(index, numberOfPlayers);
                    }
                    if (players[index] is Profi)
                    {
                        players[index].LeaveGameTable();
                        players[index] = new Profi(index, numberOfPlayers);
                    }
                }
            }
        }

        public void Back()
        {
            numberOfPlayers = 0;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].LeaveGameTable();
            }
            players.Clear();
            //pictureBox1.BackgroundImage = Properties.Resources.menu;
        }
        //====================================== 
    }
}
