using System;
using Microsoft.Data.SqlClient;

namespace AppClient.Models
{
    public class Client
    {

        // atributos

        public Guid ClientId { get; set; }
        public string TipDocument { get; set; }
        public string Document { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string LName { get; set; }
        

        public Client()
        {
        }

        public static explicit operator Client(SqlDataReader v)
        {
            throw new NotImplementedException();
        }
    }
}
