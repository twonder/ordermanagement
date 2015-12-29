using BaseMessages.Commands;

namespace Authentication.Commands
{
    public interface Logout : ICommand
    {
        string Id { get; set; }
    }
}
