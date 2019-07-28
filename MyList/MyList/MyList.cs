using System;
using System.Collections;
using System.Collections.Generic;

namespace MyList
{
    public class MyList<T> : IList<T>
    {
        private T[] array = new T[10];
        private int count = 0;

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; ++i)
            {
                yield return array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Метод для добавления объекта в лист
        /// Когда внутренний массив заполняется, создается новый с двойным размером
        /// O(1) - в общем случае
        /// O(n) - в худшем случае
        /// </summary>
        /// <param name="item">Объект, добавляемый в лист</param>
        public void Add(T item)
        {
            if (count == array.Length - 1)
            {
                T[] newArray = new T[count * 2];
                array.CopyTo(newArray, 0);
                array = newArray;
            }

            array[count++] = item;
        }

        /// <summary>
        /// Метод для очищения листа.
        /// O(n)
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < count; ++i)
                array[i] = default(T);
            count = 0;
        }

        /// <summary>
        /// Метод для проверки наличия входного объекта в листе
        /// O(n)
        /// </summary>
        /// <param name="item">Объект для поиска в листе</param>
        /// <returns>Находится ли входной объект в листе</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < count; ++i)
            {
                if (item == null && array[i] == null || item.Equals(array[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public int Count => count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Свойство для получения и установки объекта в лист по заданному индексу
        /// </summary>
        /// <param name="index">Индекс для установки или получения объекта</param>
        /// <exception cref="IndexOutOfRangeException">Выбрасывается, когда индекс меньше нуля или
        /// больше количества объектов в списке</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException($"Index {index} is out of range");
                return array[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException($"Index {index} is out of range");
                array[index] = value;
            }
        }
    }
}