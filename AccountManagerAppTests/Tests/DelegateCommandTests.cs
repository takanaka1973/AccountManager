using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AccountManagerApp.Tests
{
    [TestClass]
    public class DelegateCommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void コンストラクタは引数executeDelegateにnullを受け付けない()
        {
            var delegateCommand = new DelegateCommand((parameter) => { return true; }, null);
        }

        [TestMethod]
        public void CanExecuteが呼ばれると設定されていたデリゲートを呼び出す()
        {
            object param1 = new object();

            bool delegateCalled = false;
            bool plannedRetval = true;

            Predicate<object> canExecuteDelegate = (parameter) =>
            {
                Assert.AreSame(param1, parameter);
                delegateCalled = true;
                return plannedRetval;
            };

            var delegateCommand = new DelegateCommand(canExecuteDelegate, (parameter) => { });

            bool retval = delegateCommand.CanExecute(param1);

            Assert.IsTrue(delegateCalled);
            Assert.IsTrue(retval);

            delegateCalled = false;
            plannedRetval = false;

            retval = delegateCommand.CanExecute(param1);

            Assert.IsTrue(delegateCalled);
            Assert.IsFalse(retval);
        }

        [TestMethod]
        public void canExecuteDelegateが設定されていない場合はCanExecuteが呼ばれるとtrueを返す()
        {
            var delegateCommand = new DelegateCommand(null, (parameter) => { });
            Assert.IsTrue(delegateCommand.CanExecute(new object()));
        }

        [TestMethod]
        public void Executeが呼ばれると設定されていたデリゲートを呼び出す()
        {
            object param1 = new object();
            bool delegateCalled = false;

            Action<object> executeDelegate = (parameter) =>
            {
                Assert.AreSame(param1, parameter);
                delegateCalled = true;
            };

            var delegateCommand = new DelegateCommand((parameter) => { return true; }, executeDelegate);

            delegateCommand.Execute(param1);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        public void NotifyCanExecuteChangedを呼び出すとCanExecuteChangedイベントが発火される()
        {
            var delegateCommand = new DelegateCommand((parameter) => { return true; }, (parameter) => { });

            bool eventFired = false;

            delegateCommand.CanExecuteChanged += (object sender, EventArgs e) =>
            {
                Assert.AreSame(delegateCommand, sender);
                Assert.AreEqual(EventArgs.Empty, e);
                eventFired = true;
            };

            delegateCommand.NotifyCanExecuteChanged();

            Assert.IsTrue(eventFired);
        }

    }
}
