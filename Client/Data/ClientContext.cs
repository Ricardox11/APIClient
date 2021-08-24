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
    
    // modelo SP EF
     public DbSet<AppClient.Models.Client> ClientSp { get; set; }

    // EF scalar
    public DbSet<AppClient.Models.ClientScalar> ScalarIntValue { get; set; }


    // indica a EF que no se tiene llave primaria
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppClient.Models.ClientScalar>().HasNoKey();
    }

}
