using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP1
{
    
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
}
