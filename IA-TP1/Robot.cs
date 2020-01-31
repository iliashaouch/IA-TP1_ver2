using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP1
{

    public struct noeud
    {
        public string actions;
        public int[] pos;
    }

    class Robot
    {
        private int[] position = new int[2];
        private room[,] memoire;
        private capteur c;
        private effecteur e;
        private int conso;

        public room[,] Memoire { get => memoire; set => memoire = value; }
        public int[] Position { get => position; set => position = value; }

        public string search()
        {
            List<noeud> graph = new List<noeud>();
            noeud n = new noeud();
            n.actions = "";
            n.pos = position;
            graph.Add(n);
            bool solutionFound = testSolution(graph[0].actions);
            while (!solutionFound)
            {
                string actionspossible = getAllPossibleActions(graph[0].pos);
                foreach (char action in actionspossible)
                {
                    noeud node = new noeud();
                    node.actions = graph[0].actions + action;
                    node.pos = graph[0].pos;
                    switch (action)
                    {
                        case 'h':
                            node.pos[0] -= 1;
                            break;                                          
                        case 'b':                                           
                            node.pos[0] +=1;
                            break;                                          
                        case 'g':                                           
                            node.pos[1]-=1;
                            break;                                          
                        case 'd':                                           
                            node.pos[1] += 1;
                            break;
                    }
                    graph.Add(node);
                }
                graph.RemoveAt(0);
                solutionFound = testSolution(graph[0].actions);
            }
            return (graph[0].actions);
        }

        public bool testSolution(string solution)
        {
            room[,] state = new room[Memoire.GetLength(0),Memoire.GetLength(1)];
            int[] pos = new int[2];
            Array.Copy(memoire, state, memoire.Length);
            Array.Copy(position, pos, position.Length);

            foreach (char action in solution)
            {
                switch (action)
                {
                    case 'a':
                        if (state[pos[0], pos[1]].hasBijoux)
                        {
                            return false;
                        }
                        state[pos[0], pos[1]].hasDirt = false;
                        state[pos[0], pos[1]].hasBijoux = false;
                        break;
                    case 'r':
                        state[pos[0], pos[1]].hasBijoux = false;
                        break;
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
                    default: return false;
                }
            }
            foreach (room r in state)
            {
                if (r.hasBijoux || r.hasDirt)
                {
                    return false;
                }
            }
            return true;
        }

        public string getAllPossibleActions(int[] pos)
        {
            string actions = "ar";
            if (pos[0] < memoire.GetLength(0))
            {
                actions+='b';
            }
            if (pos[0] > 0)
            {
                actions += 'h';
            }
            if (pos[1] < memoire.GetLength(1))
            {
                actions += 'd';
            }
            if (pos[1] > 0)
            {
                actions+= 'g';
            }
            return actions;
        }

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
