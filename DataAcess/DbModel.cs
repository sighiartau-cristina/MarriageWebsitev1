namespace DataAccess
{
    using System.Data.Entity;

    public partial class DbModel : DbContext
    {
        public DbModel()
            : base("name=DbModel")
        {
        }

        public virtual DbSet<ADDRESS> ADDRESSes { get; set; }
        public virtual DbSet<GENDER> GENDERs { get; set; }
        public virtual DbSet<MARITAL_STATUS> MARITAL_STATUS { get; set; }
        public virtual DbSet<MATCH> MATCHes { get; set; }
        public virtual DbSet<ORIENTATION> ORIENTATIONs { get; set; }
        public virtual DbSet<RELIGION> RELIGIONs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<USER_PROFILE> USER_PROFILE { get; set; }
        public virtual DbSet<USER> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADDRESS>()
                .Property(e => e.ADDRESS_STREET)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS>()
                .Property(e => e.ADDRESS_STREETNO)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS>()
                .Property(e => e.ADDRESS_CITY)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS>()
                .Property(e => e.ADDRESS_COUNTRY)
                .IsUnicode(false);

            modelBuilder.Entity<GENDER>()
                .Property(e => e.GENDER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<GENDER>()
                .HasMany(e => e.USER_PROFILE)
                .WithRequired(e => e.GENDER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MARITAL_STATUS>()
                .Property(e => e.MRTSTS_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<MARITAL_STATUS>()
                .HasMany(e => e.USER_PROFILE)
                .WithRequired(e => e.MARITAL_STATUS)
                .HasForeignKey(e => e.STATUS_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ORIENTATION>()
                .Property(e => e.ORIENT_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ORIENTATION>()
                .HasMany(e => e.USER_PROFILE)
                .WithRequired(e => e.ORIENTATION)
                .HasForeignKey(e => e.ORIENTATION_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RELIGION>()
                .Property(e => e.RELIGION_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<RELIGION>()
                .HasMany(e => e.USER_PROFILE)
                .WithRequired(e => e.RELIGION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_PROFILE>()
                .Property(e => e.USRPROF_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER_PROFILE>()
                .Property(e => e.USRPROF_SURNAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER_PROFILE>()
                .Property(e => e.USRPROF_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<USER_PROFILE>()
                .Property(e => e.USRPROF_DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<USER_PROFILE>()
                .Property(e => e.USRPROF_JOB)
                .IsUnicode(false);

            modelBuilder.Entity<USER_PROFILE>()
                .HasMany(e => e.ADDRESSes)
                .WithRequired(e => e.USER_PROFILE)
                .HasForeignKey(e => e.USER_PROFILE_ID);

            modelBuilder.Entity<USER_PROFILE>()
                .HasMany(e => e.MATCHes)
                .WithRequired(e => e.USER_PROFILE)
                .HasForeignKey(e => e.MATCH_USER_PROFILE_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_PROFILE>()
                .HasMany(e => e.MATCHes1)
                .WithRequired(e => e.USER_PROFILE1)
                .HasForeignKey(e => e.USER_PROFILE_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.USER_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.USER_EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.USER_PASSWORD)
                .IsUnicode(false);
        }
    }
}
