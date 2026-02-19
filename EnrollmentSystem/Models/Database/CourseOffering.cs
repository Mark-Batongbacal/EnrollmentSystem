using System;
using System.Collections.Generic;

namespace EnrollmentSystem.Models.Database;

public partial class CourseOffering
{
    public int CourseOfferingId { get; set; }

    public int CourseId { get; set; }

    public int SemesterId { get; set; }

    public int InstructorId { get; set; }

    public int Capacity { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Instructor Instructor { get; set; } = null!;

    public virtual Semester Semester { get; set; } = null!;
}
