using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace IA_TP1
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            ThreadStart childref1 = new ThreadStart(ManoireThread);
            Console.WriteLine("In Main: Creating the manoire's thread");

            Thread manoire = new Thread(childref1);
            manoire.Start();

            ThreadStart childref2 = new ThreadStart(RobotThread);
            Console.WriteLine("In Main: Creating the robot's thread");

            Thread robot = new Thread(childref2);
            robot.Start();

        }


        public static void ManoireThread()
        {
            Manoire manoire = new Manoire();
            long lastDate = DateTime.Now.Ticks;

            while (true)
            {
                if (DateTime.Now.Ticks - lastDate > TimeSpan.TicksPerSecond)
                {
                    manoire.createObject();
                    lastDate = DateTime.Now.Ticks;
                }
            }
        }

        public static void RobotThread()
        {
            Robot robot = new Robot();
            while (true)
            {

            }
        }
    }
}
