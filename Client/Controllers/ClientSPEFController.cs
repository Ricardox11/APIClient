using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppClient.Models;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Collections;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.Common;


// controlador para manejo de SP con EF

namespace AppClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientSPEFController : ControllerBase
    {
        // adiciona el contexto de startup
        private readonly ClientContext _context;
        private readonly ILogger<ClientSPEFController> _logger;

        // servicios statup
        // instancia automatica por servicio
        public ClientSPEFController(ILogger<ClientSPEFController> logger, ClientContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Client
        [HttpGet]
        public ActionResult<List<Client>> GetClient()
        {
            List<Client> list; // variable
            string sql = "EXEC dbo.GetAll"; // comando a ejecutar
            list = _context.ClientSp.FromSqlRaw<Client>(sql).ToList(); // context ClientSp ejecuta - asigna cada fila a un client y lo convierte eb list
                                                                       // Debugger.Break(); // ver datos devueltos
            return list;
        }


        // GET: api/Client/5
        [HttpGet("{id}")]
        public ActionResult<List<Client>> GetClient(Guid id)
        {

            //Request
            var requestmsj = "Request Get Id "; // + JsonSerializer.Serialize(id);
            LogApp.WriteLog(requestmsj, "Debug", "TEXT", null);

            List<Client> list; // variable
            string sql = "EXEC GetClientById  @ClientId"; // consulta SP

            List<SqlParameter> parms = new List<SqlParameter> // lista de parametros
            {
                  // Create parameter(s)    
                   new SqlParameter { ParameterName = "@ClientId", Value = id }
            };

            list = _context.ClientSp.FromSqlRaw<Client>(sql, parms.ToArray()).ToList(); // ejecuta consulta

            //            Debugger.Break();



            // Response
            var responsemsj = "Response Get Id "; // + JsonSerializer.Serialize(client);
            LogApp.WriteLog(responsemsj, "Debug", "TEXT", list);


            return list;
        }

        // GET: api/Client
        [Route("count")]
        [HttpGet]
        public ActionResult<ClientScalar> GetCount()
        {
            ClientScalar value;
            string sql = "exec CountClient";
            value = _context.ScalarIntValue.FromSqlRaw<ClientScalar>(sql).AsEnumerable().FirstOrDefault();
            // Debugger.Break();

            return value;
        }


        // PUT: api/Clientsp
        [HttpPut]
        public  void  PutClient(Client client)
        {
            var requestmsj = "Request Update SP ";
            LogApp.WriteLog(requestmsj, "Debug", "TEXT", client);

            int rowsAffected;
            string sql = "EXEC UpdateClient @ClientId, @TipDocument, @Document , @FName, @SName, LName"; // sentencia

            List<SqlParameter> parms = new List<SqlParameter>
            { 
             // Cargar parametros

            new SqlParameter { ParameterName = "@ClientId", Value = client.ClientId},
            new SqlParameter { ParameterName = "@TipDocument", Value = client.TipDocument },
            new SqlParameter { ParameterName = "@Document", Value = client.Document },
            new SqlParameter { ParameterName = "@FName", Value = client.FName },
            new SqlParameter { ParameterName = "@SName", Value = client.SName },
            new SqlParameter { ParameterName = "@LName", Value = client.LName }

            };

            rowsAffected = _context.Database.ExecuteSqlRaw(sql, parms.ToArray());

            // Debugger.Break();
           
            //LogApp.WriteLog(responsemsj, "Debug", "TEXT", listaSp);




            return;
        }



        // GET: api/Client
        [Route("multiget")]
        [HttpGet]
        public ActionResult<List<Client>> GetMulticlient()
        {
            // listas para multiples resultados
            List<Client> CC = new List<Client>();
            List<Client> CE = new List<Client>();

            DbCommand cmd;
            DbDataReader rdr;

            // senetencia sql
            string sql = "EXEC GetMultiClient";

            // Build command object  
            cmd = _context.Database.GetDbConnection().CreateCommand(); // objeto database ADO
            cmd.CommandText = sql; // asocia el sql

            // Open database connection  
            _context.Database.OpenConnection();

            // Create a DataReader  
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // ejecuta

            // Build collection 1
            while (rdr.Read()) // leer resultado
            {
                CE.Add(new Client
                {
                    ClientId = rdr.GetGuid(0),
                    TipDocument = rdr.GetString(1),
                    Document = rdr.GetString(2)
                });
            }

            // Advance to the next result set  
            rdr.NextResult();

            // Build collection 2 
            while (rdr.Read())
            {
                CC.Add(new Client
                {
                    ClientId = rdr.GetGuid(0),
                    TipDocument = rdr.GetString(1),
                    Document = rdr.GetString(2)
                });
            }

           // Debugger.Break();

            // Close Reader and Database Connection  
            rdr.Close(); // cerrar conexion de BD

            CC.AddRange(CE);

            return CC;
        }



    }





}
