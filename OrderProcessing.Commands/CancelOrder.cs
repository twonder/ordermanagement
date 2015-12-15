using EventLib;

namespace OrderProcessing.Commands
{
    public interface CancelOrder : ICommand
    {
        string OrderId { get; set; }
    }
}
