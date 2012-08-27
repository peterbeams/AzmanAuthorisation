using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lockdown.Configuration;
using Lockdown.Configuration.Operations;
using Machine.Specifications;
using Tests.Lockdown.Configuration.Areas;
using Tests.Lockdown.Configuration.Controllers;

namespace Tests.Lockdown.Configuration
{
    [Subject(typeof(OperationIdentifierFactory))]
    public class OperationFactoryContext
    {
        protected static OperationIdentifierFactory target;
        protected static OperationIdentifier result;

        Establish context = () =>
            target = new OperationIdentifierFactory();
    }

    public class when_creating_operation_from_method_call : OperationFactoryContext
    {
        Because of = () =>
            result = target.Create<SampleClass>(c => c.SampleMethod());

        It name_should_be_full = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.SampleClass.SampleMethod");
    }

    public class when_creating_operation_using_method_info : OperationFactoryContext
    {
        private static MethodInfo method = typeof (SampleClass).GetMethod("SampleMethod");

        private Because of = () =>
                             result = target.Create(method);

        It name_should_be_full = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.SampleClass.SampleMethod");
    }

    public class when_creating_operation_with_type_name_ending_in_controller : OperationFactoryContext
    {
        Because of = () =>
            result = target.Create<SampleController>(c => c.SampleMethod());

        It name_should_not_have_controller_in_it = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.Sample.SampleMethod");
    }

    public class when_creating_operation_with_type_in_areas_namespace : OperationFactoryContext
    {
        Because of = () =>
            result = target.Create<ControllerInArea>(c => c.SampleMethod());

        It name_should_not_contain_areas_namespace = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.ControllerInArea.SampleMethod");
    }

    public class when_creating_operation_with_type_in_controllers_namespce : OperationFactoryContext
    {
        Because of = () =>
                             result = target.Create<SampleInControllerNS>(c => c.SampleMethod());

        private It name_should_not_contain_controllers_namespace = () =>
            result.Name.ShouldEqual("Tests.Lockdown.Configuration.SampleInControllerNS.SampleMethod");
    }

    public class when_creating_operation_with_method_that_is_http_post : OperationFactoryContext
    {
        private Because of = () =>
                             result = target.Create<SampleController>(c => c.SamplePost());

        private It name_should_end_with_post = () =>
                                               result.Name.ShouldEndWith("[POST]");

        private It name_should_be_type_then_method = () =>
                                                     result.Name.ShouldStartWith("Tests.Lockdown.Configuration.Sample.SamplePost");
    }

    public class when_creating_operation_from_method_in_root_namespace : OperationFactoryContext
    {
        private Establish context = () =>
                                    target.RootNamespace = "Tests.Lockdown";

        private Because of = () =>
                             result = target.Create<SampleClass>(c => c.SampleMethod());

        private It name_does_not_start_with_rootNamespace = () =>
                                                            result.Name.ShouldEqual("Configuration.SampleClass.SampleMethod");
    }

    public class when_resulting_operation_name_is_longer_than_64_chars : OperationFactoryContext
    {
        private static Exception exception;

        private Because of = () =>
                             exception = Catch.Exception(() => target.Create<SampleClassWithVeryLongClassNameToBreakConstraint>(c => c.SampleMethod()));

        private It exception_is_thrown = () => exception.ShouldNotBeNull();

        private It exception_is_of_type_constraint_violation =
            () => exception.ShouldBe(typeof (AzmanConstraintViolation));
    }

    public class SampleClassWithVeryLongClassNameToBreakConstraint
    {
        public void SampleMethod()
        {
        }
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

        [HttpPost]
        public void SamplePost()
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
