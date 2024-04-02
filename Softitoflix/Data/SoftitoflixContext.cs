using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Softitoflix.Models;

namespace Softitoflix.Data
{
    public class SoftitoflixContext : IdentityDbContext<SoftitoflixUser, SoftitoflixRole, long>
    {
        public SoftitoflixContext(DbContextOptions<SoftitoflixContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MediaCategory>().HasKey(m => new { m.MediaId, m.CategoryId });
            builder.Entity<MediaDirector>().HasKey(m => new { m.MediaId, m.DirectorId });
            builder.Entity<MediaRestriction>().HasKey(m => new { m.MediaId, m.RestrictionId });
            builder.Entity<MediaActor>().HasKey(m => new { m.MediaId, m.ActorId });
            builder.Entity<UserFavorite>().HasKey(u => new { u.UserId, u.MediaId });
            builder.Entity<UserWatched>().HasKey(u => new { u.UserId, u.EpisodeId });
            builder.Entity<Episode>().HasIndex(e => new { e.MediaId, e.SeasonNumber, e.EpisodeNumber }).IsUnique(true);
        }


        public DbSet<Softitoflix.Models.Category> Categories { get; set; } = default!;
        public DbSet<Softitoflix.Models.Director> Directors { get; set; } = default!;
        public DbSet<Softitoflix.Models.Episode> Episodes { get; set; } = default!;
        public DbSet<Softitoflix.Models.Actor> Actors { get; set; } = default!;
        public DbSet<Softitoflix.Models.Media> Medias { get; set; } = default!;
        public DbSet<Softitoflix.Models.MediaActor> MediaActors { get; set; } = default!;
        public DbSet<Softitoflix.Models.MediaCategory> MediaCategories { get; set; } = default!;
        public DbSet<Softitoflix.Models.MediaDirector> MediaDirectors { get; set; } = default!;
        public DbSet<Softitoflix.Models.MediaRestriction> MediaRestrictions { get; set; } = default!;
        public DbSet<Softitoflix.Models.Plan> Plans { get; set; } = default!;
        public DbSet<Softitoflix.Models.UserFavorite> UserFavorites { get; set; } = default!;
        public DbSet<Softitoflix.Models.UserPlan> UserPlans { get; set; } = default!;
        public DbSet<Softitoflix.Models.UserWatched> UserWatcheds { get; set; } = default!;
        public DbSet<Softitoflix.Models.Restriction> Restrictions { get; set; }

    }
}
