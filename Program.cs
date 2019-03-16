using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph_Traversal
{
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

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f = new Form1();
            Application.Run(f);
        }

        public static void Main2(string file1, string file2)
        {
            int jumlahRumah = 0;
            //Mmebuat node
            List<Node> arr = new List<Node>();
            //Membuat List of list yang berupa List[i][j] , dimana i adalah no rumah dan j adalah tetangganya
            List<List<int>> house = new List<List<int>>();
            List<int> konstruktor = new List<int>();
            //Solusi adalah path dari Y ke X
            //DFSTrack adalah langkah pengerjaan DFS (masih bermasalah)

            BacaFileGraf(ref arr, ref jumlahRumah, file1); //Dibuat prosedur sendiri agar lebih  compact

            for (int i = 0; i < jumlahRumah + 1; i++)
            {
                house.Add(konstruktor);
                konstruktor = new List<int>();
            }

            Console.WriteLine();
            Console.WriteLine("Berdasarkan file, berikut Hubungan antara rumah dalam Negeri Antah Berantah : ");

            foreach (Node no in arr)
            {
                Console.WriteLine("{0} - {1}", no.getfrom(), no.getto());
                house[no.getfrom()].Add(no.getto());
            }

            foreach (List<int> list in house)
            {
                list.Sort();
            }

            //Bagian membaca query dari file eksternal
            Console.WriteLine();
            Console.WriteLine("Masukkan file eksternal yang berisi query: ");

            string path = file2;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            int maxQuery = int.Parse(lines[0]);

            for (int i = 1; i < maxQuery + 1; i++)
            {
                string[] bits = lines[i].Split(' ');
                int code = int.Parse(bits[0]);
                int X = int.Parse(bits[1]);
                int Y = int.Parse(bits[2]);

                List<int> solusi = new List<int>();
                List<int> DFSTrack = new List<int>();

                Console.Write("Query no ");
                Console.Write(i);
                Console.WriteLine(" : ");

                if (code == 0)
                { //Mendekati istana
                    DFS(X, Y, house, solusi, DFSTrack);
                    if (solusi.Count == 0)
                    {
                        Console.WriteLine("TIDAK");
                    }
                    else
                    {
                        solusi.Reverse();
                        DFSTrack.Reverse();
                        Console.Write("IYA, rutenya adalah ");
                        display(solusi);
                        List<int> DFSTrackrev = reverseTrack(DFSTrack, Y, X);
                        Console.Write("   Langkah DFSnya : ");
                        display(DFSTrackrev);
                    }
                }

                else if (code == 1)
                { //Menjauhi istana
                    DFS(Y, X, house, solusi, DFSTrack);
                    if (solusi.Count == 0)
                    {
                        Console.WriteLine("TIDAK");
                    }
                    else
                    {
                        Console.Write("IYA, rutenya adalah ");
                        display(solusi);
                        Console.Write("   Langkah DFSnya : ");
                        display(DFSTrack);
                    }
                }
            }
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
    }
}
