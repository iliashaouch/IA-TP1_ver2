using System;
using System.Collections;
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
        public List<int[]> dirtLeft;
        public List<int[]> bijouLeft;
        public int heuristic;
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

        private bool robotMode;

        public Robot(Manoire manoir)
        {
            position = new[] { 0, 0 };
            m_manoir = manoir;
            c = new capteur(m_manoir);
            e = new effecteur(m_manoir);
        }

        public void setMode(bool mode)
        {
            robotMode = mode;
            if (robotMode)
            {
                Console.WriteLine("AStar");
            }
            else
            {
                Console.WriteLine("BFS");
            }
        }

        public void startLifeCycle()
        {
            while (true)
            {
                memoire = c.captureEnv();
                actionsList = !robotMode ? search() : AStarSearch();
                execActions(actionsList);
            }
        }



        public room[,] Memoire { get => memoire; set => memoire = value; }
        public int[] Position { get => position; set => position = value; }

        public int compare2Nodes(noeud n2, noeud n1)
        {
            if (n1.heuristic == n2.heuristic)
            {
                return 0;
            }
            else if (n1.heuristic < n2.heuristic)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public List<int[]> FindAllDirt(room[,] memoire)
        {
            List<int[]> res = new List<int[]>();
            for (int i = 0; i < memoire.GetLength(0); i++)
            {
                for (int j = 0; j < memoire.GetLength(1); j++)
                {
                    if (memoire[i, j].hasDirt)
                    {
                        int[] posi = { i, j };
                        res.Add(posi);
                    }
                }
            }
            return res;
        }

        public List<int[]> FindAllBijou(room[,] memoire)
        {
            List<int[]> res = new List<int[]>();
            for (int i = 0; i < memoire.GetLength(0); i++)
            {
                for (int j = 0; j < memoire.GetLength(1); j++)
                {
                    if (memoire[i, j].hasBijoux)
                    {
                        int[] posi = { i, j };
                        res.Add(posi);
                    }
                }
            }
            return res;
        }

        public int findHeuristic(noeud n)
        {
            return (n.bijouLeft.Count + n.dirtLeft.Count);
        }


        public string AStarSearch()
        {
            List<noeud> graph = new List<noeud>();
            noeud n = new noeud();
            n.actions = "";
            n.pos = position;
            n.bijouLeft = FindAllBijou(memoire);
            n.dirtLeft = FindAllDirt(memoire);
            n.heuristic = findHeuristic(n);

            graph.Add(n);
            bool solutionFound = testSolution(graph[0].actions);
            while (!solutionFound)
            {
                string actionspossible = getAllPossibleActions(graph[0].pos, graph[0].dirtLeft, graph[0].bijouLeft);
                foreach (char action in actionspossible)
                {
                    noeud node = new noeud();
                    node.actions = "";
                    node.pos = new int[2];
                    Array.Copy(graph[0].pos, node.pos, graph[0].pos.Length);
                    node.heuristic = graph[0].heuristic;
                    node.bijouLeft = new List<int[]>(graph[0].bijouLeft);
                    node.dirtLeft = new List<int[]>(graph[0].dirtLeft);
                    node.actions = graph[0].actions + action;
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
                        case 'a':
                            if (listContains(node.dirtLeft, node.pos))
                            {
                                listRemove(node.dirtLeft, node.pos) ;
                                node.heuristic = findHeuristic(node);
                            }
                            break;
                        case 'r':
                            if (listContains(node.bijouLeft, node.pos))
                            {
                                listRemove(node.bijouLeft, node.pos);
                                node.heuristic = findHeuristic(node);
                            }
                            break;
                    }
                    for (int i = 0; i < graph.Count; i++)
                    {
                        if (i >= graph.Count - 1)
                        {
                            graph.Add(node);
                            break;
                        }
                        if (graph[i].heuristic > node.heuristic)
                        {
                            if (i == 0) { ++i; }
                            graph.Insert(i, node);
                            break;
                        }
                    }
                }
                graph.RemoveAt(0);
                //graph.Sort(compare2Nodes);
                solutionFound = testSolution(graph[0].actions);
            }
            return (graph[0].actions);
        }

        public bool listContains(List<int[]> l, int[] pos)
        {
            foreach (int[] e in l){
                if (e[0]==pos[0] && e[1] == pos[1])
                {
                    return true;
                }
            }
            return false;
        }

        public void listRemove(List<int[]> l, int[] pos)
        {
            int a = 0;
            for (int i=0; i<l.Count;i++)
            {
                if (l[i][0] == pos[0] && l[i][1] == pos[1])
                {
                    a = i;
                    break;
                }
            }
            l.RemoveAt(a);
        }



        public string search()
        {
            List<noeud> graph = new List<noeud>();
            noeud n = new noeud();
            n.actions = "";
            n.pos = position;
            n.bijouLeft = FindAllBijou(memoire);
            n.dirtLeft = FindAllDirt(memoire);
            graph.Add(n);
            bool solutionFound = testSolution(graph[0].actions);
            while (!solutionFound)
            {
                string actionspossible = getAllPossibleActions(graph[0].pos, graph[0].dirtLeft, graph[0].bijouLeft);
                foreach (char action in actionspossible)
                {
                    noeud node = new noeud();
                    node.actions = "";
                    node.pos = new int[2];
                    Array.Copy(graph[0].pos, node.pos, graph[0].pos.Length);
                    node.bijouLeft = new List<int[]>(graph[0].bijouLeft);
                    node.dirtLeft = new List<int[]>(graph[0].dirtLeft);
                    node.actions = graph[0].actions + action;
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
                        case 'a':
                            if (listContains(node.dirtLeft, node.pos))
                            {
                                listRemove(node.dirtLeft, node.pos);
                            }
                            break;
                        case 'r':
                            if (listContains(node.bijouLeft, node.pos))
                            {
                                listRemove(node.bijouLeft, node.pos);
                            }
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
                System.Threading.Thread.Sleep(1000);
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

        public string getAllPossibleActions(int[] pos, List<int[]> dirts, List<int[]> bijoux)
        {
            if (listContains(bijoux,pos))
            {
                return "r";
            }
            if (listContains(dirts, pos))
            {
                return "a";
            }
            string actions = "";
            if (pos[0] < memoire.GetLength(0) - 1)
            {
                actions += 'b';
            }
            if (pos[0] > 0)
            {
                actions += 'h';
            }
            if (pos[1] < memoire.GetLength(1) - 1)
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
