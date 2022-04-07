using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;

namespace MySQLReader
{
    public partial class MainWindow : Window
    {
        public List<Database> Databases { get; set; } = new List<Database>();
        public List<List<Table>> TablesList { get; set; } = new List<List<Table>>();
        

        public MainWindow()
        {
            var cw = new ConnectWindow();
            cw.ShowDialog();
            
            InitializeComponent();
            Title = $"База данных {cw.connector.Database}";
            var MySQLBase = new MySQL();
            IDataBase zalupable;
            zalupable = MySQLBase;
            getData(MySQLBase, cw.connector);

        }

        public async Task getData(IDataBase BaseType, Connector connector)
        {
            
            Databases = await BaseType.UseSql(Databases, connector);
            if (connector.IsConnected)
            {
                int tablesCounter = 0;
                foreach (var table in Databases[0].Tables)
                {

                    TablesList.Add(new List<Table>());
                    var currentGrid = new DataGrid()
                    {
                        AutoGenerateColumns = false
                    };

                    int entriesCounter = 0;
                    foreach (var entry in table.Entries)
                    {
                        var tab = new Table();
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            if (entriesCounter == 0)
                            {
                                currentGrid.Columns.Add(new DataGridTextColumn()
                                {
                                    Header = table.Columns[i],
                                    Width = new DataGridLength(),
                                    Binding = new Binding($"E[{i}]")
                                });
                            }
                            tab.E.Add(entry.Elements[i].Data);
                        }
                        entriesCounter++;

                        TablesList[tablesCounter].Add(tab);


                    }
                    currentGrid.ItemsSource = TablesList[tablesCounter];
                    tablesCounter++;

                    aboba.Items.Add(new TabItem()
                    {
                        Header = table.Name,
                        Content = currentGrid
                    });
                }
                
            }
            else MessageBox.Show("Ошибка подключения к базе данных");


        }
        public List<Table> getDataForTable(List<Database> databases)
        {
            List<Table> table = new List<Table>();

            foreach (var entry in databases[0].Tables[0].Entries)
            {
                var tab = new Table();
                for (int i = 0; i < 2; i++)
                {
                    //switch (i)
                    //{
                    //    case 0:
                    //        tab.E1 = entry.Elements[0].Data;
                    //        break;
                    //    case 1:
                    //        tab.E2 = entry.Elements[1].Data;
                    //        break;
                    //    case 2:
                    //        tab.E3 = entry.Elements[2].Data;
                    //        break;
                    //    case 3:
                    //        tab.E4 = entry.Elements[3].Data;
                    //        break;
                    //}

                }
                table.Add(tab);
            }
            return table;
        }


    }
    public class Table
    {
        public List<string> E { get; set; } = new List<string>();
    }

}
