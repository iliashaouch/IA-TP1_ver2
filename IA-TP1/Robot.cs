using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_TP1
{

    // La structure noeud décrit un des noeuds de notre graph, elle est composée d'un string "actions" correspondant à la liste des actions entreprises
    // pour arriver à ce noeud, du tableau d'entier "pos" qui indique la position du robot à la fin de la séquence d'action (via 2 entiers), 
    // de deux listes de tableaux d'entiers "dirtLeft" et "bijouLeft" qui répertorie les positions respectivement des saletés et de bijoux restant
    // après la séquance d'action de ce noeud et enfin un int "heuristic" qui correspond à l'heuristique de ce noeud dans le cadre de l'algorithme A*
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

        // cette fonction permet de déterminer quelle algorithme sera utilisé par le programme via un booléen (si le paramètre entré est "true" alors 
        // l'algorithme utilisera A*, sinon il utilisera BFS (Breadth First Search)
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

        // Cette fonction est la fonction de vie du robot, elle boucle en permanance et répète les étapes : 
        // "observer l'environnement et mettre à jour mon état interne" ==> "Choisir une série d'action" ==> "executer la série d'action"
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

        // La fonction "FindAllDirt" prend en paramètre une représentation du manoir et rend la liste des positions des saletés dans le manoir
        // sous la forme d'une liste de tableaux d'entiers : List<int[]>
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

        // La fonction "FindAllBijou" prend en paramètre une représentation du manoir et rend la liste des positions des bijoux dans le manoir
        // sous la forme d'une liste de tableaux d'entiers : List<int[]>
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

        // La fonction "findHeuristic" prend un paramètre un noeud et rend sa valeur d'heuristique, elle est égale à la somme du nombre de bijoux 
        // et de saleté restants dans le manoir à la suite de sa séquence d'actions
        public int findHeuristic(noeud n)
        {
            return (n.bijouLeft.Count + n.dirtLeft.Count);
        }

        // La fonction "AStarSearch" rend la série d'actions à effectuer par le robot pour nettoyer le manoire, elle la trouve en utiisant l'algorithme A*
        public string AStarSearch()
        {
            // on créer le graph qui servira aussi de file d'attente pour les noeuds à explorer et le premier noeud et on ajoute le premier noeud au graph
            List<noeud> graph = new List<noeud>();
            noeud n = new noeud();
            n.actions = "";
            n.pos = position;
            n.bijouLeft = FindAllBijou(memoire);
            n.dirtLeft = FindAllDirt(memoire);
            n.heuristic = findHeuristic(n);
            graph.Add(n);
            // on test le noeud original pour savoir si des actions sont nécessaires 
            bool solutionFound = testSolution(graph[0].actions);
            while (!solutionFound) // tant que la solution n'est pas trouvé
            {
                // on cherche toutes les actions possibles à partir du noeud étudié
                string actionspossible = getAllPossibleActions(graph[0].pos, graph[0].dirtLeft, graph[0].bijouLeft);
                foreach (char action in actionspossible) // pour chacune de ces actions on créer un nouveau noeud
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
                                listRemove(node.dirtLeft, node.pos) ; // si on aspire des saletés on met à jour la liste de saletés restantes
                                node.heuristic = findHeuristic(node); // et on met à jour l'heuristique du noeud
                            }
                            break;
                        case 'r':
                            if (listContains(node.bijouLeft, node.pos))
                            {
                                listRemove(node.bijouLeft, node.pos); // si on aspire des saletés on met à jour la liste de saletés restantes
                                node.heuristic = findHeuristic(node); // et on met à jour l'heuristique du noeud
                            }
                            break;
                    }
                    // ensuite on parcour le graph et on ajoute le noeud au bon endroit (avant tous les noeud qui on une heurisitque moins bonne que la sienne)
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
                // après avoir obtenus tous les noeuds fils du noeud parent on supprime le noeud parent
                graph.RemoveAt(0);
                // on test le nouveau noeud se trouvant en tête de la file d'attente des noeuds à explorer (graph)
                solutionFound = testSolution(graph[0].actions);
            }
            // une fois que l'on a trouvé une bonne solution on rend la liste d'actions correspondant
            return (graph[0].actions);
        }

        // La fonction "listContains" prend en paramètre une liste de tableaux d'entier et un tableau d'entier et rend "true" si la liste contient 
        // ce tableau et "false" dans le cas contraire
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

        // La fonction "listRemove" prend en paramètre une liste de tableaux d'entier et un tableau d'entier et supprime ce tableau de la liste à sa première apparition
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


        // La fonction "Search" rend la série d'actions à effectuer par le robot pour nettoyer le manoire, elle la trouve en utiisant 
        // l'algorithme BFS (Breadth First Search)
        public string search()
        {
            // on créer le graph qui servira aussi de file d'attente pour les noeuds à explorer et le premier noeud et on ajoute le premier noeud au graph
            List<noeud> graph = new List<noeud>();
            noeud n = new noeud();
            n.actions = "";
            n.pos = position;
            n.bijouLeft = FindAllBijou(memoire);
            n.dirtLeft = FindAllDirt(memoire);
            graph.Add(n);
            // on test le noeud original pour savoir si des actions sont nécessaires 
            bool solutionFound = testSolution(graph[0].actions);
            while (!solutionFound)
            {
                // on cherche toutes les actions possibles à partir du noeud étudié
                string actionspossible = getAllPossibleActions(graph[0].pos, graph[0].dirtLeft, graph[0].bijouLeft);
                foreach (char action in actionspossible) // pour chacune de ces actions on créer un nouveau noeud
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
                                listRemove(node.dirtLeft, node.pos); // si on aspire des saletés on met à jour la liste de saletés restantes
                            }
                            break;
                        case 'r':
                            if (listContains(node.bijouLeft, node.pos))
                            {
                                listRemove(node.bijouLeft, node.pos); // si on aspire des bijoux on met à jour la liste de saletés restantes
                            }
                            break;
                    }
                    // ensuite on ajoute le noeud à la fin du graph
                    graph.Add(node);
                }
                // après avoir obtenus tous les noeuds fils du noeud parent on supprime le noeud parent
                graph.RemoveAt(0);
                // on test le nouveau noeud se trouvant en tête de la file d'attente des noeuds à explorer (graph)
                solutionFound = testSolution(graph[0].actions);
            }
            // une fois que l'on a trouvé une bonne solution on rend la liste d'actions correspondant
            return (graph[0].actions);
        }

        // La fonction "testSolution" prend en paramètre un string correspondant à une série d'action et retourne vraie si cette série d'action permet 
        // de nettoyer le manoir et faux dans le cas contraire
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

        // La fonction "getAllPossibleActions" prend en paramètres un tableau d'entier indiquant la position actuel du robot ansi que deux 
        // listes de tableaux indiquant respectivement les positions des saletés et des bijoux dans la manoir et rend un string correspondant
        // à la liste des actions réalisable par le robot dans sa situation
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

        // La fonction "execActions" prend en paramètre une série d'actions et les exécute en communiquant avec les fonction de la classe "effecteur"
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

    }
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
