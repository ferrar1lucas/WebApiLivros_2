using Microsoft.EntityFrameworkCore;
using WebApiLivros_2.Models;

namespace WebApiLivros_2.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Livro> Livros { get; set; }

        public DbSet<Autor> Autores { get; set; }
    }
}
