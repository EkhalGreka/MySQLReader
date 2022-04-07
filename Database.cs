using System.Collections.Generic;


namespace MySQLReader
{
    public class Database
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; } = new List<Table>();

        public class Table
        {
            public string Name { get; set; }

            public List<string> Columns { get; set; } = new List<string>();
            public List<Entry> Entries { get; set; } = new List<Entry>();
            public class Entry
            {
                public List<Element> Elements { get; set; } = new List<Element>();
                public class Element
                {
                    public string Type { get; set; }
                    public string Data { get; set; }
                }
            }
        }
    }
}
