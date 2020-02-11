using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace IA_TP1
{
    static class Program
    {
        public static Manoire theManoire = new Manoire();
        public static Robot robot = new Robot(theManoire);
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string robMode = "";
            Console.WriteLine("Indiquez le type du robot, BFS (0) ou AStar (1) : ");
            robMode = Console.ReadLine();
            if(robMode == "0") { Program.robot.setMode(false); }
            else { Program.robot.setMode(true); }
            //creation du Thread pour le manoire
            ThreadStart childref1 = new ThreadStart(ManoireThread);
            Thread manoire = new Thread(childref1);
            Console.WriteLine("In Main: Creating the manoire's thread");
            manoire.Start();

            //creation du Thread pour le Robot
            ThreadStart childref2 = new ThreadStart(RobotThread);
            Thread robot = new Thread(childref2);
            Console.WriteLine("In Main: Creating the robot's thread");
            robot.Start();

            //Creation de l'UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// methode de creation du Thread Manoire
        /// </summary>
        public static void ManoireThread()
        {
            long lastDate = DateTime.Now.Ticks; //recupere le temps
            while (true)
            {
                if (DateTime.Now.Ticks - lastDate > 1 * TimeSpan.TicksPerSecond) //si plus de 1 sec depuis derniere boucle
                {
                    theManoire.createObject(); //ajjout d'une poussiere ou d'un bijou
                    lastDate = DateTime.Now.Ticks;
                }
            }
        }

        /// <summary>
        /// methode de creation du Thread Robot
        /// </summary>
        public static void RobotThread()
        {
            robot.startLifeCycle(); //demare le Robot
        }
    }
}
