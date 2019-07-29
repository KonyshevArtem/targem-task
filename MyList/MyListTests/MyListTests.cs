using System;
using System.Collections.Generic;
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
            MyList<int> myList = new MyList<int> { 1 };
            Assert.AreEqual(1, myList.Count);
        }

        [Test]
        public void List_ShouldReturnItem_WhenAccessedByIndex()
        {
            MyList<int> myList = new MyList<int> { 0 };
            Assert.AreEqual(0, myList[0]);
        }

        [Test]
        public void List_ShouldSetItem_WhenAccessedByIndex()
        {
            MyList<int> myList = new MyList<int> { 0 };
            myList[0] = 1;
            Assert.AreEqual(1, myList[0]);
        }

        [TestCase(-1), TestCase(1)]
        public void List_ShouldThrowException_WhenGetItemByIncorrectIndex(int index)
        {
            MyList<int> myList = new MyList<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int a = myList[index];
            });
        }

        [TestCase(-1), TestCase(1)]
        public void List_ShouldThrowException_WhenSetItemByIncorrectIndex(int index)
        {
            MyList<int> myList = new MyList<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => { myList[index] = 0; });
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
            MyList<int> myList = new MyList<int> { 0 };
            myList.Clear();
            Assert.AreEqual(0, myList.Count);
        }

        private static object[] indexOfTestSource =
        {
            new object[] {new MyList<int> {0}, 0, 0},
            new object[] {new MyList<int> {1, 0}, 0, 1},
            new object[] {new MyList<int> {1, 0, 0}, 0, 1},
            new object[] {new MyList<int>(), 0, -1},
            new object[] {new MyList<Stack<int>> {null}, null, 0},
            new object[] {new MyList<Stack<int>> {new Stack<int>(), null}, null, 1},
            new object[] {new MyList<Stack<int>>(), null, -1},
        };

        [Test, TestCaseSource(nameof(indexOfTestSource))]
        public void IndexOf_ShouldReturnIndex<T>(MyList<T> myList, T searchItem, int expectedResult)
        {
            Assert.AreEqual(expectedResult, myList.IndexOf(searchItem));
        }

        [Test]
        public void IndexOf_ShouldReturnIndex_WhenSameInstance()
        {
            Stack<int> stack = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> { stack };
            Assert.AreEqual(0, myList.IndexOf(stack));
        }

        [Test]
        public void IndexOf_ShouldNotFindIndex_WhenOtherInstance()
        {
            Stack<int> stackA = new Stack<int>();
            Stack<int> stackB = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> { stackA };
            Assert.AreEqual(-1, myList.IndexOf(stackB));
        }

        [Test]
        public void CopyTo_ShouldThrowException_WhenArrayNull()
        {
            MyList<int> myList = new MyList<int>();
            Assert.Throws<ArgumentNullException>(() => myList.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ShouldThrowException_WhenArgumentOutOfRange()
        {
            MyList<int> myList = new MyList<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => myList.CopyTo(new int[10], -1));
        }

        private static object[] copyToTestSource =
        {
            new object[] {new MyList<int> {1, 2, 3}, new int[2], 0},
            new object[] {new MyList<int> {1, 2, 3}, new int[5], 3},
        };

        [Test, TestCaseSource(nameof(copyToTestSource))]
        public void CopyTo_ShouldThrowException_WhenArrayTooSmall(MyList<int> list, int[] array, int index)
        {
            Assert.Throws<ArgumentException>(() => list.CopyTo(array, index));
        }

        [Test]
        public void CopyTo_ShouldCopyItems()
        {
            MyList<int> myList = new MyList<int> { 1, 2, 3 };
            int[] array = new int[4];
            myList.CopyTo(array, 1);
            for (int i = 0; i < array.Length; ++i)
            {
                Assert.AreEqual(i, array[i]);
            }
        }

        [Test]
        public void CopyTo_ShouldCopyByReference_WhenListContainsClasses()
        {
            Stack<int> stack = new Stack<int>();
            MyList<Stack<int>> myList = new MyList<Stack<int>> { stack };
            Stack<int>[] array = new Stack<int>[1];
            myList.CopyTo(array, 0);
            Assert.AreSame(stack, array[0]);
        }

        private static object[] containsTestSource =
        {
            new object[] {new MyList<int>(), 0, false},
            new object[] {new MyList<int> {0}, 0, true}
        };

        [Test, TestCaseSource(nameof(containsTestSource))]
        public void Contains_ShouldSearchForItem(MyList<int> myList, int item, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, myList.Contains(item));
        }

        [TestCase(-1), TestCase(0)]
        public void RemoveAt_ShouldThrowException_WhenIncorrectIndex(int index)
        {
            MyList<int> myList = new MyList<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => myList.RemoveAt(index));
        }

        [Test]
        public void RemoveAt_ShouldRemoveOneElement()
        {
            MyList<int> myList = new MyList<int> { 0 };
            myList.RemoveAt(0);
            Assert.AreEqual(0, myList.Count);
        }

        [Test]
        public void RemoveAt_ShouldRemoveLastElement_WhenManyElements()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 2, 3 };
            myList.RemoveAt(myList.Count - 1);
            Assert.AreEqual(3, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [Test]
        public void RemoveAt_ShouldRemoveElementInMiddle()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 4, 2 };
            myList.RemoveAt(2);
            Assert.AreEqual(3, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [Test]
        public void Remove_ShouldReturnFalse_WhenListNotContainsItem()
        {
            MyList<int> myList = new MyList<int>();
            Assert.AreEqual(false, myList.Remove(0));
        }

        [Test]
        public void Remove_ShouldReturnTrueAndRemove_WhenItemFound()
        {
            MyList<int> myList = new MyList<int> { 0 };
            Assert.AreEqual(true, myList.Remove(0));
            Assert.AreEqual(0, myList.Count);
        }

        [TestCase(-1), TestCase(1)]
        public void Insert_ShouldThrowException_WhenIndexIncorrect(int index)
        {
            MyList<int> myList = new MyList<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => myList.Insert(index, 0));
        }

        [Test]
        public void Insert_ShouldInsertItem_WhenListEmpty()
        {
            MyList<int> myList = new MyList<int>();
            myList.Insert(0, 0);
            Assert.AreEqual(1, myList.Count);
            Assert.AreEqual(0, myList[0]);
        }

        [Test]
        public void Insert_ShouldInsertItemAtEnd_WhenListNotEmpty()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 2 };
            myList.Insert(3, 3);
            Assert.AreEqual(4, myList.Count);
            Assert.AreEqual(3, myList[3]);
        }

        [Test]
        public void Insert_ShouldOffsetItems_WhenInsertNotAtEnd()
        {
            MyList<int> myList = new MyList<int> { 1, 2, 3 };
            myList.Insert(0, 0);
            Assert.AreEqual(4, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }

        [Test]
        public void Insert_ShouldResizeHiddenArray_WhenCapacityOverflown()
        {
            MyList<int> myList = new MyList<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            myList.Insert(9, 9);
            myList.Insert(10, 10);
            Assert.AreEqual(11, myList.Count);
        }

        [Test]
        public void IEnumerableConstructor_ShouldCopyInputCollection()
        {
            List<int> list = new List<int> { 0, 1, 2 };
            MyList<int> myList = new MyList<int>(list);
            Assert.AreEqual(list.Count, myList.Count);
            for (int i = 0; i < myList.Count; ++i)
            {
                Assert.AreEqual(i, myList[i]);
            }
        }
    }
}