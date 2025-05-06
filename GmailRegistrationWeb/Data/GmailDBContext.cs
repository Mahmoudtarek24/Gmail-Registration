namespace GmailRegistrationWeb.Data
{
    public class GmailDBContext :DbContext
    {
        public GmailDBContext(DbContextOptions<GmailDBContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //make string is varchar(100)
           foreach(var Entites in modelBuilder.Model.GetEntityTypes()) 
           { 
             var stringproperites=Entites.GetProperties().Where(e=>e.ClrType==typeof(string)).ToList();
                foreach (var item in stringproperites)
                {
                    if (item.GetMaxLength() == null)
                    {
                        item.SetMaxLength(120);
                    }
                }   
           }   

           modelBuilder.Entity<Users>().HasIndex(e=>e.PhoneNumber).IsUnique().HasFilter(null);

            modelBuilder.Entity<Users>().Property(e => e.gender).HasConversion<string>().IsRequired();


        }
        public DbSet<Users> users { get; set; } 

    }
}
