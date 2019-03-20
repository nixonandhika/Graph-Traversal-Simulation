using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Graph_Traversal_Simulation
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private string file1 = ""; //Untuk menyimpan path dari file graf
        private string file2 = ""; //Untuk menyimpan path dari file query
        private bool input1 = false; //boolean sebagai penanda file graf telah diinput
        private bool input2 = false; //boolean sebagai penanda file query telah diinput

        //Setter
        public void setfile1(string name)
        {
            file1 = name;
            input1 = true;
        }

        public void setfile2(string name)
        {
            file2 = name;
            input2 = true;
        }

        //Getter
        public string getfile1()
        {
            return file1;
        }

        public string getfile2()
        {
            return file2;
        }

        public bool getinput1()
        {
            return input1;
        }

        public bool getinput2()
        {
            return input2;
        }

        //Initialize StartWindow
        public StartWindow()
        {
            InitializeComponent();
        }

        //Graph File Browse Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog f = new Microsoft.Win32.OpenFileDialog();

            f.DefaultExt = ".txt";
            f.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = f.ShowDialog();

            if (result == true)
            {
                string filename = f.FileName;
                setfile1(filename);
                GraphBox.Text = filename;
            }
        }

        //Query File Browse Button
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog f = new Microsoft.Win32.OpenFileDialog();

            f.DefaultExt = ".txt";
            f.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = f.ShowDialog();

            if (result == true)
            {
                string filename = f.FileName;
                setfile2(filename);
                QueryBox.Text = filename;
            }
        }

        //Start Button
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (QueryButton.IsEnabled) //Jika menggunakan query file
            {
                if (this.getinput1() && this.getinput2())
                {
                    //Membuka MainWindow dengan file query
                    MainWindow graphwindow = new MainWindow(this.getfile1(), this.getfile2()); 
                    graphwindow.Show();
                }
                else
                {
                    MessageBox.Show("Please Insert Both Graph and Query File Before Starting");
                }
            }
            else //Jika tidak menggunakan query file
            {
                if (this.getinput1())
                {
                    //Membuka MainWindow tanpa file query
                    MainWindow graphWindow = new MainWindow(this.getfile1(), ""); 
                    graphWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please Insert Graph File Before Starting");
                }
            }
        }

        //Exit Button
        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //Event Handler untuk CheckBox penggunaan query file
        private void QueryEnabled(object sender, RoutedEventArgs e)
        {
            QueryButton.IsEnabled = true;
        }

        private void QueryDisabled(object sender, RoutedEventArgs e)
        {
            QueryButton.IsEnabled = false;
            QueryBox.Text = "";
            input2 = false;
        }
    }
}
