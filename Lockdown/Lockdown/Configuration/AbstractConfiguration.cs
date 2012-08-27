using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Lockdown.Configuration
{
    public class OperationIdentifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TaskDefinition
    {
        public string Name { get; set; }
        public List<OperationIdentifier> AllowedOperations { get; set; }
        public List<TaskDefinition> AllowedSubTasks { get; set; }
    }

    public class RoleDefintion
    {
        public string Name { get; set; }
        public List<RoleDefintion> InheritedRoles { get; set; }
        public List<TaskDefinition> AllowedTasks { get; set; }
    }

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