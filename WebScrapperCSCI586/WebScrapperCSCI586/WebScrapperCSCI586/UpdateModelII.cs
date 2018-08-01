namespace WebScrapperCSCI586
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UpdateModelII : DbContext
    {
        public UpdateModelII()
            : base("name=UpdateModelII")
        {
        }

        public virtual DbSet<GameSpotGame> GameSpotGames { get; set; }
        public virtual DbSet<IGNGame> IGNGames { get; set; }
        public virtual DbSet<MetaCriticsGame> MetaCriticsGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
