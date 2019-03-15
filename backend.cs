using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
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

    class Program
    {
        //Pada dasarnya membaca file eksternal berupa map rumah
        static void BacaFileGraf(ref List<Node> arr, ref int n){
              string filename;
              Console.WriteLine("Masukkan file eksternal yang berisi map untuk graf");
              Console.WriteLine("File harus berada pada folder yang sama dengan executable");
              Console.Write("Nama input file(.txt): ");
              filename = Console.ReadLine();


              string path =  filename;
              string[] lines = File.ReadAllLines(path, Encoding.UTF8);

              n = int.Parse(lines[0]);

              for(int i = 1; i < n; i++)
              {
                  string[] bits = lines[i].Split(' ');
                  arr.Add(new Node(int.Parse(bits[0]), int.Parse(bits[1])));
                  //arr.Add(new Node(int.Parse(bits[1]), int.Parse(bits[0])));
              }
              List<Node> SortedList = arr.OrderBy(o => o.from).ToList();
              arr = SortedList;
            }

        //DFS untuk Y ke X
        public static void DFS(int from, int to, List<List<int>> grafList, List <int> solusi, List <int> DFSTrack){
              bool found = false;
              bool backtrack = false;
              solusi.Add(from);
              DFSTrack.Add(from);
              DFSUtil(from,to,grafList,solusi,DFSTrack,ref found,ref backtrack);
            }

        //Dibantu dengan utility dari DFS
        public static void DFSUtil(int from, int to, List<List<int>> grafList, List <int> solusi, List <int> DFSTrack,ref bool found,ref bool backtrack){
              if(from == to)
                {
                  found = true;
                }
              else if(grafList[from].Count == 0)
              {
                backtrack = true;
                solusi.RemoveAt(solusi.Count-1);
              }
              else{
                for(int i=0 ; i<grafList[from].Count;i++){
                  if(!found){
                    solusi.Add(grafList[from][i]);
                    DFSTrack.Add(grafList[from][i]);
                    DFSUtil(grafList[from][i],to,grafList,solusi,DFSTrack,ref found,ref backtrack);

                    if(!found && backtrack){
                      DFSTrack.Add(from);
                      backtrack = false;
                    }
                  }
                }
                if(!found)
                {solusi.RemoveAt(solusi.Count-1);}
                backtrack=true;
              }
            }

        //Prosedur untuk display isi List
        public static void display(List <int> someList){
              foreach(int elemen in someList){
                Console.Write(elemen);
                Console.Write(" ");
              }
              Console.WriteLine();
            }

        //Fungsi yang mengakali isi TrackDFS untuk kasus dari daun menuju akar (rumah menuju istana)
        public static List<int> reverseTrack(List <int> someList,int Y, int X){
          List<int> list= new List<int>();
          int i = 0;
          while(someList[i] != X){
            list.Add(someList[i]);
            i++;
          }
          list.Add(X);
          return list;
        }


        //Main Program
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hide and Seek using DFS!! :) ");
            Console.WriteLine("=============================================");
            Console.WriteLine();

            int jumlahRumah = 0;
            //Mmebuat node
            List<Node> arr = new List<Node>();
            //Membuat List of list yang berupa List[i][j] , dimana i adalah no rumah dan j adalah tetangganya
            List<List<int>> house = new List<List<int>>();
            List<int> konstruktor = new List<int>();
            //Solusi adalah path dari Y ke X
            //DFSTrack adalah langkah pengerjaan DFS (masih bermasalah)

            BacaFileGraf(ref arr, ref jumlahRumah); //Dibuat prosedur sendiri agar lebih  compact

            for(int i =0 ; i<jumlahRumah+1 ;i++){
            house.Add(konstruktor);
            konstruktor = new List<int>();
            }

            Console.WriteLine();
            Console.WriteLine("Berdasarkan file, berikut Hubungan antara rumah dalam Negeri Antah Berantah : ");

            foreach(Node no in arr)
            {
               Console.WriteLine("{0} -> {1}", no.getfrom(), no.getto());
               house[no.getfrom()].Add(no.getto());
            }

            foreach(List<int> list in house)
            {
               list.Sort();
            }

            //Bagian membaca query dari file eksternal
            Console.WriteLine();
            Console.WriteLine("Masukkan file eksternal yang berisi query: ");
            string filename;
            Console.WriteLine("File harus berada pada folder yang sama dengan executable");
            Console.Write("Nama query file(.txt): ");
            filename = Console.ReadLine();

            string path =  filename;
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            int maxQuery = int.Parse(lines[0]);

            for(int i = 1; i < maxQuery+1; i++)
            {
                string[] bits = lines[i].Split(' ');
                int code = int.Parse(bits[0]);
                int X = int.Parse(bits[1]);
                int Y = int.Parse(bits[2]);

                List <int> solusi = new List<int>();
                List <int> DFSTrack = new List<int>();

                Console.Write("Query no ");
                Console.Write(i);
                Console.WriteLine(" : ");

                if(code == 0 ){ //Mendekati istana
                  DFS(X,Y,house,solusi,DFSTrack);
                  if(solusi.Count == 0){
                    Console.WriteLine("TIDAK");
                  }
                  else{
                    solusi.Reverse();
                    DFSTrack.Reverse();
                    Console.Write("IYA, rutenya adalah ");
                    display(solusi);
                    List<int> DFSTrackrev = reverseTrack(DFSTrack,Y,X);
                    Console.Write("   Langkah DFSnya : ");
                    display(DFSTrackrev);
                  }
                }

                else if(code == 1 ){ //Menjauhi istana
                  DFS(Y,X,house,solusi,DFSTrack);
                  if(solusi.Count == 0){
                    Console.WriteLine("TIDAK");
                  }
                  else{
                    Console.Write("IYA, rutenya adalah ");
                    display(solusi);
                    Console.Write("   Langkah DFSnya : ");
                    display(DFSTrack);
                  }
                }
            }
          }
      }


  }





            /* = new List<int>();
            List<Node> arr1 = new List<Node>();
            List<Node> arr0 = new List<Node>();

            visited.Add(1);
            for(int i = 0; i < 2 * (n-1); i++)
            {
                if(arr[i].getfrom() == 1)
                {
                    visited.Add(arr[i].getto());
                    arr1.Add(arr[i]);
                }
                else
                {
                    if(arr1.Contains(new Node(arr[i].getto(), arr[i].getfrom())))
                    {
                        arr0.Add(arr[i]);
                    } else if (!visited.Contains(arr[i].getfrom()))
                    {
                        bool exist = false;
                        int j = 0;
                        while(j < visited.Count && !exist)
                        {
                            if (arr.Contains(new Node(visited[j], arr[i].getfrom())) && arr[i].getto() != visited[j])
                            {
                                exist = true;
                            }
                            else
                            {
                                j++;
                            }
                        }
                        if (exist)
                        {
                            arr1.Add(arr[i]);
                        }
                        else
                        {
                            arr0.Add(arr[i]);
                        }
                    }
                    else
                    {
                        arr1.Add(arr[i]);
                        if (!visited.Contains(arr[i].getto()))
                        {
                            visited.Add(arr[i].getto());
                        }
                    }
                }
            }

            Console.WriteLine("Isi arr1: ");
            foreach (Node no in arr1)
            {
                Console.WriteLine("{0} -> {1}", no.getfrom(), no.getto());
            }

            Console.WriteLine("Isi arr0: ");
            foreach (Node no in arr0)
            {
                Console.WriteLine("{0} -> {1}", no.getfrom(), no.getto());
            }*/

            //Console.ReadKey();
