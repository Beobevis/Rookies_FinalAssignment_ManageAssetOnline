namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;
using WebApi.Entities;


public class DataContext : DbContext
{
    /*
    public DataContext()
    {

    }
    */
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<ReturnRequest> ReturnRequests { get; set; } = null!;
    public DbSet<TokenLogout> TokenLogouts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TokenLogout>().ToTable("TokenLogout");
        builder.Entity<TokenLogout>().HasKey(t => t.Id);

        builder.Entity<User>().ToTable("User");
        builder.Entity<User>().HasKey(u => u.Id);

        builder.Entity<Asset>().ToTable("Asset");
        builder.Entity<Asset>().HasKey(a => a.Id);
        builder.Entity<Asset>()
        .HasOne(a => a.Category)
        .WithMany(c => c.Assets)
        .HasForeignKey(a => a.CategoryId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();
        /*
        builder.Entity<Asset>()
        .HasMany(a => a.Assignments)
        .WithOne(a => a.Asset)
        .HasForeignKey(a => a.AssetId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);

        builder.Entity<Asset>()
        .HasMany(a => a.ReturnRequests)
        .WithOne(a => a.Asset)
        .HasForeignKey(a => a.AssetId)
        .OnDelete(DeleteBehavior.NoAction)
        .IsRequired(false);
        */
        builder.Entity<Category>().ToTable("Category");
        builder.Entity<Category>().HasKey(c => c.Id);

        builder.Entity<Assignment>().ToTable("Assignment");
        builder.Entity<Assignment>().HasKey(a => a.Id);
        builder.Entity<Assignment>()
        .HasOne(a => a.Asset)
        .WithMany(a => a.Assignments)
        .HasForeignKey(a => a.AssetId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

        builder.Entity<Assignment>()
        .HasOne(a => a.AssignTo)
        .WithMany(u => u.AssignTos)
        .HasForeignKey(a => a.AssignToId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();

        builder.Entity<Assignment>()
        .HasOne(a => a.AssignedBy)
        .WithMany(u => u.AssignBys)
        .HasForeignKey(a => a.AssignedById)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();

        builder.Entity<ReturnRequest>().ToTable("ReturnRequest");
        builder.Entity<ReturnRequest>().HasKey(r => r.Id);
        builder.Entity<ReturnRequest>()
        .HasOne(r => r.Asset)
        .WithMany(a => a.ReturnRequests)
        .HasForeignKey(r => r.AssetId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();

        builder.Entity<ReturnRequest>()
        .HasOne(r => r.RequestedBy)
        .WithMany(u => u.ReturnBys)
        .HasForeignKey(r => r.RequestedById)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();

        builder.Entity<ReturnRequest>()
        .HasOne(r => r.AcceptedBy)
        .WithMany(u => u.AcceptBys)
        .HasForeignKey(r => r.AcceptedById)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

        builder.Entity<Category>().HasData(
            new Category { Id = 1, CategoryName = "Laptop", Prefix = "LA" },
            new Category { Id = 2, CategoryName = "Desktop", Prefix = "DE" },
            new Category { Id = 3, CategoryName = "Printer", Prefix = "PR" },
            new Category { Id = 4, CategoryName = "Scanner", Prefix = "SC" },
            new Category { Id = 5, CategoryName = "Network", Prefix = "NE" }
        );

        builder.Entity<Asset>().HasData(
            new Asset
            {
                Id = 1,
                AssetCode = "LA000001",
                AssetName = "HP Laptop",
                CategoryId = 1,
                Specification = "HP Laptop",
                InstalledDate = new DateTime(2022, 04, 25),
                Location = "Hochiminh",
                State = AssetState.Available,
                CreateAt = DateTime.Now,
                CreateBy = 1,
            },
            new Asset
            {
                Id = 2,
                AssetCode = "LA000002",
                AssetName = "Dell Laptop",
                CategoryId = 1,
                Specification = "Dell Laptop",
                InstalledDate = new DateTime(2022, 04, 25),
                Location = "Hochiminh",
                State = AssetState.Available,
                CreateAt = DateTime.Now,
                CreateBy = 1,
            }
        );

        builder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                StaffCode = "SD0001",
                Firstname = "Ed",
                Lastname = "Pendlebery",
                Username = "edp",
                DoB = new DateTime(1991, 12, 15),
                PasswordHash = BCryptNet.HashPassword("edp@15121991"),
                JoinDate = new DateTime(2021, 09, 08),
                Type = Role.Admin,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = true
            }, new User
            {
                Id = 2,
                StaffCode = "SD0002",
                Firstname = "Courtney",
                Lastname = "O'Loinn",
                Username = "courtneyo",
                DoB = new DateTime(1992, 09, 07),
                PasswordHash = BCryptNet.HashPassword("courtneyo@07091992"),
                JoinDate = new DateTime(2021, 08, 06),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = true,
                Location = "Hochiminh",
                IsFirstLogin = false
            }, new User
            {
                Id = 3,
                StaffCode = "SD0003",
                Firstname = "Eudora",
                Lastname = "Renahan",
                Username = "eudorar",
                DoB = new DateTime(1991, 05, 05),
                PasswordHash = BCryptNet.HashPassword("eudorar@05051991"),
                JoinDate = new DateTime(2021, 05, 23),
                Type = Role.Admin,
                Gender = "Female",
                IsDisabled = false,
                Location = "Hanoi",
                IsFirstLogin = false
            }, new User
            {
                Id = 4,
                StaffCode = "SD0004",
                Firstname = "Bevin",
                Lastname = "Hugueville",
                Username = "bevinh",
                DoB = new DateTime(1991, 03, 10),
                PasswordHash = BCryptNet.HashPassword("2"),
                JoinDate = new DateTime(2021, 12, 09),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = false
            }, new User
            {
                Id = 5,
                StaffCode = "SD0005",
                Firstname = "Andrew",
                Lastname = "Broadis",
                Username = "andrewb",
                DoB = new DateTime(1998, 06, 21),
                PasswordHash = BCryptNet.HashPassword("andrewb@21061998"),
                JoinDate = new DateTime(2021, 03, 31),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = true
            }, new User
            {
                Id = 6,
                StaffCode = "SD0006",
                Firstname = "Tades",
                Lastname = "Zecchi",
                Username = "tadesz",
                DoB = new DateTime(1994, 12, 21),
                PasswordHash = BCryptNet.HashPassword("tadesz@21121994"),
                JoinDate = new DateTime(2022, 02, 18),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hanoi",
                IsFirstLogin = false
            }, new User
            {
                Id = 7,
                StaffCode = "SD0007",
                Firstname = "Vernor",
                Lastname = "Huson",
                Username = "vernorh",
                DoB = new DateTime(1999, 10, 25),
                PasswordHash = BCryptNet.HashPassword("vernorh@25101999"),
                JoinDate = new DateTime(2022, 01, 04),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = true,
                Location = "Hochiminh",
                IsFirstLogin = false
            }, new User
            {
                Id = 8,
                StaffCode = "SD0008",
                Firstname = "Rufe",
                Lastname = "Yole",
                Username = "rufey",
                DoB = new DateTime(1990, 11, 16),
                PasswordHash = BCryptNet.HashPassword("rufey@16111990"),
                JoinDate = new DateTime(2022, 01, 17),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = false,
                Location = "Hochiminh",
                IsFirstLogin = false
            }, new User
            {
                Id = 9,
                StaffCode = "SD0009",
                Firstname = "Orton",
                Lastname = "Woodyear",
                Username = "ortonw",
                DoB = new DateTime(1994, 03, 29),
                PasswordHash = BCryptNet.HashPassword("ortonw@29031994"),
                JoinDate = new DateTime(2021, 11, 29),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = true,
                Location = "Hochiminh",
                IsFirstLogin = false
            }, new User
            {
                Id = 10,
                StaffCode = "SD0010",
                Firstname = "Peyter",
                Lastname = "Carmichael",
                Username = "peyterc",
                DoB = new DateTime(2000, 01, 17),
                PasswordHash = BCryptNet.HashPassword("peyterc@17012000"),
                JoinDate = new DateTime(2021, 05, 18),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = true,
                Location = "Hochiminh",
                IsFirstLogin = false
            }, new User
            {
                Id = 11,
                StaffCode = "SD0011",
                Firstname = "Kathy",
                Lastname = "Pitchers",
                Username = "kathyp",
                DoB = new DateTime(1995, 07, 17),
                PasswordHash = BCryptNet.HashPassword("kathyp@17071995"),
                JoinDate = new DateTime(2021, 07, 14),
                Type = Role.Staff,
                Gender = "Female",
                IsDisabled = false,
                Location = "Hanoi",
                IsFirstLogin = true
            }, new User
            {
                Id = 12,
                StaffCode = "SD0012",
                Firstname = "Beau",
                Lastname = "Thorndycraft",
                Username = "beaut",
                DoB = new DateTime(1991, 07, 20),
                PasswordHash = BCryptNet.HashPassword("beaut@20071991"),
                JoinDate = new DateTime(2021, 08, 11),
                Type = Role.Staff,
                Gender = "Male",
                IsDisabled = true,
                Location = "Hochiminh",
                IsFirstLogin = true
            }
        );
    }
}
