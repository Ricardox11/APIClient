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

namespace AppClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientContext _context;

        public string connectionString;




        public ClientController(ClientContext context)
        {
            _context = context;
            // prueba log
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient()
        {

            // Manejo log manual 
            // connectionString =  ConfigurationApp.Configuration["ConnectionStrings:ClientContext"];

            // configuracion sql
            // var sqlLoggerOptions = new MSSqlServerSinkOptions
            // {
            //    AutoCreateSqlTable = true,
            //    SchemaName = "Logger",
            //    TableName = "Logs",
            //    BatchPostingLimit = 1
            // };


            //  log manual 

            // var logger = new LoggerConfiguration()
            //   .MinimumLevel.Debug()
            // .WriteTo.Console(LogEventLevel.Information)
            // .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
            // .WriteTo.MSSqlServer(connectionString, sqlLoggerOptions) 
            //.CreateLogger();

            // log por appsettings.json

            // cargar config
            //  var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // crear log con config
            //var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();


            //logger.Verbose("Mensaje Verbose");
            //logger.Debug("Mensaje Debug");
            //logger.Information("Mensaje Information");
            //logger.Warning("Mensaje Warning");
            //logger.Error("Mensaje Error");
            //logger.Fatal("Mensaje Fatal");


            //Request
            //var requestmsj = "Request Get " + JsonSerializer.Serialize(Client);

            // Response
            List<AppClient.Models.Client> lista = _context.Client.ToList();
            var responsemsj = "Response Get " + JsonSerializer.Serialize(lista);
            LogApp.WriteLog(responsemsj, "Debug", "TEXT");

            return await _context.Client.ToListAsync();
        }

        // GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(Guid id)
        {

            //Request
            var requestmsj = "Request Get Id " + JsonSerializer.Serialize(id);
            LogApp.WriteLog(requestmsj, "Debug", "TEXT");

            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                // Error
                var error = "Error Get Id " + JsonSerializer.Serialize(NotFound());
                LogApp.WriteLog(error, "Error", "TEXT");

                return NotFound();
            }

            // Response
            var responsemsj = "Response Get Id " + JsonSerializer.Serialize(client);
            LogApp.WriteLog(responsemsj, "Debug", "TEXT");


            return client;
        }

        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(string id, Client client)
        {

            //Request
            var requestmsj = "Request PutClient Id - Client " + id + " "+ JsonSerializer.Serialize(client);
            LogApp.WriteLog(requestmsj, "Debug", "TEXT");

            if (id != client.ClientId.ToString())
            {
                var error = "Error PutClient " + JsonSerializer.Serialize(BadRequest());
                LogApp.WriteLog(error, "Error", "TEXT");

                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ClientExists(id))
                {
                    var error2 = "Error PutClient " + JsonSerializer.Serialize(NotFound());
                    LogApp.WriteLog(error2, "Error", "TEXT");
                    return NotFound();
                }
                else
                {
                    var error3 = "Error PutClient " + JsonSerializer.Serialize(e);
                    LogApp.WriteLog(error3, "Error", "TEXT");
                    return NotFound();
                    throw;
                }
            }

            // Response
            var responsemsj = "Response Put Id " + JsonSerializer.Serialize(NoContent());
            LogApp.WriteLog(responsemsj, "Debug", "TEXT");


            return NoContent();
        }

        // POST: api/Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {

            //Request
            var requestmsj = "Request Post " + JsonSerializer.Serialize(client);
            LogApp.WriteLog(requestmsj, "Debug", "TEXT");



            _context.Client.Add(client);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (ClientExists(client.ClientId.ToString()))
                {
                    var error = "Error PostClient " + JsonSerializer.Serialize(Conflict());
                    LogApp.WriteLog(error, "Error", "TEXT");

                    return Conflict();
                }
                else
                {
                    var error2 = "Error PostClient " + JsonSerializer.Serialize(e);
                    LogApp.WriteLog(error2, "Error", "TEXT");
                    throw;
                }
            }

            // Response
            var responsemsj = "Response Post " + JsonSerializer.Serialize(client);
            LogApp.WriteLog(responsemsj, "Debug", "TEXT");


            return CreatedAtAction("GetClient", new { id = client.ClientId }, client);
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {


            //Request
            var requestmsj = "Request DeleteClient " + JsonSerializer.Serialize(id);
            LogApp.WriteLog(requestmsj, "Debug", "TEXT");

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                var error = "Error DeleteClient " + JsonSerializer.Serialize(NotFound());
                LogApp.WriteLog(error, "Error", "TEXT");

                return NotFound();
            }

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            // Response
            var responsemsj = "Response DeleteClient " + JsonSerializer.Serialize(NoContent());
            LogApp.WriteLog(responsemsj, "Debug", "TEXT");


            return NoContent();
        }

        private bool ClientExists(string id)
        {
            return _context.Client.Any(e => e.ClientId.ToString() == id);
        }
    }
}
