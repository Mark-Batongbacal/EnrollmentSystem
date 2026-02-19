using System;
using System.Collections.Generic;

namespace EnrollmentSystem.Models.Database;

public partial class Student
{
    public int StudentId { get; set; }

    public int StudentNumber { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? DateCreated { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
