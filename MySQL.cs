using MySqlConnector;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MySQLReader
{
    public class MySQL : IDataBase
    {
        public async Task<List<Database>> UseSql(List<Database> databases, Connector connector)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                //Server = "151.248.125.40",
                //Port = 3306,
                //UserID = "amogus",
                //Password = "12345",
                Server = connector.Server,
                Port = Convert.ToUInt32(connector.Port),
                UserID = connector.Login,
                Password = connector.Password,
                Database = connector.Database,
            };
            using (var connection = new MySqlConnection(builder.ConnectionString))
            {

                try
                { 
                    await connection.OpenAsync(); 
                }
                catch
                {

                }
                connector.IsConnected = (connection.State == System.Data.ConnectionState.Open);
                if (connector.IsConnected)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SHOW DATABASES;";
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            //while (await reader.ReadAsync())
                            //{
                            await reader.ReadAsync();
                            databases.Add(new Database() { Name = reader.GetString(0) });
                            // }


                        }
                    }
                    for (int i = 0; i < databases.Count(); i++)
                    //for (int i = 1; i < 2; i++)
                    {
                        databases[i] = await GetDatabase(connection, databases[i]);
                    }
                }
                
            }
            return databases;
        }
        public async Task<Database> GetDatabase(MySqlConnection connection, Database database)
        {

            //using (var command = connection.CreateCommand())
            //{
            //    command.CommandText = $"USE {database.Name};";
            //    await command.ExecuteNonQueryAsync();
            //}
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SHOW TABLES;";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    int tablesCounter = 0;
                    while (await reader.ReadAsync())
                    {
                        //if (tablesCounter == 0) tablesCounter++; //КОСТЫЛЬ
                        //else
                        database.Tables.Add(new Database.Table() { Name = reader.GetString(0) });
                    }
                }
            }
            for (int i = 0; i < database.Tables.Count(); i++)
            {
                database.Tables[i] = await GetTable(connection, database.Tables[i]);
            }
            return database;
        }
        public async Task<Database.Table> GetTable(MySqlConnection connection, Database.Table table)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {table.Name};";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    foreach (var aboba in reader.GetColumnSchema())
                    {
                        table.Columns.Add(aboba.ColumnName);
                    }
                    while (await reader.ReadAsync())
                    {
                        var entry = new Database.Table.Entry();
                        for (int i = 0; i < table.Columns.Count(); i++)
                        {
                            string type = reader.GetDataTypeName(i);
                            if (type.Contains("CHAR")) type = "CHAR";
                            switch (type)
                            {
                                case "CHAR":
                                //entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetChar(i).ToString() });
                                //break;
                                case "VARCHAR":
                                case "TINYTEXT":
                                case "TEXT":
                                case "BLOB":
                                case "MEDIUMTEXT":
                                case "MEDIUMBLOB":
                                case "LONGTEXT":
                                case "LONGBLOB":
                                case "ENUM":
                                case "SET":
                                    entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetString(i).ToString() });
                                    
                                    break;
                                case "TINYINT":
                                case "SMALLINT":
                                case "MEDIUMINT":
                                case "INT":
                                    entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetInt32(i).ToString() });
                                    break;
                                case "BIGINT":
                                    entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetInt64(i).ToString() });
                                    break;
                                case "FLOAT":
                                    entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetFloat(i).ToString() });
                                    break;
                                case "DOUBLE":
                                    entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetDouble(i).ToString() });
                                    break;
                                case "DECIMAL":
                                    entry.Elements.Add(new Database.Table.Entry.Element() { Type = type, Data = reader.GetDecimal(i).ToString() });
                                    break;
                            }
                        }
                        table.Entries.Add(entry);
                    }
                }
            }
            return table;
        }
    }
}


