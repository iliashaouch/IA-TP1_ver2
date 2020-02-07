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
        private int[] position;
        private room[,] memoire;
        private capteur c;
        private effecteur e;
        private int conso;
        private Manoire m_manoir;
        private string actionsList;

        public Robot(Manoire manoir)
        {
            position = new[] { 0, 0 };
            m_manoir = manoir;
            c = new capteur(m_manoir);
            e = new effecteur(m_manoir);
        }

        public void startLifeCycle()
        {
            while (true)
            {
                memoire = c.captureEnv();
                actionsList = search();
                execActions(actionsList);
            }
        }



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
                            node.pos[0] += 1;
                            break;
                        case 'g':
                            node.pos[1] -= 1;
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

        void execActions(string strActions)
        {
            foreach (char action in strActions)
            {
                switch (action)
                {
                    case 'h':
                    case 'b':
                    case 'g':
                    case 'd':
                        position = e.deplacer(action, position);
                        break;
                    case 'a':
                        e.aspirer(position);
                        break;
                    case 'r':
                        e.ramasser(position);
                        break;
                }
            }
        }

        public bool testSolution(string solution)
        {
            room[,] state = new room[Memoire.GetLength(0), Memoire.GetLength(1)];
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
                actions += 'b';
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
                actions += 'g';
            }
            return actions;
        }

    }
    class capteur
    {
        private Manoire m_manoir;

        public capteur(Manoire manoir)
        {
            m_manoir = manoir;
        }

        public room[,] captureEnv()
        {
            return m_manoir.getState();
        }
    }

    class effecteur
    {
        private Manoire m_manoir;

        public effecteur(Manoire manoir)
        {
            m_manoir = manoir;
        }

        public void aspirer(int[] pos)
        {
            m_manoir.cleanRoom(pos);
        }

        public void ramasser(int[] pos)
        {
            m_manoir.ramassageBijoux(pos);
        }

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
