namespace EnrollmentSystem.Repository.CourseOfferings
{
    public interface ICourseOfferingRepository
    {
        Task<CourseOffering?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CourseOffering>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(CourseOffering offering, CancellationToken cancellationToken = default);
        void Update(CourseOffering offering);
        void Delete(CourseOffering offering);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<CourseOffering>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
    }
}

