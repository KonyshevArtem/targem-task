using System;
using System.Collections.Generic;
using System.Linq;
using MyList;
using NUnit.Framework;

namespace MyListTests
{
    [TestFixture]
    public class MyListTests
    {
        [Test]
        public void Count_ShouldBeZero_WhenListEmpty()
        {
            MyList<int> myList = new MyList<int>();
            Assert.AreEqual(0, myList.Count);
        }

        [Test]
        public void Count_ShouldIncrease_WhenItemAdded()
        {
            MyList<int> myList = new MyList<int> {1};
            Assert.AreEqual(1, myList.Count);
        }

        [Test]
        public void List_ShouldReturnItem_WhenAccessedByIndex()
        {
            MyList<int> myList = new MyList<int> {0};
            Assert.AreEqual(0, myList[0]);
        }

        [Test]
        public void List_ShouldSetItem_WhenAccessedByIndex()
        {
            MyList<int> myList = new MyList<int> {0};
            myList[0] = 1;
            Assert.AreEqual(1, myList[0]);
        }

        [TestCase(-1), TestCase(1)]
        public void List_ShouldThrowException_WhenGetItemByIncorrectIndex(int index)
        {
            MyList<int> myList = new MyList<int>();
            IndexOutOfRangeException exception = Assert.Throws<IndexOutOfRangeException>(() =>
            {
                int a = myList[index];
            });
            Assert.AreEqual($"Index {index} is out of range", exception.Message);
        }

        [TestCase(-1), TestCase(1)]
        public void List_ShouldThrowException_WhenSetItemByIncorrectIndex(int index)
        {
            MyList<int> myList = new MyList<int>();
            IndexOutOfRangeException exception = Assert.Throws<IndexOutOfRangeException>(() => { myList[index] = 0; });
            Assert.AreEqual($"Index {index} is out of range", exception.Message);
        }

        [Test]
        public void HiddenArray_ShouldBeResizedAndCopied_WhenCapacityOverflown()
        {
            MyList<int> myList = new MyList<int>();
            for (int i = 0; i < 11; ++i)
            {
                myList.Add(i);
            }

            Assert.AreEqual(11, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [Test]
        public void HiddenArrayItems_ShouldBeCopiedByReference_WhenCapacityOverflown()
        {
            List<Stack<int>> list = new List<Stack<int>>();
            MyList<Stack<int>> myList = new MyList<Stack<int>>();
            for (int i = 0; i < 11; ++i)
            {
                Stack<int> stack = new Stack<int>();
                list.Add(stack);
                myList.Add(stack);
            }

            for (int i = 0; i < 11; ++i)
            {
                Assert.AreSame(list[i], myList[i]);
            }
        }

        [Test]
        public void List_ShouldBeIterable_ByForEach()
        {
            MyList<int> myList = new MyList<int>();
            for (int i = 0; i < 5; ++i)
            {
                myList.Add(i);
            }

            int counter = 0;
            foreach (int i in myList)
            {
                Assert.AreEqual(counter++, i);
            }

            Assert.AreEqual(5, counter);
        }

        [Test]
        public void List_ShouldBeCleared_WhenClearCalled()
        {
            MyList<int> myList = new MyList<int> {0};
            myList.Clear();
            Assert.AreEqual(0, myList.Count);
        }

        private static object[] containsTestSource =
        {
            new object[] {new MyList<int> {0}, 0, true},
            new object[] {new MyList<int>(), 0, false},
            new object[] {new MyList<Stack<int>> {null}, null, true},
            new object[] {new MyList<Stack<int>>(), null, false},
        };

        [Test, TestCaseSource(nameof(containsTestSource))]
        public void ListContains_ShouldSearchForItem<T>(MyList<T> myList, T searchItem, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, myList.Contains(searchItem));
        }

        [Test]
        public void ListContains_ShouldReturnTrue_WhenSameInstance()
        {
            Stack<int> stack = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> {stack};
            Assert.AreEqual(true, myList.Contains(stack));
        }

        [Test]
        public void ListContains_ShouldReturnFalse_WhenOtherInstance()
        {
            Stack<int> stackA = new Stack<int>();
            Stack<int> stackB = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> {stackA};
            Assert.AreEqual(false, myList.Contains(stackB));
        }
    }
}