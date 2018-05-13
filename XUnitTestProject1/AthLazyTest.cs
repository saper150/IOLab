using IOLab;
using System;
using Xunit;

namespace XUnitTestProject1
{
    class Person { }

    public class LazyTest {
        [Fact]
        public void LazyAthConstructorShouldNotCallFactoryFunction() {
            int callCount = 0;
            new AthLazy<Person>(() => {
                callCount++;
                return new Person();
            });
            Assert.Equal(0, callCount);
        }

        [Fact]
        public void LazyAthShouldReturnFactoryFunctionResultOnGetValue() {
            var person = new Person();
            var lazyPerson = new AthLazy<Person>(() => person);
            Assert.Equal(lazyPerson.GetValue(), person);
        }

        [Fact]
        public void LazyAthShouldCallFactoryFunctionOnlyOnce() {
            int callCount = 0;
            var lazyPerson = new AthLazy<Person>(() => {
                callCount++;
                return new Person();
            });
            lazyPerson.GetValue();
            lazyPerson.GetValue();
            lazyPerson.GetValue();
            Assert.Equal(1, callCount);
        }
    }
}
