using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace Graph_Traversal_Simulation
{
    //Kelas bentukan Node
    class Node : IEquatable<Node>
    {
        //Atribut
        public int from; //Node awal
        public int to; //Node tujuan

        //Ctor
        public Node(int f, int t)
        {
            setfrom(f);
            setto(t);
        }

        //Setter
        public void setfrom(int f)
        {
            from = f;
        }
        public void setto(int t)
        {
            to = t;
        }

        //Getter
        public int getfrom()
        {
            return from;
        }
        public int getto()
        {
            return to;
        }

        //Override perbandingan
        public bool Equals(Node n)
        {
            return this.from == n.from && this.to == n.to;
        }
    }

    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///

        //Deklarasi Graf Dua Arah
        private static IBidirectionalGraph<object, IEdge<object>> appGraph;

        //Getter dan Setter Graf
        public static IBidirectionalGraph<object, IEdge<object>> AppGraph
        {
            get { return appGraph; }
        }

        //Method Membuat Graf
        private static void CreateGraph(List<Node> arr, int n)
        {
            var g = new BidirectionalGraph<object, IEdge<object>>();

            //Menambah Simpul
            string[] vertices = new string[n];
            for (int i = 0; i < n; i++)
            {
                vertices[i] = (i + 1).ToString();
                g.AddVertex(vertices[i]);
            }

            //Menambah Edge
            for (int i = 0; i < arr.Count; i++)
            {
                //Edge Ditambah 2 kali karena 2 arah
                g.AddEdge(new Edge<object>(getElFrom(i).ToString(), getElTo(i).ToString()));
                g.AddEdge(new Edge<object>(getElTo(i).ToString(), getElFrom(i).ToString()));
            }

            //Assign graf yang mau ditampilkan dengan g
            appGraph = g;
        }

        //Atribut
        private static int JumlahRumah = 0; //Jumlah node yang ada
        private static List<Node> arr; //List of Node untuk menyimpan edge dari file
        private static List<List<int>> house; //List of List of int untuk mengisi ketetanggaan dari setiap simpul
        private static string route = ""; //String untuk menyimpan rute dari node awal ke node tujuan jika jawaban == "YA"
        private static string journey = ""; //String untuk menyimpan rute yang ditempul DFS untuk mencari rute ke node tujuan

        //Getter
        public static int getJumlahRumah()
        {
            return JumlahRumah;
        }

        public static int getElFrom(int idx)
        {
            return arr[idx].getfrom();
        }

        public static int getElTo(int idx)
        {
            return arr[idx].getto();
        }

        public static string getroute()
        {
            return route;
        }

        public static string getjourney()
        {
            return journey;
        }

        //Setter
        public static void setJumlahRumah(int n)
        {
            JumlahRumah = n;
        }

        

        public static void main(string file1, string file2)
        {
            //Mmebuat node
            arr = new List<Node>();
            //Membuat List of list yang berupa List[i][j] , dimana i adalah no rumah dan j adalah tetangganya
            house = new List<List<int>>();
            List<int> konstruktor = new List<int>();
            //Solusi adalah path dari Y ke X
            //DFSTrack adalah langkah pengerjaan DFS (masih bermasalah)

            BacaFileGraf(ref arr, ref JumlahRumah, file1); //Dibuat prosedur sendiri agar lebih  compact

            for (int i = 0; i < JumlahRumah + 1; i++)
            {
                house.Add(konstruktor);
                konstruktor = new List<int>();
            }

            foreach (Node no in arr)
            {
                house[no.getfrom()].Add(no.getto());
            }

            CreateGraph(arr, JumlahRumah);

            foreach (List<int> list in house)
            {
                list.Sort();
            }
        }

        //Pada dasarnya membaca file eksternal berupa map rumah (hubungan edge antar simpul)
        static void BacaFileGraf(ref List<Node> arr, ref int n, string file1)
        {
            string path = file1;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8); //menyimpan semua isi file dalam array of string

            n = int.Parse(lines[0]); //baris pertama file yang disimpan pada lines[0] yang berisi jumlah simpul disimpan

            for (int i = 1; i < n; i++)
            {
                string[] bits = lines[i].Split(' '); //Memisahkan spasi dari baris menjadi 3, yaitu kode 0/1, simpul tujuan, dan simpul awal
                arr.Add(new Node(int.Parse(bits[0]), int.Parse(bits[1]))); //Menambahkan ke list of Node simpul awal dan simpul tujuan
            }
            List<Node> SortedList = arr.OrderBy(o => o.from).ToList(); //Melakukan sort list of Node
            arr = SortedList;
        }

        //Method untuk menyelesaikan suatu query
        public static string SolveQuery(string q)
        {
            string[] bits = q.Split(' '); //Pemisahan isi query menjadi kode 0/1, simpul tujuan, dan simpulan awal
            int code = int.Parse(bits[0]); //Berisi kode; 0 untuk bergerak mendekati istana; 1 untuk bergerak menjauhi istana
            int X = int.Parse(bits[1]); //Berisi simpul tujuan
            int Y = int.Parse(bits[2]); //Berisi simpul awal

            List<int> solusi = new List<int>(); //List of int untuk menyimpan rute solusi
            List<int> DFSTrack = new List<int>(); //List of int untuk menyimpan jalur pencarian DFS

            if (code == 0) //Jika bergerak mendekati istana
            { //Mendekati istana
                DFS(X, Y, house, solusi, DFSTrack); //X dan Y ditukar sehingga DFS dari simpul X ke simpul Y
                if (solusi.Count == 0) //Jika tidak ada solusi, mengembalikan "TIDAK"
                {
                    return "TIDAK";
                }
                else //Jika ada solusi, mengembalikan "YA"
                {
                    solusi.Reverse(); //Karena X dan Y ditukar, solusi juga perlu direverse
                    DFSTrack.Reverse();
                    route = display(solusi);
                    List<int> DFSTrackrev = reverseTrack(DFSTrack, Y, X);
                    journey = display(DFSTrackrev);
                    return "YA";
                }
            }

            else if (code == 1) //Jika bergerak menjauhi istana
            { //Menjauhi istana
                DFS(Y, X, house, solusi, DFSTrack); //DFS dari simpul Y ke X
                if (solusi.Count == 0) //Jika tidak ada solusi, mengembalikan "TIDAK"
                {
                    return "TIDAK";
                }
                else //Jika ada, mengembalikan "YA"
                {
                    route = display(solusi);
                    journey = display(DFSTrack);
                    return "YA";
                }
            }
            else //Jika kode bukan nol atau 1, mengembalikan "INPUT SALAH"
            {
                return "INPUT SALAH";
            }
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
            if (from == to) //Jika simpul awal sama dengan simpul tujuan, berarti mungkin dicapai
            {
                found = true;
            }
            else if (grafList[from].Count == 0) //Jika DFS telah mencapai daun, lakukan backtrack
            {
                backtrack = true;
                solusi.RemoveAt(solusi.Count - 1);
            }
            else
            {
                for (int i = 0; i < grafList[from].Count; i++) //Loop hingga ketemu / semua daun telah ditelusuri
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
                    else
                    {
                        break;
                    }
                }
                if (!found)
                { solusi.RemoveAt(solusi.Count - 1); }
                backtrack = true;
            }
        }

        //Prosedur untuk display isi List
        public static string display(List<int> someList) //Untuk memgembalikan isi dari sebuah list dalam bentuk satu string
        {
            string res = "";
            int i = 0;
            foreach (int elemen in someList)
            {
                res += elemen;
                if (i < someList.Count-1)
                {
                    res += " - > ";
                }
                i++;
            }
            return res;
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
