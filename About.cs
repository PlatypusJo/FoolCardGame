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
    public partial class About : Form
    {
        MainMenu menu;

        public About(MainMenu mainMenu)
        {
            InitializeComponent();
            menu = mainMenu;
            this.BackgroundImage = Properties.Resources.gameTableAbout;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menu.Show();
            Close();
        }
    }
}
