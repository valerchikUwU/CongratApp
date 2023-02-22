using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp3
{
    public class ApplicationContext : DbContext //класс контекста данных
    {
        public DbSet<Person> Persons { get; set; } //набор объектов, которые хранятся в бд

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        // устанавливает параметры подключения 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
    }
}