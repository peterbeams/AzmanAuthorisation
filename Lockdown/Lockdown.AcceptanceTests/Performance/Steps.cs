using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Lockdown.AcceptanceTests.Performance
{
    [Binding]
    public class Steps
    {
        [BeforeScenario("performance")]
        public void BeforeScenario()
        {
            PerformanceContext.Timer = new System.Diagnostics.Stopwatch();
            PerformanceContext.Timer.Start();
        }

        [AfterScenario("performance")]
        public void AfterScenario()
        {
            Console.WriteLine(string.Format("Test took {0}ms",  PerformanceContext.Timer.ElapsedMilliseconds));
        }

        [When(@"I get the list of operations (.*) times")]
        public void WhenIGetTheListOfOperationsXTimes(int times)
        {
            for (var i = 0; i < times; i++)
            {
                var operations = TestContext.Store.GetOperations();
            }
        }

        [When(@"I get the list of operations (.*) times in parallel")]
        public void WhenIGetTheListOfOperationsXTimesInParallel(int times)
        {
            Parallel.For(0, times, i => TestContext.Store.GetOperations());
        }

        [Then(@"the test takes less than (.*)ms")]
        public void ThenTheTestTakesLessThanXms(int duration)
        {
            PerformanceContext.Timer.Stop();
            Assert.That(PerformanceContext.Timer.ElapsedMilliseconds, Is.LessThan(duration));
        }
    }
}
