using CourseSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<CourseContent> CourseContents { get; set;}
        public DbSet<CourseGrade> CourseGrades { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CourseContent>()
                .HasKey(cl => new { cl.CourseId, cl.ContentId });
            modelBuilder.Entity<TeacherCourse>()
                .HasKey(cl => new { cl.TeacherId, cl.CourseGradeId });
            
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            //Seeding a  'Administrator' role to AspNetRoles table
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole {Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "SuperAdmin", NormalizedName = "SUPERADMIN".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "71805494-f543-40ad-b63e-870319d46db8", Name = "Teacher", NormalizedName = "TEACHER".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2bbf9a62-7592-45c5-9e08-97f817c9b3cc", Name = "Student", NormalizedName = "Student".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "acbb1919-8c47-4be3-a4cb-c4198a65ca96", Name = "Admin", NormalizedName = "ADMIN".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "49a8f2dc-208c-48cb-bf49-82c25c0f9e38", Name = "Guest", NormalizedName = "Guest".ToUpper() });

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<ApplicationUser>();


            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    FirstName = "SUPER",
                    LastName = "ADMIN",
                    FullName = "SUPER ADMIN",
                    UserName = "Y3I7152427U3JUA",
                    NormalizedUserName = "Y3I7152427U3JUA",
                    PasswordHash = hasher.HashPassword(null, "8mzFdw)i#HxvUJu")
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "a7936f4c-d172-43ef-a7e2-916c4b3ef9cf", // primary key
                    FirstName = "Ogretmen",
                    LastName = "Deneme",
                    FullName = "Ogretmen Deneme",
                    UserName = "OgretmenDeneme",
                    NormalizedUserName = "OGRETMENDENEME",
                    PasswordHash = hasher.HashPassword(null, "123qweQ!")
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "bc651f10-6033-44ba-ad5d-db9495d56613", // primary key
                    FirstName = "Ogrenci",
                    LastName = "Deneme",
                    FullName = "Ogrenci Deneme",
                    UserName = "OgrenciDeneme",
                    NormalizedUserName = "OGRENCIDENEME",
                    PasswordHash = hasher.HashPassword(null, "123qweQ!")
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "d309c246-c6e9-4021-9f08-8206a00ed969", // primary key
                    FirstName = "Admin",
                    LastName = "Deneme",
                    FullName = "Admin Deneme",
                    UserName = "AdminDeneme",
                    NormalizedUserName = "ADMINDENEME",
                    PasswordHash = hasher.HashPassword(null, "123qweQ!")
                }
            );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "acf21fe8-eda6-4cde-a6f7-b4adc1507007", // primary key
                    FirstName = "Misafir",
                    LastName = "Deneme",
                    FullName = "Misafir Deneme",
                    UserName = "MisafirDeneme",
                    NormalizedUserName = "MISAFIRDENEME",
                    PasswordHash = hasher.HashPassword(null, "123qweQ!")
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId= "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210"
                },
                new IdentityUserRole<string>
                {
                    UserId = "a7936f4c-d172-43ef-a7e2-916c4b3ef9cf",
                    RoleId = "71805494-f543-40ad-b63e-870319d46db8"
                },
                new IdentityUserRole<string>
                {
                    UserId = "bc651f10-6033-44ba-ad5d-db9495d56613",
                    RoleId = "2bbf9a62-7592-45c5-9e08-97f817c9b3cc"
                },
                new IdentityUserRole<string>
                {
                    UserId = "d309c246-c6e9-4021-9f08-8206a00ed969",
                    RoleId = "acbb1919-8c47-4be3-a4cb-c4198a65ca96"
                },
                new IdentityUserRole<string>
                {
                    UserId = "acf21fe8-eda6-4cde-a6f7-b4adc1507007",
                    RoleId = "49a8f2dc-208c-48cb-bf49-82c25c0f9e38"
                }
            );
        }
    }
}
