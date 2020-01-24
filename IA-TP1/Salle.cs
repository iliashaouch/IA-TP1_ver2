using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP1
{
    public struct room
    {
        public bool hasDirt;
        public bool hasBijoux;
    }

    class Salle
    {
        private bool dirt = false;
        private bool bijoux = false;

        public Salle()
        {

        }

        public bool getDirt
        {
            get => dirt;
        }
        public bool getBijoux
        {
            get => bijoux;
        }
        public void setDirt(bool val)
        {
            dirt = val;
        }
        public void setBijoux(bool val)
        {
            bijoux = val;
        }
        override public string ToString()
        {
            return "bijoux: " + bijoux + ", dirt: " + dirt;
        }
    }
    class Manoire
    {
        private Salle[,] salles = new Salle[5, 5];

        public Manoire()
        {
            for (int i = 0; i < salles.GetLength(0); i++)
            {
                for (int j = 0; j < salles.GetLength(1); j++) salles[i, j] = new Salle();
            }

        }

        internal Salle[,] Salles { get => salles; set => salles = value; }

        public void createObject()
        {
            Random rand = new Random();

            int nrand = rand.Next(0, 10);
            int[] s = { rand.Next(0, salles.GetLength(0)), rand.Next(0, salles.GetLength(1)) };
            if (nrand == 1)
            {
                salles[s[0], s[1]].setBijoux(true);
            }
            else if (nrand % 2 == 0)
            {
                salles[s[0], s[1]].setDirt(true);
            }
        }

        public void cleanRoom(int[] s)
        {
            salles[s[0], s[1]].setBijoux(false);
            salles[s[0], s[1]].setDirt(false);
        }

        public void ramassageBijoux(int[] s)
        {
            salles[s[0], s[1]].setBijoux(false);
        }

        public override string ToString()
        {
            string retour = "";
            for (int i = 0; i < salles.GetLength(0); i++)
            {
                for (int j = 0; j < salles.GetLength(1); j++)
                {
                    retour += salles[i, j] + "|";
                }
                retour += "\n";
            }
            return retour;
        }

        public room[,] getState()
        {
            room[,] rooms = new room[5, 5];
            for (int i = 0; i < salles.GetLength(0); i++)
            {
                for (int j = 0; j < salles.GetLength(1); j++)
                {
                    rooms[i, j].hasDirt = salles[i, j].getDirt;
                    rooms[i, j].hasBijoux = salles[i, j].getBijoux;
                }
            }
            return rooms;
        }

    }
}
