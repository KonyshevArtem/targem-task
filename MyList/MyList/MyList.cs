using System;
using System.Collections;
using System.Collections.Generic;

namespace MyList
{
    public class MyList<T> : IList<T>
    {
        private T[] array = new T[10];

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
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
            if (Count == array.Length - 1)
            {
                T[] newArray = new T[Count * 2];
                array.CopyTo(newArray, 0);
                array = newArray;
            }

            array[Count++] = item;
        }

        /// <summary>
        /// Метод для очищения листа.
        /// O(n)
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Count; ++i)
                array[i] = default(T);
            Count = 0;
        }

        /// <summary>
        /// Метод для проверки наличия входного объекта в листе
        /// O(n)
        /// </summary>
        /// <param name="item">Объект для поиска в листе</param>
        /// <returns>Находится ли входной объект в листе</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Метод для копирования объектов листа во входной массив, начиная с указанного индекса
        /// O(n)
        /// </summary>
        /// <param name="array">Массив, в который происходит копирование</param>
        /// <param name="arrayIndex">Индекс, с которого объекты помещаются в новый массив</param>
        /// <exception cref="ArgumentNullException">Выкидывается, когда входной массив null</exception>
        /// <exception cref="IndexOutOfRangeException">Выкидывается, когда указанный индекс меньше нуля</exception>
        /// <exception cref="ArgumentException">Выкидывается, когда входной массив недостаточной длинны
        /// для копирования всех объектов</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (Count > array.Length - arrayIndex)
                throw new ArgumentException("Input array too small", nameof(array));
            for (int i = 0; i < Count; ++i)
            {
                array[i + arrayIndex] = this.array[i];
            }
        }

        /// <summary>
        /// Метод для удаления объекта, найденного в листе
        /// O(n)
        /// </summary>
        /// <param name="item">Объект для удаления из листа</param>
        /// <returns>true если объект найден и удален, false если объект не найден</returns>
        public bool Remove(T item)
        {
            int itemIndex = IndexOf(item);
            if (itemIndex == -1)
                return false;
            RemoveAt(itemIndex);
            return true;
        }

        public int Count { get; private set; }
        public bool IsReadOnly => false;

        /// <summary>
        /// Метод для нахождения индекса первого равного объекта, находящегося в листе
        /// O(n)
        /// </summary>
        /// <param name="item">Объект для поиска в листе</param>
        /// <returns>Индекс объекта, если он найден, иначе -1</returns>
        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (item == null && array[i] == null || item != null && item.Equals(array[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Метод для удаления элемента, находящегося на заданном индексе
        /// O(n)
        /// </summary>
        /// <param name="index">Индекс, на котором необходимо удалить элемент</param>
        /// <exception cref="ArgumentOutOfRangeException">Выкидывается, когда заданный индекс меньше нуля или больше длины массива</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            array[index] = default(T);
            for (int i = index; i < Count - 1; ++i)
            {
                array[i] = array[i + 1];
                array[i + 1] = default(T);
            }

            --Count;
        }

        /// <summary>
        /// Свойство для получения и установки объекта в лист по заданному индексу
        /// </summary>
        /// <param name="index">Индекс для установки или получения объекта</param>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, когда индекс меньше нуля или
        /// больше количества объектов в списке</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return array[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                array[index] = value;
            }
        }
    }
}