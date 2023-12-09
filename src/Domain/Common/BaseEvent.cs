using MediatR;

namespace Domain.Common;

public abstract class BaseEvent : INotification
{
    public DateTimeOffset DateOccurred { get; } = DateTimeOffset.Now;
}