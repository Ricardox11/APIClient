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

    // SP - clase para manejo de clientes - implememntacion SP
    // conexion DAO para consumir SP



    public class ClientRepository
    {


       // private readonly ClientContext _context;

        public string connectionString;


        


        public async Task<List<Client>> GetAll()
        {


            // recupera cadena de conexion
            connectionString = ConfigurationApp.Configuration["ConnectionStrings:ClientContext"];

            using (SqlConnection sql = new SqlConnection(connectionString)) // crea conexion
            {
                using (SqlCommand cmd = new SqlCommand("GetAll", sql)) // define SP a ejecutar
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; // define tipo

                    var response = new List<Client>(); // define objeto donde guardar los datos

                    await sql.OpenAsync(); // abre conexion

                    using (var reader = await cmd.ExecuteReaderAsync()) // ejecuta comando
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader)); // adiciona respuesta a objeto
                        }
                    }

                    var responsemsj = "Response Get SP " + cmd.CommandText ; 
                    LogApp.WriteLog(responsemsj, "Debug", "TEXT", response);


                    return response;

                }
            }

            //throw new NotImplementedException();

        }

        public async Task<Client> GetById(Guid id)
        {
            // recupera cadena de conexion
            connectionString = ConfigurationApp.Configuration["ConnectionStrings:ClientContext"];

            using (SqlConnection sql = new SqlConnection(connectionString)) // crea conexion
            {
                using (SqlCommand cmd = new SqlCommand("GetClientById", sql)) // define SP a ejecutar
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; // define tipo

                    cmd.Parameters.Add(new SqlParameter("@ClientId", id)); // parametro

                    var response = new Client(); // define objeto donde guardar los datos

                    await sql.OpenAsync(); // abre conexion

                    using (var reader = await cmd.ExecuteReaderAsync()) // ejecuta comando
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader); // adiciona respuesta a objeto
                        }
                    }

                    var responsemsj = "Response Get ID  SP " + cmd.CommandText + " " + cmd.Parameters[0].Value;
                    LogApp.WriteLog(responsemsj, "Debug", "TEXT", response);

                    return response;

                }
            }

            //throw new NotImplementedException();

        }

        private Client MapToValue(SqlDataReader reader)
        {

            // convierte respuesta reader en objeto
            return new Client()
            {
                ClientId = (Guid)reader["ClientId"],
                TipDocument = reader["TipDocument"].ToString(),
                Document = reader["Document"].ToString(),
                FName = reader["FName"].ToString(),
                SName = reader["SName"].ToString(),
                LName = reader["LName"].ToString(),

            };
        }

        public async Task Insert(Client client)
        {

            var requestmsj = "Request Insert SP " ;
            LogApp.WriteLog(requestmsj, "Debug", "TEXT", client);



            // recupera cadena de conexion
            connectionString = ConfigurationApp.Configuration["ConnectionStrings:ClientContext"];

            using (SqlConnection sql = new SqlConnection(connectionString)) // crea conexion
            {
                using (SqlCommand cmd = new SqlCommand("InsertClient", sql)) // crea instancia de SP 
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; // tipo SP

                    // cargar valores para SP

                    cmd.Parameters.Add(new SqlParameter("@ClientId", Guid.NewGuid()));
                    cmd.Parameters.Add(new SqlParameter("@TipDocument", client.TipDocument));
                    cmd.Parameters.Add(new SqlParameter("@Document", client.Document));
                    cmd.Parameters.Add(new SqlParameter("@FName", client.FName));
                    cmd.Parameters.Add(new SqlParameter("@SName", client.SName));
                    cmd.Parameters.Add(new SqlParameter("@LName", client.LName));
                


                    await sql.OpenAsync(); // abre conexion

                    await cmd.ExecuteNonQueryAsync(); // ejecuta proceso

                    List<string> listaSp = new List<string>();

                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        listaSp.Add(cmd.Parameters[i].Value.ToString());

                    }

                    var responsemsj = "Response Insert SP " + cmd.CommandText + " Parametros ";
                    LogApp.WriteLog(responsemsj, "Debug", "TEXT", listaSp);



                    return;
                }
            }
        }



        public async Task Update(Client client)
        {

            var requestmsj = "Request Update SP ";
            LogApp.WriteLog(requestmsj, "Debug", "TEXT", client);

            // recupera cadena de conexion
            connectionString = ConfigurationApp.Configuration["ConnectionStrings:ClientContext"];

            using (SqlConnection sql = new SqlConnection(connectionString)) // crea conexion
            {
                using (SqlCommand cmd = new SqlCommand("UpdateClient", sql)) // crea instancia de SP 
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; // tipo SP

                    // cargar valores para SP

                    cmd.Parameters.Add(new SqlParameter("@ClientId", client.ClientId));
                    cmd.Parameters.Add(new SqlParameter("@TipDocument", client.TipDocument));
                    cmd.Parameters.Add(new SqlParameter("@Document", client.Document));
                    cmd.Parameters.Add(new SqlParameter("@FName", client.FName));
                    cmd.Parameters.Add(new SqlParameter("@SName", client.SName));
                    cmd.Parameters.Add(new SqlParameter("@LName", client.LName));



                    await sql.OpenAsync(); // abre conexion

                    await cmd.ExecuteNonQueryAsync(); // ejecuta proceso

                    List<string> listaSp = new List<string>();

                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        listaSp.Add(cmd.Parameters[i].Value.ToString());
                    }

                    var responsemsj = "Response Update SP " + cmd.CommandText + " Parametros ";
                    LogApp.WriteLog(responsemsj, "Debug", "TEXT", listaSp);




                    return;
                }
            }

        }


        public async Task Delete(Guid Id)
        {

            // recupera cadena de conexion
            connectionString = ConfigurationApp.Configuration["ConnectionStrings:ClientContext"];


            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteClient", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClientId", Id));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    var responsemsj = "Response Get ID  SP " + cmd.CommandText + " " + cmd.Parameters[0].Value;
                    LogApp.WriteLog(responsemsj, "Debug", "TEXT", null);


                    return;
                }
            }
        }





    }
}
