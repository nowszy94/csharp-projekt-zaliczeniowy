using Microsoft.EntityFrameworkCore;

namespace projekt.Model
{
    public class GithubContext : DbContext
    {
        public GithubContext(DbContextOptions<GithubContext> options)
            : base(options)
        {
        }

        public DbSet<GithubRepo> GithubRepoItems { get; set; }
    }
}
