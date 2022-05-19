namespace RandomSkunk.Results.UnitTests;

public class Delegates_helper_methods
{
    public class For_Action
    {
        private static readonly Action _action = () => { };

        [Fact]
        public void Returns_action_parameter()
        {
            var actual = Delegates.Action(_action);

            actual.Should().BeSameAs(_action);
        }

        [Fact]
        public void When_action_parameter_is_null_Throws_ArgumentNullException()
        {
            Action act = () => Delegates.Action(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_Func_of_T
    {
        private static readonly Func<int> _func = () => 1;

        [Fact]
        public void Returns_func_parameter()
        {
            var actual = Delegates.Func(_func);

            actual.Should().BeSameAs(_func);
        }

        [Fact]
        public void When_generic_argument_is_Task_Throws_ArgumentOutOfRangeException()
        {
            Action act = () => Delegates.Func(() => Task.CompletedTask);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void When_generic_argument_is_Task_of_T_Throws_ArgumentOutOfRangeException()
        {
            Action act = () => Delegates.Func(() => Task.FromResult(1));

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void When_func_parameter_is_null_Throws_ArgumentNullException()
        {
            Action act = () => Delegates.Func<int>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_AsyncAction
    {
        [Fact]
        public void Returns_asyncAction_parameter()
        {
            AsyncAction asyncAction = () => Task.CompletedTask;

            var actual = Delegates.AsyncAction(asyncAction);

            actual.Should().BeSameAs(asyncAction);
        }

        [Fact]
        public void When_asyncAction_parameter_is_null_Throws_ArgumentNullException()
        {
            Action act = () => Delegates.AsyncAction(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }

    public class For_AsyncFunc_of_T
    {
        [Fact]
        public void Returns_asyncFunc_parameter()
        {
            AsyncFunc<int> asyncFunc = () => Task.FromResult(1);

            var actual = Delegates.AsyncFunc(asyncFunc);

            actual.Should().BeSameAs(asyncFunc);
        }

        [Fact]
        public void When_asyncFunc_parameter_is_null_Throws_ArgumentNullException()
        {
            Action act = () => Delegates.AsyncFunc<int>(null!);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
