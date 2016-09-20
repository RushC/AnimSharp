using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnimSharp.Animate;

namespace AControllerTest
{
    [TestClass]
    public class AnimatedObjectTest
    {
        [TestMethod]
        public void IsImmutableTest()
        {
            var testObjects = new AnimatedObject[]
            {
                new AnimatedObject(new System.Drawing.Rectangle()),
                new AnimatedObject(new System.Drawing.Point()),
                new AnimatedObject(new ValueAnimation(0, 1))
            };
            var testResults = new bool[]
            {
                false,
                false,
                false
            };

            for (int i = 0; i < testObjects.Length; i++)
            {
                var actual = testObjects[i].IsImmutable();
                var expected = testResults[i];
                Assert.AreEqual(
                    actual,
                    expected,
                    String.Format(
                        "Test Case #{0} - Expected: {1} Actual: {2}",
                        i + 1,
                        expected,
                        actual
                    )
                );
            }
        }
    }
}
