using System;
using Lockdown.Messages.Data;

namespace Lockdown.MVC.Tokens
{
    public interface ITokenFactory
    {
        UserToken GetCurrent();
        void Clear();
    }
}