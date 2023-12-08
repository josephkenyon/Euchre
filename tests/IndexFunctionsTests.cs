using webapi.Domain.Statics;
using static webapi.Domain.Statics.Enums;

namespace tests
{
    public class IndexFunctionsTests
    {
        [Test]
        public void IncrementZeroByOne()
        {
            var index = Utils.IncrementIndex(0, 1);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void IncrementThreeByOne()
        {
            var index = Utils.IncrementIndex(3, 1);
            Assert.That(index, Is.EqualTo(0));
        }

        [Test]
        public void IncrementThreeByThree()
        {
            var index = Utils.IncrementIndex(3, 3);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void DecrementThreeByOne()
        {
            var index = Utils.DecrementIndex(3, 1);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void DecrementZeroByOne()
        {
            var index = Utils.DecrementIndex(0, 1);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void DecrementOneByOne()
        {
            var index = Utils.DecrementIndex(1, 1);
            Assert.That(index, Is.EqualTo(0));
        }

        [Test]
        public void DecrementOneByTwo()
        {
            var index = Utils.DecrementIndex(1, 2);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void DecrementOneByThree()
        {
            var index = Utils.DecrementIndex(1, 3);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void DecrementZeroByTwo()
        {
            var index = Utils.DecrementIndex(0, 2);
            Assert.That(index, Is.EqualTo(2));
        }
    }
}