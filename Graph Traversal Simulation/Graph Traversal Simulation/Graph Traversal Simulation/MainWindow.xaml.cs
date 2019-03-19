using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickGraph;

namespace Graph_Traversal_Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    class Node : IEquatable<Node>
    {
        //Atribut
        public int from;
        public int to;

        public Node(int f, int t)
        {
            setfrom(f);
            setto(t);
        }
        public void setfrom(int f)
        {
            from = f;
        }
        public void setto(int t)
        {
            to = t;
        }
        public int getfrom()
        {
            return from;
        }
        public int getto()
        {
            return to;
        }
        public bool Equals(Node n)
        {
            return this.from == n.from && this.to == n.to;
        }
    }

    public partial class MainWindow : Window
    {
        private IBidirectionalGraph<object, IEdge<object>> _graphToVisualize;

        public IBidirectionalGraph<object, IEdge<object>> GraphToVisualize
        {
            get { return _graphToVisualize; }
        }

        public MainWindow()
        {
            Main();
            InitializeComponent();
        }

        public void Main()
        {
            List<Node> arr = new List<Node>();
            arr.Add(new Node(1, 2));
            arr.Add(new Node(1, 7));
            arr.Add(new Node(1, 3));
            arr.Add(new Node(2, 9));
            arr.Add(new Node(5, 4));
            arr.Add(new Node(5, 6));
            arr.Add(new Node(7, 8));
            arr.Add(new Node(3, 5));
            CreateGraphToVisualize(arr, 9);
        }

        private void CreateGraphToVisualize(List<Node> arr, int n)
        {
            var g = new BidirectionalGraph<object, IEdge<object>>();

            //add the vertices to the graph
            string[] vertices = new string[n+1];
            for (int i = 0; i < n+1; i++)
            {
                vertices[i] = (i+1).ToString();
                g.AddVertex(vertices[i]);
            }

            //add some edges to the graph
            for(int i = 0; i < arr.Count; i++)
            {
                g.AddEdge(new Edge<object>((arr[i].getfrom()).ToString(), (arr[i].getto()).ToString()));
            }

            /*foreach (Node no in arr)
            {
                g.AddEdge(new Edge<object>(vertices[no.getfrom()], vertices[no.getto()]));
            }*/

            _graphToVisualize = g;
        }

        //Pada dasarnya membaca file eksternal berupa map rumah
        static void BacaFileGraf(ref List<Node> arr, ref int n, string file1)
        {
            string path = file1;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            n = int.Parse(lines[0]);

            for (int i = 1; i < n; i++)
            {
                string[] bits = lines[i].Split(' ');
                arr.Add(new Node(int.Parse(bits[0]), int.Parse(bits[1])));
                //arr.Add(new Node(int.Parse(bits[1]), int.Parse(bits[0])));
            }
            List<Node> SortedList = arr.OrderBy(o => o.from).ToList();
            arr = SortedList;
        }

        //DFS untuk Y ke X
        public static void DFS(int from, int to, List<List<int>> grafList, List<int> solusi, List<int> DFSTrack)
        {
            bool found = false;
            bool backtrack = false;
            solusi.Add(from);
            DFSTrack.Add(from);
            DFSUtil(from, to, grafList, solusi, DFSTrack, ref found, ref backtrack);
        }

        //Dibantu dengan utility dari DFS
        public static void DFSUtil(int from, int to, List<List<int>> grafList, List<int> solusi, List<int> DFSTrack, ref bool found, ref bool backtrack)
        {
            if (from == to)
            {
                found = true;
            }
            else if (grafList[from].Count == 0)
            {
                backtrack = true;
                solusi.RemoveAt(solusi.Count - 1);
            }
            else
            {
                for (int i = 0; i < grafList[from].Count; i++)
                {
                    if (!found)
                    {
                        solusi.Add(grafList[from][i]);
                        DFSTrack.Add(grafList[from][i]);
                        DFSUtil(grafList[from][i], to, grafList, solusi, DFSTrack, ref found, ref backtrack);

                        if (!found && backtrack)
                        {
                            DFSTrack.Add(from);
                            backtrack = false;
                        }
                    }
                }
                if (!found)
                { solusi.RemoveAt(solusi.Count - 1); }
                backtrack = true;
            }
        }

        //Prosedur untuk display isi List
        public static void display(List<int> someList)
        {
            foreach (int elemen in someList)
            {
                Console.Write(elemen);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        //Fungsi yang mengakali isi TrackDFS untuk kasus dari daun menuju akar (rumah menuju istana)
        public static List<int> reverseTrack(List<int> someList, int Y, int X)
        {
            List<int> list = new List<int>();
            int i = 0;
            while (someList[i] != X)
            {
                list.Add(someList[i]);
                i++;
            }
            list.Add(X);
            return list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            a.Text = "Halo";
            Console.WriteLine("Halo");
            Main();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
    
}
