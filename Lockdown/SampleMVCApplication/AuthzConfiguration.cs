using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lockdown.Configuration;
using SampleMVCApplication.Controllers;

namespace SampleMVCApplication
{
    public class AuthzConfiguration : AbstractConfiguration
    {
        private ITask createUserTask;
        private ITask viewUser;
        private ITask listUsers;

        public ITask ViewUser
        {
            get
            {
                return Task("ViewUser")
                        .Uses<UserController>(c => c.Index(0));
            }
        }

        public ITask CreateUser
        {
            get
            {
                return Task("CreateUser")
                                .Uses<UserController>(c => c.Create())
                                .Uses<UserController>(c => c.Create(null))
                                .Uses(ViewUser);
            }
        }

        public ITask ListUsers
        {
            get
            {
                return Task("ListUsers")
                            .Uses<UserController>(c => c.Index())
                            .Uses(ViewUser);
            }
        }
    }
}