using Microsoft.EntityFrameworkCore;
using SettingsNetComponent;

namespace SettingsStorage.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {
            Database.EnsureCreated(); // создаем БД если отсутствует
        }

        public DbSet<AppSettingsRecord> AppSettings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=SettingsDb; Integrated Security=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // добавляем композитный ключ для однозначной идентификации приложений и хостов
            //modelBuilder.Entity<AppSettingsRecord>().HasKey(asr => new { asr.Id, asr.Mac});
        }
    }
}
