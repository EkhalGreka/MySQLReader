using System;
using System.Collections.Generic;
using System.IO;
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

namespace MySQLReader
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public Connector connector = new Connector();
        public ConnectWindow()
        {

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.connector.Login = Login.Text;
            this.connector.Password = Password.Password;
            this.connector.Server = Server.Text;
            this.connector.Port = Port.Text;
            this.connector.Database = DB.Text;
            SaveInfo();
            this.Close();
        }

        public void GetSavedInfo()
        {
            if (File.Exists("savedbase.txt"))
            {
                using (var reader = new StreamReader("savedbase.txt"))
                {
                    Login.Text = reader.ReadLine();
                    Password.Password = reader.ReadLine();
                    Server.Text = reader.ReadLine();
                    Port.Text = reader.ReadLine();
                    DB.Text = reader.ReadLine();
                }
            }
        }
        public void SaveInfo()
        {

            using (var writer = new StreamWriter("savedbase.txt", false))
            {
                writer.WriteLine(Login.Text);
                writer.WriteLine(Password.Password);
                writer.WriteLine(Server.Text);
                writer.WriteLine(Port.Text);
                writer.WriteLine(DB.Text);
            }

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            GetSavedInfo();
        }
    }
    public class Connector
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public bool IsConnected { get; set; }
    }

}
