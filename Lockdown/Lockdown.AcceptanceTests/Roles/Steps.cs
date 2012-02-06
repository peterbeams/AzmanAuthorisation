using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Lockdown.AcceptanceTests.Roles
{
    [Binding]
    public class Steps : AssertionHelper
    {
        private IEnumerable<Role> _result;

        [When(@"I get the list of roles")]
        public void WhenIGetTheListOfRoles()
        {
            _result = TestContext.Store.GetRoles();
        }

        [Given(@"the store has no roles")]
        public void GivenTheStoreHasNoRoles()
        {
        }

        [Then(@"I get a list of roles with (.*) item\(s\) in it")]
        public void ThenIGetAListOfRolesWithNItemsInIt(int count)
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Count(), Is.EqualTo(count));
        }

        [Then(@"I get a list with a role called (.*) in it")]
        public void ThenIGetAListWithARoleCalledRole1InIt(string name)
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Any(r => r.Name.Equals(name)), Is.True);
        }

        [Then(@"I get an empty list of roles")]
        public void ThenIGetAnEmptyList()
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Count(), Is.EqualTo(0));
        }

        [Then(@"the role contains operation (.*)")]
        public void ThenTheRoleContainsOperationN(int operationId)
        {
            Assert.That(_result.ElementAt(0).Operations.Any(o => o.Id == operationId), Is.True);
        }

        [Then(@"the role contains task (.*)")]
        public void ThenTheRoleContainsN(string taskName)
        {
            Assert.That(_result.ElementAt(0).Tasks.Any(o => o.Name == taskName), Is.True);
        }

        [Then(@"the role should contain (.*) tasks")]
        public void TheRoleShoulContainNTasks(int n)
        {
            Assert.That(_result.ElementAt(0).Tasks.Count(), EqualTo(n));
        }
    }
}
