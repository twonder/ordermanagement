using BaseMessages.Commands;

namespace OrderEntry.Commands
{
    public interface IProductCommand : ICommand
    {
        string OrderId { get; set; }
        string CustomerId { get; set; }
        string ProductId { get; set; }
    }
}
