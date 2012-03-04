using System;
using Lockdown.MVC.Tokens;

namespace Lockdown.MVC.Config
{
    public interface ITokenFactoryConfig
    {
        IClientConfig UseTokenFactory<T>() where T : ITokenFactory, new();
        IClientConfig UseTokenFactory(Func<ITokenFactory> createFactoryFunction);
    }
}