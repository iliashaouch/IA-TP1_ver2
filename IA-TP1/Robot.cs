using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP1
{

    class Robot
    {
        private int[] pos = new int[2];
        private Manoire memoire;
        private capteur c;
        private effecteur e;
        private int conso;
    }
    class capteur
    {
        public room[,] captedrooms = new room[5, 5];
        public void a(Manoire manoire)
        {
            captedrooms = manoire.getState();
        }

    }

    class effecteur
    {
        public void aspirer(int[] pos)
        {

        }

        public void ramasser(int[] pos)
        {

        }

        public void deplacer(int[] pos)
        {

        }
    }
}
