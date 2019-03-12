using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string filename;
            Console.WriteLine("File harus berada pada folder yang sama dengan executable");
            Console.Write("Nama input file: ");
            filename = Console.ReadLine();
            List<Node> arr = new List<Node>();

            //var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory.ToString(), filename);
            //Console.WriteLine(path);
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "\\" + filename);
            string path = @"C:\Users\nixon\Desktop\" + filename;

            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            int n = int.Parse(lines[0]);

            for(int i = 1; i < n; i++)
            {
                string[] bits = lines[i].Split(' ');
                arr.Add(new Node(int.Parse(bits[0]), int.Parse(bits[1])));
                arr.Add(new Node(int.Parse(bits[1]), int.Parse(bits[0])));
            }
            List<Node> SortedList = arr.OrderBy(o => o.from).ToList();
            arr = SortedList;

            Console.WriteLine("Isi arr: ");
            foreach(Node no in arr)
            {
                Console.WriteLine("{0} -> {1}", no.getfrom(), no.getto());
            }

            List<int> visited = new List<int>();
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
            }

            Console.ReadKey();

        }
    }

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
}
