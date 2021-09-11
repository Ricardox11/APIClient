using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppClient.Models;

public class ClientContext : DbContext
{
    public ClientContext(DbContextOptions<ClientContext> options)
        : base(options)
    {

        // creacion de BD automatica

        // Database.EnsureDeleted(); // elimina la BD



       Database.EnsureCreated(); // solucion a error Crea BD si no existe

        // si hay cambios en Modelo Reconstruye la  BD - Desarrollo
        // produccion usar migrate


    }

    public DbSet<AppClient.Models.Client> Client { get; set; }

    // modelo SP EF
    public DbSet<AppClient.Models.Client> ClientSp { get; set; }

    // EF scalar
    public DbSet<AppClient.Models.ClientScalar> ScalarIntValue { get; set; }

    // master detail definir context
    public DbSet<AppClient.Models.Project> Projects { get; set; }
    public DbSet<AppClient.Models.Branch> Branches { get; set; }
    public DbSet<AppClient.Models.ProjectBranch> ProjectBranches { get; set; }




    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // indica a EF que no se tiene llave primaria
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppClient.Models.ClientScalar>().HasNoKey();

        // indica llave primaria compuesta
        modelBuilder.Entity<ProjectBranch>().HasKey(x => new { x.ProjectId, x.BranchId });
    }

}
