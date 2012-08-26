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

        public override void Configure()
        {
            createUserTask = Task("CreateUser")
                                .Uses<UserController>(c => c.Create())
                                .Uses<UserController>(c => c.Create(null));

            
        }
    }
}