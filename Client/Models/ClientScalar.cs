using System;
namespace AppClient.Models
{

    // clase para consultar el SP exec CountClient
    // retorna un scalar

    public class ClientScalar
    {
        public int Value { get; set; } // value nombre de la respuesta del SP

    

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
