using System;
using System.Collections.Generic;

namespace EnrollmentSystem.Models.Database;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Credits { get; set; } = null!;

    public virtual ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
}
