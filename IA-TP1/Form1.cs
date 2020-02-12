using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IA_TP1.robot;
using IA_TP1.manoir;

namespace IA_TP1
{
    public partial class Form1 : Form
    {
        private int[] posRob = new int[2]; //memoire de la position du robot
                                           //afin d'effacer la derniere position du robot
        public Form1()
        {
            InitializeComponent(); //creation du Form (auto-genere)

            //affichage de la situation initiale
            Program.robot.Position.CopyTo(posRob, 0);
            printRobot();
            SetSalle(Program.theManoire.getState());

            //creation de la clock pour auto-refresh
            Timer timer = new Timer
            {
                Interval = (int)(0.1 * 1000) //refresh toutes les 0.1 sec
            };
            timer.Tick += new EventHandler(autoRefresh);
            timer.Start();
        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// modifie l'affichage des salles selon leur etat
        /// </summary>
        /// <param name="rooms">matrice des sallles du manoire</param>
        private void SetSalle(room[,] rooms)
        {
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if (rooms[i, j].hasDirt && rooms[i, j].hasBijoux) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.DarkViolet; //bijoux+poussiere
                    else if (rooms[i, j].hasBijoux) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Blue; //bijoux
                    else if (rooms[i, j].hasDirt) tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.Red; //poussiere
                    else tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White; //vide
                }
            }
        }

        /// <summary>
        /// modifie l'affichage du robot
        /// </summary>
        private void printRobot()
        {
            tableLayoutPanel1.GetControlFromPosition(posRob[0], posRob[1]).Text = ""; //efface la derniere position connue du Robot
            Program.robot.Position.CopyTo(posRob, 0); //recupere la nouvelle position
            tableLayoutPanel1.GetControlFromPosition(posRob[0], posRob[1]).Text = "X"; //print
        }

        /// <summary>
        /// modifie l'affichage du nombre de bijoux aspires par accidents
        /// </summary>
        private void printNbBijoux()
        {
            string[] s = countBijoux.Text.Split('\n');
            string[] s2 = s[1].Split(' ');
            countBijoux.Text = s[0] + "\n" + Program.theManoire.NbBijouxAspires.ToString() + " " + s2[1] + " " + s2[2];
        }

        /// <summary>
        /// auto refresh via le Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoRefresh(object sender, EventArgs e)
        {
            SetSalle(Program.theManoire.getState());
            printRobot();
            printNbBijoux();
        }
    }
}
