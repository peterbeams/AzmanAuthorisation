using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace Lockdown.AcceptanceTests.Common
{
    [Binding]
    public class Steps
    {
        [Given(@"I have an azman store")]
        public void GivenIHaveAnAzmanStore()
        {
            TestContext.AzmanStorePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
            var storeXml = GetType().LoadResourceFromAssemblyContainingType("Lockdown.AcceptanceTests.Common.Empty.xml");
            storeXml = string.Format(storeXml, Guid.NewGuid());
            File.WriteAllText(TestContext.AzmanStorePath, storeXml);
        }

        [Given(@"the store has an application called MyApp")]
        public void GivenTheStoreHasAnApplicationCalledMyApp()
        {
            var document = XDocument.Load(TestContext.AzmanStorePath);

            //get the azman element
            var azMan = document.Element("AzAdminManager");

            var app = new XElement("AzApplication");
            app.Add(new XAttribute("Guid", Guid.NewGuid()));
            app.Add(new XAttribute("Name", "MyApp"));
            app.Add(new XAttribute("Description", string.Empty));
            app.Add(new XAttribute("ApplicationVersion", string.Empty));

            azMan.Add(app);

            document.Save(TestContext.AzmanStorePath);
        }

        [Given(@"the store has an operation (.*) with id (.*)")]
        public void GivenTheStoreHasAnOperationXWithIdY(string name, int id)
        {
            var document = XDocument.Load(TestContext.AzmanStorePath);

            //get the azman element
            var azMan = document.Element("AzAdminManager");

            //add the application
            var app = azMan.Element("AzApplication");

            var operation = new XElement("AzOperation");
            operation.Add(new XAttribute("Guid", Guid.NewGuid()));
            operation.Add(new XAttribute("Name", name));
            operation.Add(new XAttribute("Description", string.Empty));
            operation.Add(new XElement("OperationID", id));

            app.Add(operation);

            document.Save(TestContext.AzmanStorePath);
        }

        [Given(@"the store has an role called (.*)")]
        public void GivenTheStoreHasAnRoleCalledRoleX(string name)
        {
            var document = XDocument.Load(TestContext.AzmanStorePath);

            //get the azman element
            var azMan = document.Element("AzAdminManager");

            //add the application
            var app = azMan.Element("AzApplication");

            var role = new XElement("AzTask");
            role.Add(new XAttribute("Guid", Guid.NewGuid()));
            role.Add(new XAttribute("Name", name));
            role.Add(new XAttribute("Description", string.Empty));
            role.Add(new XAttribute("BizRuleImportedPath", string.Empty));
            role.Add(new XAttribute("RoleDefinition", "True"));

            app.Add(role);

            document.Save(TestContext.AzmanStorePath);
        }

        [When(@"I open the store")]
        public void WhenIOpenTheStore()
        {
            TestContext.Store = new AuthorizationStore(string.Format("msxml://{0}", TestContext.AzmanStorePath));
        }
    }
}
