using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IA_TP1.manoir;

namespace IA_TP1.robot
{
    class capteur
    {
        private Manoire m_manoir;

        public capteur(Manoire manoir)
        {
            m_manoir = manoir;
        }

        // La fonction "captureEnv" rend l'état actuel du manoir
        public room[,] captureEnv()
        {
            return m_manoir.getState();
        }
    }
}
