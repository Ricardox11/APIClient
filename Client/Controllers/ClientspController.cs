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

namespace AppClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientspController : ControllerBase
    {
        private readonly ClientRepository _context; // usa el repository de client

        public string connectionString;



        public ClientspController(ClientRepository context)
        {
            _context = context; // DI del repository
        
        }

        // GET: api/Clientsp
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> Get()
        {

            return await _context.GetAll(); // llama funcion getall del repository
        }

        // GET: api/Clientsp/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> Get(Guid id)
        {
            var response = await _context.GetById(id);

            if(response == null)
            {
                return NotFound();
            }


            return response;
        }



        // POST: api/Clientsp
        [HttpPost]
        public async Task Post([FromBody] Client client)
        {
            await _context.Insert(client); // insertar con repository
        }


        // PUT: api/Clientsp
        [HttpPut]
        public async Task Put([FromBody] Client client)
        {
            await _context.Update(client); // insertar con repository
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _context.Delete(id);
        }



    }

}