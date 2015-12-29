using BaseMessages.Commands;

namespace Authentication.Commands
{
    public interface Login : ICommand
    {
        string Id { get; set; }
    }
}
