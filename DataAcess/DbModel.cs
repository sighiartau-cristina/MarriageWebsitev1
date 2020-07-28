namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbModel : DbContext
    {
        public DbModel()
            : base("name=DbModel")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Orientation> Orientations { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.AddressStreet)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressStreetNo)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressCity)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressCountry)
                .IsUnicode(false);

            modelBuilder.Entity<File>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<File>()
                .Property(e => e.ContentType)
                .IsUnicode(false);

            modelBuilder.Entity<File>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<Gender>()
                .Property(e => e.GenderName)
                .IsUnicode(false);

            modelBuilder.Entity<Gender>()
                .HasMany(e => e.UserProfiles)
                .WithRequired(e => e.Gender)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.MessageText)
                .IsUnicode(false);

            modelBuilder.Entity<Message>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Orientation>()
                .Property(e => e.OrientationName)
                .IsUnicode(false);

            modelBuilder.Entity<Orientation>()
                .HasMany(e => e.UserProfiles)
                .WithRequired(e => e.Orientation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Religion>()
                .Property(e => e.ReligionName)
                .IsUnicode(false);

            modelBuilder.Entity<Religion>()
                .HasMany(e => e.UserProfiles)
                .WithRequired(e => e.Religion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.UserProfiles)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Job)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Matches)
                .WithRequired(e => e.UserProfile)
                .HasForeignKey(e => e.MatchUserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Matches1)
                .WithRequired(e => e.UserProfile1)
                .HasForeignKey(e => e.UserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.UserProfile)
                .HasForeignKey(e => e.ReceiverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Messages1)
                .WithRequired(e => e.UserProfile1)
                .HasForeignKey(e => e.SenderId)
                .WillCascadeOnDelete(false);
        }
    }
}
