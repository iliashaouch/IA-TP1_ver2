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
        private int[] posRob = new int[2];
        public Form1()
        {
            InitializeComponent();

            Program.robot.Position.CopyTo(posRob,0);
            printRobot();
            SetSalle(Program.theManoire.getState());

            Timer timer = new Timer();
            timer.Interval = (1 * 100);
            timer.Tick += new EventHandler(autoRefresh);
            timer.Start();
        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void test()
        {
            Robot robot = new Robot(new Manoire());
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

        private void SetSalle(room[,] rooms)
        {
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if (rooms[i, j].hasDirt && rooms[i, j].hasBijoux) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.DarkViolet;
                    else if (rooms[i, j].hasBijoux) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Blue;
                    else if (rooms[i, j].hasDirt) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Red;
                    else tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
                }
            }
        }

        private void printRobot()
        {
            tableLayoutPanel1.GetControlFromPosition(posRob[0], posRob[1]).Text = "";
            Program.robot.Position.CopyTo(posRob, 0);
            tableLayoutPanel1.GetControlFromPosition(posRob[0], posRob[1]).Text = "X";
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            SetSalle(Program.theManoire.getState());
            printRobot();
        }

        private void autoRefresh(object sender, EventArgs e)
        {
            SetSalle(Program.theManoire.getState());
            printRobot();
        }
    }
}
