using System;
using System.Collections.Generic;

namespace EnrollmentSystem.Models.Database;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }

    public int CourseOfferingId { get; set; }

    public int? FinalGrade { get; set; }

    public DateOnly? DateEnrolled { get; set; }

    public virtual CourseOffering CourseOffering { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
