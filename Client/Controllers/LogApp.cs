using System;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json;
using System.Collections.Generic;


// funcion para manejar el log de la aplicacion

// WriteLog("Mensaje", "Information", "JSON"); Mensaje JSON para seguimiento de request y response
// WriteLog("Mensaje", "Information", "TEXT"); Mensaje TEXT Informativos o errores
// IEnumerable<Object> Objetcd - trama a escribir o serializar


namespace AppClient.Controllers
{
    public static class LogApp
    {


        public static void WriteLog(string message, string LogType, string DataType, Object Objetcd )
        {
            // cargar config del log
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // crear log con config
            var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            //menssage
            if (Objetcd != null)
            {
                message = message + ' ' + JsonSerializer.Serialize(Objetcd);

            }
           

            // verifica el tipo de log que se requiere


            // convierte en Json
            if (DataType == "JSON" && LogType != "Information" )
            {
                message = JsonSerializer.Serialize(message);
            }

            // mensajes informativos
            if (LogType == "Information")
            {
                logger.Information(message);
            }

            // mensajes debug
            if (LogType == "Debug")
            {
                logger.Debug(message);
            }

            // mensajes seguimiento errores
            if (LogType == "Error")
            {
                logger.Error(message);
                logger.Fatal(message);
            }

            // appsetings.json define en donde mostrar error file, console o DB


            //logger.Verbose("Mensaje Verbose");
            //logger.Debug("Mensaje Debug");
            //logger.Information("Mensaje Information");
            //logger.Warning("Mensaje Warning");
            //logger.Error("Mensaje Error");
            //logger.Fatal("Mensaje Fatal");
            //logger.Information(message);

            // var json = JsonSerializer.Serialize(message);


        }


    }
}
