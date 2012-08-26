using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Lockdown.Configuration
{
    public abstract class AbstractConfiguration : IDefineRoles
    {
        public void Configure()
        {
        }

        protected ITask Task(string name)
        {
            return null;
        }

        protected IRole Role(string name)
        {
            return null;
        }
    }

    public interface ITask : IDefineWhatATaskUses
    {
    }

    public interface IDefineWhatATaskUses
    {
        ITask Uses<T>(Expression<Func<T, ActionResult>> action);
        ITask Uses(ITask subTask);
    }

    public interface IRole : IDefineWhatARoleDoes
    {
    }

    public interface IDefineWhatARoleDoes
    {
        IRole Can(ITask task);
        IRole Inherits(IRole inheritFrom);
    }
}