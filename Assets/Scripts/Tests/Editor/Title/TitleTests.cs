using Core.Title;
using NUnit.Framework;
using Zenject;

namespace Tests.Editor.Title
{
    [TestFixture]
    public class TitleTests : ZenjectUnitTestFixture
    {
        public override void Setup()
        {
            base.Setup();

            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnStateChange>();

            signalBus = Container.Resolve<SignalBus>();
        }

        private SignalBus signalBus;

        [Test]
        public void _01_Should_Change_State_To_Login_Success()
        {
            var stateHandler = new StateHandler(signalBus);

            var stateChange = new OnStateChange();

            signalBus.Subscribe<OnStateChange>(e => stateChange = e);

            stateHandler.ChangeState(State.Login);

            StateShouldBeSame(State.Login, stateHandler.GetCurrentState());
            StateShouldBeSame(State.Title, stateChange.preState);
            StateShouldBeSame(State.Login, stateChange.state);
        }

        private static void StateShouldBeSame(State expected, State compareState)
        {
            Assert.AreEqual(expected, compareState);
        }
    }
}