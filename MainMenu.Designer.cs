
namespace Durak__Fool_
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.TwoPlayersOrBeginner = new System.Windows.Forms.Button();
            this.ThreePlayersOrAmateur = new System.Windows.Forms.Button();
            this.FourPlayersOrMaster = new System.Windows.Forms.Button();
            this.LoadOrBack = new System.Windows.Forms.Button();
            this.About = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TwoPlayersOrBeginner
            // 
            this.TwoPlayersOrBeginner.BackColor = System.Drawing.Color.Gold;
            this.TwoPlayersOrBeginner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TwoPlayersOrBeginner.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TwoPlayersOrBeginner.Location = new System.Drawing.Point(936, 346);
            this.TwoPlayersOrBeginner.Name = "TwoPlayersOrBeginner";
            this.TwoPlayersOrBeginner.Size = new System.Drawing.Size(310, 58);
            this.TwoPlayersOrBeginner.TabIndex = 0;
            this.TwoPlayersOrBeginner.Text = "Игра вдвоём";
            this.TwoPlayersOrBeginner.UseVisualStyleBackColor = false;
            this.TwoPlayersOrBeginner.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TwoPlayersOrBeginner_MouseUp);
            // 
            // ThreePlayersOrAmateur
            // 
            this.ThreePlayersOrAmateur.BackColor = System.Drawing.Color.Gold;
            this.ThreePlayersOrAmateur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ThreePlayersOrAmateur.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ThreePlayersOrAmateur.Location = new System.Drawing.Point(936, 436);
            this.ThreePlayersOrAmateur.Name = "ThreePlayersOrAmateur";
            this.ThreePlayersOrAmateur.Size = new System.Drawing.Size(310, 58);
            this.ThreePlayersOrAmateur.TabIndex = 1;
            this.ThreePlayersOrAmateur.Text = "Игра втроём";
            this.ThreePlayersOrAmateur.UseVisualStyleBackColor = false;
            this.ThreePlayersOrAmateur.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ThreePlayersOrAmateur_MouseUp);
            // 
            // FourPlayersOrMaster
            // 
            this.FourPlayersOrMaster.BackColor = System.Drawing.Color.Gold;
            this.FourPlayersOrMaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FourPlayersOrMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FourPlayersOrMaster.Location = new System.Drawing.Point(936, 532);
            this.FourPlayersOrMaster.Name = "FourPlayersOrMaster";
            this.FourPlayersOrMaster.Size = new System.Drawing.Size(310, 58);
            this.FourPlayersOrMaster.TabIndex = 2;
            this.FourPlayersOrMaster.Text = "Игра вчетвером";
            this.FourPlayersOrMaster.UseVisualStyleBackColor = false;
            this.FourPlayersOrMaster.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FourPlayersOrMaster_MouseUp);
            // 
            // LoadOrBack
            // 
            this.LoadOrBack.BackColor = System.Drawing.Color.Gold;
            this.LoadOrBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadOrBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LoadOrBack.Location = new System.Drawing.Point(936, 722);
            this.LoadOrBack.Name = "LoadOrBack";
            this.LoadOrBack.Size = new System.Drawing.Size(310, 58);
            this.LoadOrBack.TabIndex = 3;
            this.LoadOrBack.Text = "Загрузить игру";
            this.LoadOrBack.UseVisualStyleBackColor = false;
            this.LoadOrBack.Click += new System.EventHandler(this.LoadOrBack_Click);
            // 
            // About
            // 
            this.About.BackColor = System.Drawing.Color.Gold;
            this.About.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.About.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.About.Location = new System.Drawing.Point(936, 633);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(310, 58);
            this.About.TabIndex = 4;
            this.About.Text = "О программе";
            this.About.UseVisualStyleBackColor = false;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // Exit
            // 
            this.Exit.BackColor = System.Drawing.Color.Gold;
            this.Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Exit.Location = new System.Drawing.Point(936, 814);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(310, 58);
            this.Exit.TabIndex = 5;
            this.Exit.Text = "Выйти";
            this.Exit.UseVisualStyleBackColor = false;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1922, 1053);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.About);
            this.Controls.Add(this.LoadOrBack);
            this.Controls.Add(this.FourPlayersOrMaster);
            this.Controls.Add(this.ThreePlayersOrAmateur);
            this.Controls.Add(this.TwoPlayersOrBeginner);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.Text = "Дурак подкидной";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TwoPlayersOrBeginner;
        private System.Windows.Forms.Button ThreePlayersOrAmateur;
        private System.Windows.Forms.Button FourPlayersOrMaster;
        private System.Windows.Forms.Button LoadOrBack;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.Button Exit;
    }
}