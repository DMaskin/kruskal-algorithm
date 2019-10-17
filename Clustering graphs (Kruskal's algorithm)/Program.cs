using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering_graphs__Kruskal_s_algorithm_
{
    /// 
    /// Class represents graph edge.
    /// 
    public class Edge
    {
        public int U;
        public int V;
        public double Weight;
    }

    /// 
    /// Implementation of Kruskal algorithm.
    /// 
    public class Kruskal
    {
        private const int MAX = 100;
        private int _edgesCount;
        private int _verticlesCount;
        private List<Edge> _edges;
        private int[,] tree;
        private int[] sets;

        public List<Edge> Edges { get { return _edges; } }
        public int VerticlesCount { get { return _verticlesCount; } }
        public double Cost { get; private set; }

        public List<Edge> SetEdges
        {
            set
            {
                _edges = value;
            }
        }

        public Kruskal(string input)
        {
            tree = new int[MAX, 3];
            sets = new int[MAX];

            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            _verticlesCount = int.Parse(lines[0]);
            _edgesCount = int.Parse(lines[1]);
            _edges = new List<Edge>();

            _edges.Add(null);

            for (int i = 2; i < lines.Count(); i++)
            {
                string[] line = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                _edges.Add(new Edge
                {
                    U = int.Parse(line[0]),
                    V = int.Parse(line[1]),
                    Weight = double.Parse(line[2])
                });
            }

            for (int i = 1; i <= _verticlesCount; i++)
                sets[i] = i;
        }

        private void ArrangeEdges(int k) //сортируем рёбра
        {
            Edge temp;
            for (int i = 1; i < k; i++)
            {
                for (int j = 1; j <= k - i; j++)
                {
                    if (_edges[j].Weight > _edges[j + 1].Weight)
                    {
                        temp = _edges[j];
                        _edges[j] = _edges[j + 1];
                        _edges[j + 1] = temp;
                    }
                }
            }
        }

        /*private int Find(int vertex)
        {
            return (sets[vertex]);
        } */

        public int Find(int x)
        {
            if (sets[x] == x) return x;
            return sets[x] = Find(sets[x]);
        }

        private void Join(int v1, int v2)
        {
            if (v1 < v2)
                sets[v2] = v1;
            else
                sets[v1] = v2;
        }

        public void BuildSpanningTree(int clusters)
        {
            //int k = _verticlesCount;
            int k = _edgesCount;
            int i, t = 1;
            this.ArrangeEdges(k);
            this.Cost = 0;
            for (i = 1; i <= k; i++)
            {
                for (i = 1; i < k; i++)
                    if (this.Find(_edges[i].U) != this.Find(_edges[i].V))
                    {
                        tree[t, 0] = (int)_edges[i].Weight;
                        tree[t, 1] = _edges[i].U;
                        tree[t, 2] = _edges[i].V;
                        this.Cost += _edges[i].Weight;
                        this.Join(Find(_edges[i].U), Find(_edges[i].V));
                        t++;
                    }

            }
        }

        public void DisplayInfo(int clusters)
        {
            Console.WriteLine("Рёбрами минимального остовного дерева являются:");
            //for (int i = 1; i <_verticlesCount; i++) //исходный вывод для алг Краскала
            for (int i = 1; i < _verticlesCount; i++) //Кластеризация				
                Console.WriteLine(tree[i, 1] + " - " + tree[i, 2] + " вес: " + tree[i, 0]);

            Console.WriteLine("\nДля разделения на " + clusters + "кластеров исключаем вершины: ");
            for (int i = _verticlesCount - (clusters - 1); i < _verticlesCount; i++)
                Console.WriteLine(tree[i, 1] + " - " + tree[i, 2] + " вес: " + tree[i, 0]);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            /*Kruskal k = new Kruskal(@"6
10
1 2 1
2 5 6
1 3 3
5 3 7
3 4 4
3 2 2
2 4 5
5 4 8
4 6 9
5 6 10");*/

            /*Kruskal k = new Kruskal(@"5
8
1 3 3
1 2 5
1 5 7
2 3 1
3 5 8
2 5 5
5 4 3
2 4 1
");*/

            Kruskal kruskal = new Kruskal(@"12
16
1 2 2
2 3 1
1 4 4
4 3 2
4 10 3
3 5 6
10 11 1
11 12 2
10 5 5
12 7 4
7 8 2
7 5 3
5 6 2
7 6 2
6 9 3
9 8 1");
            int clusters = 3;

            /*Kruskal kruskal = new Kruskal(@"9
12
1 2 2
2 3 3
1 3 4
3 4 6
4 5 3
5 6 1
4 6 2
4 7 6
3 7 5
7 8 2
8 9 4
7 9 2");



			int clusters = 3;*/
            kruskal.BuildSpanningTree(clusters);
            Console.WriteLine("Стоимость: " + kruskal.Cost);
            kruskal.DisplayInfo(clusters);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
