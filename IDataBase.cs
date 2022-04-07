using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySQLReader
{
    public interface IDataBase
    {
        public Task<List<Database>> UseSql(List<Database> databases, Connector connector);
    }
}
