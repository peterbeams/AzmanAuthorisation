using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockdown.Configuration;
using Lockdown.Configuration.Operations;
using Machine.Specifications;
using Tests.Lockdown.Configuration.Areas;
using Tests.Lockdown.Configuration.Controllers;

namespace Tests.Lockdown.Configuration
{
    [Subject(typeof(OperationFactory))]
    public class OperationFactoryContext
    {
        protected static OperationFactory target;
        protected static OperationIdentifier result;

        Establish context = () =>
            target = new OperationFactory();
    }

    public class when_creating_operation_from_method_call : OperationFactoryContext
    {
        Because of = () =>
            result = target.CreateForMethodCall<SampleClass>(c => c.SampleMethod());

        It name_should_be_full = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.SampleClass.SampleMethod");
    }

    public class when_creating_operation_with_type_name_ending_in_controller : OperationFactoryContext
    {
        Because of = () =>
            result = target.CreateForMethodCall<SampleController>(c => c.SampleMethod());

        It name_should_not_have_controller_in_it = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.Sample.SampleMethod");
    }

    public class when_creating_operation_with_type_in_areas_namespace : OperationFactoryContext
    {
        Because of = () =>
            result = target.CreateForMethodCall<ControllerInArea>(c => c.SampleMethod());

        It name_should_not_contain_areas_namespace = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.ControllerInArea.SampleMethod");
    }

    public class when_creating_operation_with_type_in_controllers_namespce : OperationFactoryContext
    {
        Because of = () =>
                             result = target.CreateForMethodCall<SampleInControllerNS>(c => c.SampleMethod());

        private It name_should_not_contain_controllers_namespace = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.SampleInControllerNS.SampleMethod");
    }

    public class SampleClass
    {
        public void SampleMethod()
        {
        }
    }

    public class SampleController
    {
        public void SampleMethod()
        {
        }
    }

    namespace Controllers
    {
        public class SampleInControllerNS
        {
            public void SampleMethod()
            {   
            }
        }
    }

    namespace Areas
    {
        public class ControllerInArea
        {
            public void SampleMethod()
            {
            }
        }
    }
}
