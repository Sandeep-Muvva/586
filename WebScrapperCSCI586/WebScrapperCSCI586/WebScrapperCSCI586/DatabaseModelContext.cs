namespace WebScrapperCSCI586
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseModelContext : DbContext
    {
        public DatabaseModelContext()
            : base("name=DatabaseModel")
        {
        }

        public virtual DbSet<GameSpotGame> GameSpotGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
