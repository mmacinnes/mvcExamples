using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Student { get; set; } = default!;
    public DbSet<Course> Course { get; set; } = default!;
    public DbSet<Enrollment> Enrollments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
            .HasData( 
                    new Student{StudentID=10001,FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
                    new Student{StudentID=10002,FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{StudentID=10003,FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{StudentID=10004,FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{StudentID=10005,FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{StudentID=10006,FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
                    new Student{StudentID=10007,FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{StudentID=10008,FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            );
                        modelBuilder.Entity<Course>().HasData( 
                new Course{CourseID=1050,Title="Chemistry",Credits=3},
                new Course{CourseID=4022,Title="Microeconomics",Credits=3},
                new Course{CourseID=4041,Title="Macroeconomics",Credits=3},
                new Course{CourseID=1045,Title="Calculus",Credits=4},
                new Course{CourseID=3141,Title="Trigonometry",Credits=4},
                new Course{CourseID=2021,Title="Composition",Credits=3},
                new Course{CourseID=2042,Title="Literature",Credits=4}
            );


            modelBuilder.Entity<Enrollment>()
                .HasData( 
                    new Enrollment{EnrollmentID=1,StudentID=10001,CourseID=1050,Grade=Grade.A},
                    new Enrollment{EnrollmentID=2,StudentID=10001,CourseID=4022,Grade=Grade.C},
                    new Enrollment{EnrollmentID=3,StudentID=10001,CourseID=4041,Grade=Grade.B},
                    new Enrollment{EnrollmentID=4,StudentID=10002,CourseID=1045,Grade=Grade.B},
                    new Enrollment{EnrollmentID=5,StudentID=10002,CourseID=3141,Grade=Grade.F},
                    new Enrollment{EnrollmentID=6,StudentID=10002,CourseID=2021,Grade=Grade.F},
                    new Enrollment{EnrollmentID=7,StudentID=10003,CourseID=1050},
                    new Enrollment{EnrollmentID=8,StudentID=10004,CourseID=1050},
                    new Enrollment{EnrollmentID=9,StudentID=10004,CourseID=4022,Grade=Grade.F},
                    new Enrollment{EnrollmentID=10,StudentID=10005,CourseID=4041,Grade=Grade.C},
                    new Enrollment{EnrollmentID=11,StudentID=10006,CourseID=1045},
                    new Enrollment{EnrollmentID=12,StudentID=10007,CourseID=3141,Grade=Grade.A}
            );

        }
}
