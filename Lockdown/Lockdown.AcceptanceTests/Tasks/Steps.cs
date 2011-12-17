using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Lockdown.AcceptanceTests.Tasks
{
    [Binding]
    public class Steps
    {
        private IEnumerable<Task> _result;

        [When(@"I get the list of tasks")]
        public void WhenIGetTheListOfRoles()
        {
            _result = TestContext.Store.GetTasks();
        }

        [Given(@"the store has no tasks")]
        public void GivenTheStoreHasNoRoles()
        {
        }

        [Then(@"I get a list of tasks with 2 items in it")]
        public void ThenIGetAListOfTasksWith2ItemsInIt()
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Count(), Is.EqualTo(2));
        }

        [Then(@"I get a list with a task called (.*) in it")]
        public void ThenIGetAListWithARoleCalledRole1InIt(string name)
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Any(r => r.Name.Equals(name)), Is.True);
        }

        [Then(@"I get an empty list of tasks")]
        public void ThenIGetAnEmptyList()
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Count(), Is.EqualTo(0));
        }
    }
}
