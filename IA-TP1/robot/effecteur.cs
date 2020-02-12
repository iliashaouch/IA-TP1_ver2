using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IA_TP1.manoir;

namespace IA_TP1.robot
{

    class effecteur
    {
        private Manoire m_manoir;

        public effecteur(Manoire manoir)
        {
            m_manoir = manoir;
        }

        // la fonction "aspirer" appelle la fonction "cleanRoom" du manoir à la position entrée. Cette fonction du manoir retire les saleté et bijoux de la salle indiquée 
        public void aspirer(int[] pos)
        {
            m_manoir.cleanRoom(pos);
        }

        // la fonction "ramasser" appelle la fonction "cleanRoom" du manoir à la position entrée. Cette fonction du manoir retire les bijoux de la salle indiquée 
        public void ramasser(int[] pos)
        {
            m_manoir.ramassageBijoux(pos);
        }

        // la fonction "deplacer" prend en paramètre une action (bas, haut, gauche ou droite) et change la position du robot 
        public int[] deplacer(char action, int[] pos)
        {
            switch (action)
            {
                case 'b':
                    pos[0] += 1;
                    break;
                case 'h':
                    pos[0] -= 1;
                    break;
                case 'g':
                    pos[1] -= 1;
                    break;
                case 'd':
                    pos[1] += 1;
                    break;
            }
            return pos;
        }
    }
}
