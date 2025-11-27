using FinancieraCreditoApi.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;



namespace FinancieraCreditoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<SolicitudCredito> SolicitudCredito => Set<SolicitudCredito>();
        public DbSet<RespuestaCreditoFinanciera> RespuestaCreditoFinanciera => Set<RespuestaCreditoFinanciera>();
        public DbSet<NotificacionAsesor> NotificacionAsesor => Set<NotificacionAsesor>();
        public DbSet<ParametroFinancieraTiempoReconsulta> ParametrosReconsulta => Set<ParametroFinancieraTiempoReconsulta>();

        public DbSet<Asesor> Asesor => Set<Asesor>();
        public DbSet<Financiera> Financiera => Set<Financiera>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //tablas
            modelBuilder.Entity<SolicitudCredito>().ToTable("SolicitudCredito");
            modelBuilder.Entity<RespuestaCreditoFinanciera>().ToTable("RespuestaCreditoFinanciera");
            modelBuilder.Entity<NotificacionAsesor>().ToTable("NotificacionAsesor");
            modelBuilder.Entity<ParametroFinancieraTiempoReconsulta>().ToTable("ParametroFinancieraTiempoReconsulta");
            modelBuilder.Entity<Asesor>().ToTable("Asesor");
            modelBuilder.Entity<Financiera>().ToTable("Financiera");

            // Índices
            modelBuilder.Entity<SolicitudCredito>()
                .HasIndex(s => s.NumeroSolicitud)
                .IsUnique();

            modelBuilder.Entity<RespuestaCreditoFinanciera>()
                .HasIndex(r => r.NumeroSolicitud);

            modelBuilder.Entity<RespuestaCreditoFinanciera>()
                .Property(r => r.MontoAprobado)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RespuestaCreditoFinanciera>()
                 .Property(r => r.Tasa)
                 .HasColumnType("decimal(5,2)");
        }
    }

}
