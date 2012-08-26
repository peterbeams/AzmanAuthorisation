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

        public override void Configure()
        {
            viewUser = Task("ViewUser")
                        .Uses<UserController>(c => c.Index(0));

            createUserTask = Task("CreateUser")
                                .Uses<UserController>(c => c.Create())
                                .Uses<UserController>(c => c.Create(null))
                                .Uses(viewUser);

            listUsers = Task("ListUsers")
                            .Uses<UserController>(c => c.Index())
                            .Uses(viewUser);
        }
    }
}