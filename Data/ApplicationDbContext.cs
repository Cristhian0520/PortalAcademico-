using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlataformaAcademicaApp.Models;
using System;

namespace PlataformaAcademicaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Curso: código único
            builder.Entity<Curso>()
                   .HasIndex(c => c.Codigo)
                   .IsUnique();

            // Matricula: combinación CursoId + UsuarioId única
            builder.Entity<Matricula>()
                   .HasIndex(m => new { m.CursoId, m.UsuarioId })
                   .IsUnique();

            // TimeSpan -> string converter para SQLite ("HH:mm")
            var timeSpanConverter = new ValueConverter<TimeSpan, string>(
                v => v.ToString(@"hh\:mm"), // nota la corrección del formato
                v => TimeSpan.Parse(v));

            builder.Entity<Curso>().Property(c => c.HorarioInicio).HasConversion(timeSpanConverter);
            builder.Entity<Curso>().Property(c => c.HorarioFin).HasConversion(timeSpanConverter);
        }
    }
}
