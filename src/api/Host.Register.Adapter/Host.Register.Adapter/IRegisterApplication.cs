using Host.Domain.Models;

namespace Host.Register.Adapter;

public interface IRegisterApplication
{
    Task Register(Application application);
}