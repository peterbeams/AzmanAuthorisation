using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Lockdown.AcceptanceTests.Store
{
    [Binding]
    public class Steps
    {
        [Given(@"I have a file path")]
        public void GivenIHaveAFilePath()
        {
            Common.Steps.SetPathToAzmanStore();
        }

        [Given(@"there is on file at the path")]
        public void GivenThereIsOnFileAtThePath()
        {
            File.Delete(TestContext.AzmanStorePath);
        }

        [When(@"I create a new store at the path")]
        public void WhenICreateANewStoreAtThePath()
        {
            AuthorizationStore.Create(string.Format("msxml://{0}", TestContext.AzmanStorePath));
        }

        [Then(@"I can open the new store")]
        public void ThenICanOpenTheNewStore()
        {
            TestContext.Store = new AuthorizationStore(string.Format("msxml://{0}", TestContext.AzmanStorePath));
        }
    }
}
