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

            Container.DeclareSignal<OnTitleStateChange>();

            signalBus = Container.Resolve<SignalBus>();
        }

        private SignalBus signalBus;

        [Test]
        public void _01_Should_Change_State_To_Login_Success()
        {
            var stateHandler = new TitleStateHandler(signalBus);

            var stateChange = new OnTitleStateChange();

            signalBus.Subscribe<OnTitleStateChange>(e => stateChange = e);

            stateHandler.ChangeState(TitleState.Login);

            StateShouldBeSame(TitleState.Login, stateHandler.GetCurrentState());
            StateShouldBeSame(TitleState.Title, stateChange.PreTitleState);
            StateShouldBeSame(TitleState.Login, stateChange.TitleState);
        }

        private static void StateShouldBeSame(TitleState expected, TitleState compareTitleState)
        {
            Assert.AreEqual(expected, compareTitleState);
        }
    }
}