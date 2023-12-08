using Microsoft.EntityFrameworkCore;
using webapi.Domain.Game;
using webapi.Domain.Player;
using webapi.Domain.PlayerConnectionDetails;
using webapi.Domain.Tricks;

namespace webapi.Data
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .HasKey(game => game.Name);

            modelBuilder.Entity<PlayerConnectionDetails>()
                .HasKey(playerConnection => new { playerConnection.GameName, playerConnection.PlayerName });

            modelBuilder.Entity<Player>()
                .HasKey(player => new { player.GameName, player.Name });

            modelBuilder.Entity<Trick>()
                .HasKey(trick => trick.GameName);
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerConnectionDetails> PlayerConnections { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Trick> Tricks { get; set; }
    }
}
