using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ArrayTrie.Tests
{
    public class ArrayTrieTests
    {
        private IArrayTrie<int> ClassUnderTest;

        public ArrayTrieTests()
        {
            this.ClassUnderTest = new ArrayTrie<int>();
        }

        public class Push : ArrayTrieTests
        {
            [Fact]
            public void CreatesANewArrayWithTheValue()
            {
                var newArray = ClassUnderTest.Push(123);

                Assert.Equal(new int[0], ClassUnderTest);
                Assert.Equal(new[] { 123 }, newArray);
            }

            [Fact]
            public void CreatesANewArrayWithTheValue2()
            {
                var newArray = ClassUnderTest.Push(123);
                var newArray2 = newArray.Push(456);

                Assert.Equal(new int[0], ClassUnderTest);
                Assert.Equal(new[] { 123 }, newArray);
                Assert.Equal(new[] { 123, 456 }, newArray2);
            }

            [Fact]
            public void CanPushManyValues()
            {
                IArrayTrie<int> arr = ClassUnderTest;
                foreach (var val in Enumerable.Range(1, 10))
                {
                    arr = arr.Push(val);
                }

                Assert.Equal(Enumerable.Range(1, 10), arr);
            }
        }

        public class Count : ArrayTrieTests
        {
            [Fact]
            public void IsInitiallyZero()
            {
                Assert.Equal(0, ClassUnderTest.Count);
            }

            [Fact]
            public void IncreasesWhenItemPushed()
            {
                var newArray1 = ClassUnderTest.Push(1);
                Assert.Equal(0, ClassUnderTest.Count);
                Assert.Equal(1, newArray1.Count);
            }

            [Fact]
            public void IncreasesWhenItemPushed2()
            {
                var newArray1 = ClassUnderTest.Push(1);
                var newArray2 = newArray1.Push(1);

                Assert.Equal(0, ClassUnderTest.Count);
                Assert.Equal(1, newArray1.Count);
                Assert.Equal(2, newArray2.Count);
            }
        }

        public class Indexing : ArrayTrieTests
        {
            [Fact]
            public void IndexingEmptyArrayThrows()
            {
                Assert.Throws<IndexOutOfRangeException>(() => ClassUnderTest[0]);
            }

            [Fact]
            public void IndexReturnsCorrectItem()
            {
                var arr = ClassUnderTest;
                Enumerable.Range(1, 10).Reverse().ToList().ForEach(v => arr = arr.Push(v));

                for (int index = 0, expectedValue = 10; index < 10; index++, expectedValue--)
                {
                    Assert.Equal(expectedValue, arr[index]);
                }
            }
        }

        public class Pop : ArrayTrieTests
        {
            [Fact]
            public void CreatesANewArrayWithTheValue()
            {
                ClassUnderTest = ClassUnderTest.Push(123);
                
                int val;
                var newArray = ClassUnderTest.Pop(out val);
                
                Assert.Equal(new int[] {123}, ClassUnderTest);
                Assert.Equal(new int[0] , newArray);
                Assert.Equal(123, val);
            }

            [Fact]
            public void CreatesANewArrayWithTheValue2()
            {
                Enumerable.Range(1, 5).ToList().ForEach(x => ClassUnderTest = ClassUnderTest.Push(x));

                int val1, val2;
                var newArray1 = ClassUnderTest.Pop(out val1);
                var newArray2 = newArray1.Pop(out val2);

                Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, ClassUnderTest);
                Assert.Equal(new[] { 1,2,3,4 }, newArray1);
                Assert.Equal(5, val1);
                Assert.Equal(new[] { 1,2,3 }, newArray2);
                Assert.Equal(4, val2);
            }


            [Fact]
            public void ThrowsIfArrayIsEmpty()
            {
                int val;
                Assert.Throws<InvalidOperationException>(() => new ArrayTrie<int>().Pop(out val));
            }


            [Fact]
            public void CanPopManyValues()
            {
                IArrayTrie<int> arr = ClassUnderTest;
                foreach (var val in Enumerable.Range(1, 10))
                {
                    arr = arr.Push(val);
                }

                List<int> expected = Enumerable.Range(1, 10).ToList();
                while (arr.Count > 0)
                {
                    int val;
                    arr = arr.Pop(out val);

                    Assert.Equal(expected[expected.Count - 1], val);
                    expected.RemoveAt(expected.Count - 1);
                    Assert.Equal(expected, arr);
                }
            }
        }
    }
}
