using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Lockdown.Configuration
{
    public abstract class AbstractConfiguration : IDefineRoles
    {
        public abstract void Configure();

        protected ITask Task(string name)
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
    }
}