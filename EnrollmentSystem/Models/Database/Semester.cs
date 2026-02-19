using System;
using System.Collections.Generic;

namespace EnrollmentSystem.Models.Database;

public partial class Semester
{
    public int SemesterId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
}
