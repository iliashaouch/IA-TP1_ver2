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
        // les booléen dirt et bijoux indiquent respectiviement si il y a des saletés ou des bijoux dans la salle
        private bool dirt = false;
        private bool bijoux = false;

        public Salle()
        {

        }

        // les fonctions getDirt et getBijoux rendent respectivement les booléen dirt et bijoux et permettent ainsi de connaitre l'état de la pièce
        public bool getDirt
        {
            get => dirt;
        }
        public bool getBijoux
        {
            get => bijoux;
        }

        // les fonctions setDirt et setBijoux prennent en paramètre un booléen et applique sa valeur respectivement aux variabes "dirt" et "bijoux" 
        // de la salle et permettent ainsi de changer l'état de la pièce
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
        private UInt16 nbBijouxAspires = 0;


        public Manoire()
        {
            for (int i = 0; i < salles.GetLength(0); i++)
            {
                for (int j = 0; j < salles.GetLength(1); j++) salles[i, j] = new Salle();
            }

        }

        internal Salle[,] Salles { get => salles; set => salles = value; }

        // La fonction "createObject" choisis un nombre puis un tableau de deux entiers au hasard, le premier nombre indique, selon sa valeur
        // si une saleté (si il est pair) ou un bijou (si il est égale à 1) sera généré dans une salle (dont la position est déterminé
        // par le tableau d'entiers) 
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

        // La fonction "cleanRoom" prend en paramètre un tableau d'entier représentant  la position d'une salle qu'elle nettoie (elle la vide de 
        // saleté et de bijou et si il y avait des bijoux dans cette salle elle incrémente "nbBijouxAspires", un compteur de bijoux aspiré
        public void cleanRoom(int[] s)
        {
            if (salles[s[0], s[1]].getBijoux) { ++nbBijouxAspires; }

            salles[s[0], s[1]].setBijoux(false);
            salles[s[0], s[1]].setDirt(false);
        }

        // La fonction "ramassageBijoux" prend en paramètre un tableau d'entier représentant  la position d'une salle qu'elle vide de bijoux 
        public void ramassageBijoux(int[] s)
        {
            salles[s[0], s[1]].setBijoux(false);
        }

        // la fonction "getState" rend un tableau de pièce correspondant à l'état actuel du manoir
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

    }
}
