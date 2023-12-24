using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Durak__Fool_
{
    partial class GameTable
    {
        
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameTable));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LetTakeButton = new System.Windows.Forms.Button();
            this.TakeButton = new System.Windows.Forms.Button();
            this.BeatButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MessageBox1 = new System.Windows.Forms.Label();
            this.MessageBox2 = new System.Windows.Forms.Label();
            this.MessageBox3 = new System.Windows.Forms.Label();
            this.ToMenuButton = new System.Windows.Forms.Button();
            this.SaveToMenu = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.Tick);
            // 
            // LetTakeButton
            // 
            this.LetTakeButton.BackColor = System.Drawing.Color.Gold;
            resources.ApplyResources(this.LetTakeButton, "LetTakeButton");
            this.LetTakeButton.Name = "LetTakeButton";
            this.LetTakeButton.UseVisualStyleBackColor = false;
            this.LetTakeButton.Click += new System.EventHandler(this.LetTakeButton_Click);
            // 
            // TakeButton
            // 
            this.TakeButton.BackColor = System.Drawing.Color.Gold;
            resources.ApplyResources(this.TakeButton, "TakeButton");
            this.TakeButton.Name = "TakeButton";
            this.TakeButton.UseVisualStyleBackColor = false;
            this.TakeButton.UseWaitCursor = true;
            this.TakeButton.Click += new System.EventHandler(this.TakeButton_Click);
            // 
            // BeatButton
            // 
            this.BeatButton.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.BeatButton.BackColor = System.Drawing.Color.Gold;
            resources.ApplyResources(this.BeatButton, "BeatButton");
            this.BeatButton.Name = "BeatButton";
            this.BeatButton.TabStop = false;
            this.BeatButton.UseVisualStyleBackColor = false;
            this.BeatButton.Click += new System.EventHandler(this.BeatButton_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Name = "label1";
            // 
            // MessageBox1
            // 
            resources.ApplyResources(this.MessageBox1, "MessageBox1");
            this.MessageBox1.Name = "MessageBox1";
            // 
            // MessageBox2
            // 
            resources.ApplyResources(this.MessageBox2, "MessageBox2");
            this.MessageBox2.Name = "MessageBox2";
            // 
            // MessageBox3
            // 
            resources.ApplyResources(this.MessageBox3, "MessageBox3");
            this.MessageBox3.Name = "MessageBox3";
            // 
            // ToMenuButton
            // 
            this.ToMenuButton.BackColor = System.Drawing.Color.Gold;
            resources.ApplyResources(this.ToMenuButton, "ToMenuButton");
            this.ToMenuButton.Name = "ToMenuButton";
            this.ToMenuButton.UseVisualStyleBackColor = false;
            this.ToMenuButton.Click += new System.EventHandler(this.ToMenuButton_Click);
            // 
            // SaveToMenu
            // 
            this.SaveToMenu.BackColor = System.Drawing.Color.Gold;
            resources.ApplyResources(this.SaveToMenu, "SaveToMenu");
            this.SaveToMenu.Name = "SaveToMenu";
            this.SaveToMenu.UseVisualStyleBackColor = false;
            this.SaveToMenu.Click += new System.EventHandler(this.SaveToMenu_Click);
            // 
            // GameTable
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dial;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SaveToMenu);
            this.Controls.Add(this.ToMenuButton);
            this.Controls.Add(this.MessageBox3);
            this.Controls.Add(this.MessageBox2);
            this.Controls.Add(this.MessageBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.BeatButton);
            this.Controls.Add(this.TakeButton);
            this.Controls.Add(this.LetTakeButton);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GameTable";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameTable_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Timer timer1;
        private Button LetTakeButton;
        private Button TakeButton;
        private Button BeatButton;
        private PictureBox pictureBox1;
        private Label label1;
        private Label MessageBox1;
        private Label MessageBox2;
        private Label MessageBox3;
        private Button ToMenuButton;
        private Button SaveToMenu;
    }
}

