namespace COIS2020.vrajchauhan0827768.Assignments;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using COIS2020.StarterCode.Assignments;

public class ArrayList<T> : ISriList<T> where T : IComparable<T>, IEquatable<T>
{
    /// <summary>
    /// How many items an ArrayList should have space for when it is first constructed.
    /// </summary>
    private const int DEFAULT_CAPACITY = 4;

    /// <summary>
    /// Internal buffer of items.
    /// </summary>
    protected T[] buffer;

    // The number of items currently in the list, AKA the number of `buffer` slots that currently have an item in them.
    public int Count { get; protected set; }

    /// <summary>
    /// The total number of items this list's internal buffer has space for. When <c>Capacity</c> equals
    /// <see cref="Count"/>, an insertion will trigger a resize/reallocation.
    /// </summary>
    public int Capacity { get => buffer.Length; }


    /// <summary>
    /// Creates a new ArrayList.
    /// </summary>
    public ArrayList()
    {
        buffer = new T[DEFAULT_CAPACITY];
        Count = 0;
    }

    /// <summary>
    /// Creates a new ArrayList with a specified pre-allocated capacity.
    /// </summary>
    ///
    /// <param name="capacity">How many items to reserve space for.</param>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="capacity"/> is less than or equal to zero.
    /// </exception>
    public ArrayList(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        buffer = new T[capacity];
        Count = 0;
    }

    /// <summary>
    /// Gets the item at the specified index.
    /// </summary>
    ///
    /// <param name="index">The index of the item to get.</param>
    ///
    /// <returns>The item at the specified index.</returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="index"/> is less than zero or greater than or equal to <see cref="Count"/>.
    /// </exception>
    public T Get(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        return buffer[index];

    }

    /// <summary>
    ///Gets the index of the first occurrence of the specified item in the list.
    /// </summary>
    ///
    /// <param name="item">The item to find.</param>
    ///
    /// <returns>The index of the first occurrence of the specified item, or -1 if the item is not found.</returns>
    public int FindIndex(T item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (buffer[i].Equals(item))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    ///Grow the internal buffer by a factor of 2.
    /// </summary>
    private void Grow()
    {
        T[] newBuffer = new T[Count * 2];
        for (int i = 0; i < Count; i++)
        {
            newBuffer[i] = buffer[i];
        }
        buffer = newBuffer;
    }

    /// <summary>
    ///Inserts an item at the specified index.  If the internal buffer is full, it should be grown. The item should be inserted at the specified index, and all items at or after that index should be shifted one position to the right.
    /// </summary>
    ///
    /// <param name="index">The index at which to insert the item.</param>
    /// <param name="item">The item to insert.</param>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="index"/> is less than zero or greater than <see cref="Count"/>.
    /// </exception>

    public void InsertAt(int index, T item)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (Count == Capacity)
            Grow();

        for (int i = Count; i > index; i--)
        {
            buffer[i] = buffer[i - 1];
        }
        buffer[index] = item;

        Count++;
    }

    /// <summary>
    /// Adds an item to the back of the list.   
    /// </summary>
    ///
    /// <param name="item">The item to add.</param>

    public void AddBack(T item)
    {
        InsertAt(Count, item);
    }

    /// <summary>
    /// Adds an item to the front of the list.
    /// </summary>
    ///
    /// <param name="item">The item to add.</param>

    public void AddFront(T item)
    {
        InsertAt(0, item);
    }

    /// <summary>
    /// Removes the item at the specified index. All items after the specified index should be shifted one position to the left.
    /// </summary>
    ///
    /// <param name="index">The index of the item to remove.</param>
    ///
    /// <returns>The item that was removed.</returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="index"/> is less than zero or greater than or equal to <see cref="Count"/>.
    /// </exception>

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        T remove = buffer[index];
        for (int i = index; i < Count - 1; i++)
        {
            buffer[i] = buffer[i + 1];
        }
        Count--;
        return remove;
    }

    /// <summary>
    /// Removes the first item from the list. If the item is not found, the list should remain unchanged.
    /// </summary>
    ///
    /// <param name="item">The item to remove.</param>
    ///
    public T RemoveFirst()
    {
        return RemoveAt(0);
    }

    /// <summary>
    /// Removes the last item from the list. If the item is not found, the list should remain unchanged.
    /// </summary>
    ///
    public T RemoveLast()
    {
        return RemoveAt(Count - 1);
    }

    /// <summary>
    /// clears the list of all items.
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < Count; i++)
        {
            buffer[i] = default!;
        }
        Count = 0;
    }

    /// <summary>
    /// Swaps the items at the specified indices.
    /// </summary>
    ///
    /// <param name="index1">The index of the first item to swap.</param>
    /// <param name="index2">The index of the second item to swap.</param>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="index1"/> is less than zero or greater than or equal to <see cref="Count"/>.
    /// </exception>
    ///
    public void Swap(int index1, int index2)
    {
        if (index1 < 0 || index1 >= Count)
            throw new ArgumentOutOfRangeException(nameof(index1));
        if (index2 < 0 || index2 >= Count)
            throw new ArgumentOutOfRangeException(nameof(index2));

        T temp = buffer[index1];
        buffer[index1] = buffer[index2];
        buffer[index2] = temp;

    }

    /// <summary>
    /// rotates the list to the left by one position.
    /// </summary>
    public void RotateLeft()
    {
        if (Count <= 1)
            return;

        T firstItem = RemoveAt(0);
        AddBack(firstItem);
    }


    /// <summary>
    /// rotates the list to the right by one position.
    /// </summary>
    public void RotateRight()
    {
        if (Count <= 1)
            return;

        T lastItem = RemoveAt(Count - 1);
        AddFront(lastItem);
    }
    /// <summary>
    /// Sorts the list in ascending order by default. If reverse is true, sorts the list in descending order.
    /// </summary>
    /// <param name="reverse">If true, the list should be sorted in descending order.</param>
    public void Sort(bool reverse = false)
    {
        if (Count <= 1)
            return;

        QuickSort(0, Count - 1, reverse);
    }

    private void QuickSort(int left, int right, bool reverse)
    {
        if (left < right)
        {
            int pivot = Partition(left, right, reverse);
            QuickSort(left, pivot - 1, reverse);
            QuickSort(pivot + 1, right, reverse);
        }
    }

    private int Partition(int left, int right, bool reverse)
    {
        T pivot = buffer[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (reverse)
            {
                if (buffer[j].CompareTo(pivot) > 0)
                {
                    i++;
                    Swap(i, j);
                }
            }
            else
            {
                if (buffer[j].CompareTo(pivot) < 0)
                {
                    i++;
                    Swap(i, j);
                }
            }
        }
        Swap(i + 1, right);
        return i + 1;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return buffer[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();

    }


}
