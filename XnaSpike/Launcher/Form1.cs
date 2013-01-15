using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Launcher
{
    public partial class Form1 : Form
    {
        class LevelItem
        {
            public string DisplayName { get; set; }
            public int LevelIndex { get; set; }

            public override string ToString()
            {
                return this.DisplayName;
            }
        }

        public Form1()
        {
            InitializeComponent();
            InitializeDropDown();
        }

        private void InitializeDropDown()
        {
            this.comboBox1.Items.Add(new LevelItem() { DisplayName = "Level 1", LevelIndex = 1 } );
            this.comboBox1.SelectedIndex = 0;
        }

        private int GetSelectedLevel()
        {
            LevelItem item = this.comboBox1.SelectedItem as LevelItem;
            if (item != null)
            {
                return item.LevelIndex;
            }

            return 1;
        }

        private void buttonSinglePlayer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("game.exe", "-m single -l " + this.GetSelectedLevel());
        }

        private void buttonAlgorithm1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("game.exe", "-m algo1 -l " + this.GetSelectedLevel());
        }

        private void buttonCredits_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("game.exe", "-m credits");
        }
    }
}
