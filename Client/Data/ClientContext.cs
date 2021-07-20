using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppClient.Models;

    public class ClientContext : DbContext
    {
        public ClientContext (DbContextOptions<ClientContext> options)
            : base(options)
        {

        // creacion de BD automatica

        //  Database.EnsureDeleted(); // elimina la BD



       Database.EnsureCreated(); // solucion a error Crea BD si no existe

        // si hay cambios en Modelo Reconstruye la  BD - Desarrollo
        // produccion usar migrate


    }

    public DbSet<AppClient.Models.Client> Client { get; set; }
    }
