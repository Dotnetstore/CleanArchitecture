namespace Domain.Common.Interfaces;

public interface IBaseAuditableEntity
{
    Guid? CreatedBy { get; init; }

    DateTimeOffset CreatedDate { get; init; }

    Guid? LastUpdatedBy { get; init; }

    DateTimeOffset? LastUpdatedDate { get; init; }

    bool IsDeleted { get; init; }

    Guid? DeletedBy { get; init; }

    DateTimeOffset? DeletedDate { get; init; }

    bool IsSystem { get; init; }

    bool IsGdpr { get; init; }
}