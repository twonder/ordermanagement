using BaseMessages.Commands;

namespace OrderEntry.Commands
{
    public interface StartOrder : ICommand
    {
        string OrderId { get; set; }
        string CustomerId { get; set; }
    }
}
