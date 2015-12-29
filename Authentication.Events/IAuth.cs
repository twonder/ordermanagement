using BaseMessages.Events;

namespace Authentication.Events
{
    public interface IAuth : IEvent
    {
        string Id { get; set; }
    }
}
