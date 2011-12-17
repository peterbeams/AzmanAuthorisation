using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Lockdown.AcceptanceTests.Common;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Lockdown.AcceptanceTests.Operations
{
    [Binding]
    public class Steps
    {
        private IEnumerable<Operation> _result;

        [When(@"I get the list of operations")]
        public void WhenIGetTheListOfOperations()
        {
            var store = TestContext.Store;
            _result = store.GetOperations();
        }

        [Then(@"I get a list with an operation called (.*) with id (.*)")]
        public void ThenIGetAListWithAnOperationCalledXWithId1(string name, int id)
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result.Any(o => o.Id == id), Is.True);
            Assert.That(_result.Where(o => o.Id == id).Single().Name, Is.EqualTo(name));
        }
    }
}
