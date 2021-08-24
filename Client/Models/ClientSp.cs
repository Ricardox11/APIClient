using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace AppClient.Models
{
    // modelo para uso de SP con EF


    // definir annotation para SP EF
    [Table("Client", Schema = "dbo")]
    public partial class ClientSp
    {

        // atributos

        public Guid ClientId { get; set; }
        public string TipDocument { get; set; }
        public string Document { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string LName { get; set; }


        public ClientSp()
        {
        }

        public static explicit operator ClientSp(SqlDataReader v)
        {
            throw new NotImplementedException();
        }

        // depurador
        public override string ToString()
        {
            return FName;
        }
    }
}
