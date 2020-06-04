using Modeln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ClientesDALC
    {
        public List<Cliente> ListarClientes()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Clientes.ToList();
            }
        }
    }
}
