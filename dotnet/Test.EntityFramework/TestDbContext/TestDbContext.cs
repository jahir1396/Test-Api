using Microsoft.EntityFrameworkCore;
using Sistema.Proyecto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test.EntityFramework.TestDbContext
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {

        }
        public TestDbContext(DbContextOptions<TestDbContext> options): base(options)
        {

        }

        public DbSet<CatArticulo> CatArticulo { get; set; }
        public DbSet<CatCategoria> CatCategoria { get; set; }
        public DbSet<CatEstatusVentaIngreso> CatEstatusVentaIngreso { get; set; }
        public DbSet<CatPersona> CatPersona { get; set; }
        public DbSet<CatParametro> CatParametro { get; set; }
        public DbSet<CatRol> CatRol { get; set; }
        public DbSet<CatTipoPersona> CatTipoPersona { get; set; }
        public DbSet<CatUsuario> CatUsuario { get; set; }
        public DbSet<OpeDetalleIngreso> OpeDetalleIngreso { get; set; }
        public DbSet<OpeDetalleVenta> OpeDetalleVenta { get; set; }
        public DbSet<OpeIngreso> OpeIngreso { get; set; }
        public DbSet<OpeVenta> OpeVenta { get; set; }
        public DbSet<OpeVentaTracking> OpeVentaTracking { get; set; }
        public DbSet<CatPlantillaCorreo> CatPlantillaCorreo { get; set; }
        public DbSet<OpeNotificacion> OpeNotificacion { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CatUsuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
            builder.Entity<CatUsuario>()
                .HasIndex(u => u.FolioUsuario)
                .IsUnique();
            builder.Entity<CatPersona>()
                .HasIndex(u => u.Email)
                .IsUnique();
            //Ejemplo de relaciones muchos a muchos
            //modelBuilder.Entity<CatFirmaSociedad>()
            //    .HasMany(e => e.Firmas)
            //    .WithMany(e => e.Sociedades)
            //    .Map(m => m.ToTable("OpeFirmaSociedad").MapLeftKey("IdSociedad").MapRightKey("IdFirma"));
        }
    }
}
