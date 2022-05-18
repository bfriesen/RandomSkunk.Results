namespace RandomSkunk.Results.UnitTests;

public class Delegates_helper_methods
{
    public class For_Action
    {
        [Fact]
        public void Returns_action_parameter()
        {
            Action action = () => { };

            var actual = Delegates.Action(action);

            actual.Should().BeSameAs(action);
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
        [Fact]
        public void Returns_func_parameter()
        {
            Func<int> func = () => 1;

            var actual = Delegates.Func(func);

            actual.Should().BeSameAs(func);
        }

        [Fact]
        public void When_generic_argument_is_Task_Throws_ArgumentOutOfRangeException()
        {
            Func<Task> func = () => Task.CompletedTask;

            Action act = () => Delegates.Func(func);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void When_generic_argument_is_Task_of_T_Throws_ArgumentOutOfRangeException()
        {
            Func<Task<int>> func = () => Task.FromResult(1);

            Action act = () => Delegates.Func(func);

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
