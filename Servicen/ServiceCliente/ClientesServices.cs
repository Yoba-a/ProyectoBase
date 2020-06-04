using Modeln;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicen.ServiceCliente
{
    public class ClientesServices
    {

        private static ClientesDALC obj = new ClientesDALC();
        public static List<Cliente> ListarClientes()
        {
                return obj.ListarClientes();
        }
    }
}
