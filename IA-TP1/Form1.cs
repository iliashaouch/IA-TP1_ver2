using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IA_TP1
{
    public partial class Form1 : Form
    {
        private room[,] exemple;
        public Form1()
        {
            InitializeComponent();
            Random rand = new Random();
            exemple = new room[5, 5];
            for (int i = 0; i < exemple.GetLength(0); i++)
            {
                for (int j = 0; j < exemple.GetLength(1); j++)
                {
                    exemple[i, j].hasDirt = rand.Next() % 2 == 0;
                    exemple[i, j].hasBijoux = rand.Next() % 3 == 0;
                }
            }
            test2(exemple);
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void test()
        {
            Robot robot = new Robot();
            int[] p = { 0, 0 };
            robot.Position = p;
            room[,] exemple = new room[2, 2];
            exemple[0, 0].hasDirt = false;
            exemple[0, 0].hasBijoux = false;
            exemple[1, 0].hasDirt = false;
            exemple[1, 0].hasBijoux = false;
            exemple[0, 1].hasDirt = false;
            exemple[0, 1].hasBijoux = false;
            exemple[1, 0].hasDirt = false;
            exemple[1, 0].hasBijoux = false;
            exemple[1, 1].hasDirt = true;
            exemple[1, 1].hasBijoux = false;
            robot.Memoire = exemple;
            Console.WriteLine(robot.search());
            string verif;
            verif = Console.ReadLine();    //Console attend enter avant de fermer 
        }

        private void test2(room[,] rooms)
        {
            int k = 0;
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if (rooms[i, j].hasDirt && rooms[i, j].hasBijoux) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Black;
                    else if (rooms[i, j].hasBijoux) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Blue;
                    else if (rooms[i, j].hasDirt) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Red;
                    else tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
                }
            }
        }

        public void boucleTest()
        {
            long lastDate = DateTime.Now.Ticks;

            while (true)
            {
                if (DateTime.Now.Ticks - lastDate > 2 * TimeSpan.TicksPerSecond)
                {
                    miseAJourR(exemple);
                }
                if (DateTime.Now.Ticks - lastDate > 1 * TimeSpan.TicksPerSecond)
                {
                    test2(exemple);
                }
                lastDate = DateTime.Now.Ticks;
            }
        }

        private void miseAJourR(room[,] rooms)
        {
            Random rand = new Random();
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if(rooms[i, j].hasDirt = rand.Next() % 2 == 0) rooms[i,j].hasDirt=!rooms[i,j].hasDirt;
                    if(rooms[i, j].hasBijoux = rand.Next() % 3 == 0) rooms[i,j].hasBijoux=!rooms[i,j].hasBijoux;
                }
            }
        }
    }
}
