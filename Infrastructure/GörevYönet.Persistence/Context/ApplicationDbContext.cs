using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Doğru namespace
using Microsoft.EntityFrameworkCore;
using GörevYönet.Domain.Entitites; // Kullanıcı tanımlı entity'ler için doğru namespace

namespace GörevYönet.Persistence.Context
{
    
public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

    }


}

