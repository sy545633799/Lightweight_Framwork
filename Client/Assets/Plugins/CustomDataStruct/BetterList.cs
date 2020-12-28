//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This improved version of the System.Collections.Generic.List that doesn't release the buffer on Clear(), resulting in better performance and less garbage collection.
/// </summary>

public class BetterList<T>
{
#if UNITY_FLASH

	List<T> mList = new List<T>();
	
	/// <summary>
	/// Direct access to the buffer. Note that you should not use its 'Length' parameter, but instead use BetterList.size.
	/// </summary>
	
	public T this[int i]
	{
		get { return mList[i]; }
		set { mList[i] = value; }
	}
	
	/// <summary>
	/// Compatibility with the non-flash syntax.
	/// </summary>
	
	public List<T> buffer { get { return mList; } }

	/// <summary>
	/// Direct access to the buffer's size. Note that it's only public for speed and efficiency. You shouldn't modify it.
	/// </summary>

	public int size { get { return mList.Count; } }

	/// <summary>
	/// For 'foreach' functionality.
	/// </summary>

	public IEnumerator<T> GetEnumerator () { return mList.GetEnumerator(); }

	/// <summary>
	/// Clear the array by resetting its size to zero. Note that the memory is not actually released.
	/// </summary>

	public void Clear () { mList.Clear(); }

	/// <summary>
	/// Clear the array and release the used memory.
	/// </summary>

	public void Release () { mList.Clear(); }

	/// <summary>
	/// Add the specified item to the end of the list.
	/// </summary>

	public void Add (T item) { mList.Add(item); }

	/// <summary>
	/// Insert an item at the specified index, pushing the entries back.
	/// </summary>

	public void Insert (int index, T item) { mList.Insert(index, item); }

	/// <summary>
	/// Returns 'true' if the specified item is within the list.
	/// </summary>

	public bool Contains (T item) { return mList.Contains(item); }

	/// <summary>
	/// Remove the specified item from the list. Note that RemoveAt() is faster and is advisable if you already know the index.
	/// </summary>

	public bool Remove (T item) { return mList.Remove(item); }

	/// <summary>
	/// Remove an item at the specified index.
	/// </summary>

	public void RemoveAt (int index) { mList.RemoveAt(index); }

	/// <summary>
	/// Mimic List's ToArray() functionality, except that in this case the list is resized to match the current size.
	/// </summary>

	public T[] ToArray () { return mList.ToArray(); }

	/// <summary>
	/// List.Sort equivalent.
	/// </summary>

	public void Sort (System.Comparison<T> comparer) { mList.Sort(comparer); }

#else

	/// <summary>
	/// Direct access to the buffer. Note that you should not use its 'Length' parameter, but instead use BetterList.size.
	/// </summary>

	public T[] buffer;

	/// <summary>
	/// Direct access to the buffer's size. Note that it's only public for speed and efficiency. You shouldn't modify it.
	/// </summary>

	public int size = 0;

	/// <summary>
	/// For 'foreach' functionality.
	/// </summary>

	public IEnumerator<T> GetEnumerator ()
	{
		if (buffer != null)
		{
			for (int i = 0; i < size; ++i)
			{
				yield return buffer[i];
			}
		}
	}
	
	/// <summary>
	/// Convenience function. I recommend using .buffer instead.
	/// </summary>
	
	public T this[int i]
	{
		get { return buffer[i]; }
		set { buffer[i] = value; }
	}

    // 功能：预分配最大节点
    public void PreAllocate(int nSize)
    {
        if (nSize < 0)
            nSize = 0;
        if(buffer == null || buffer.Length < nSize)
        {
            T[] newList = new T[nSize];
            if (buffer != null && size > 0) buffer.CopyTo(newList, 0);
            buffer = newList;
        }
    }
    public void CleanPreWrite(int nSize)
    {
        size = 0;
        PreAllocate(nSize);
    }
    // 功能：反转数组
    public void Reverse()
    {
        int nEnd = size - 1;
        T temp;
        for(int i = 0; i<nEnd; ++i, --nEnd)
        {
            temp = buffer[i];
            buffer[i] = buffer[nEnd];
            buffer[nEnd] = temp;
        }
    }

    /// <summary>
    /// Helper function that expands the size of the array, maintaining the content.
    /// </summary>

    void AllocateMore ()
	{
		T[] newList = (buffer != null) ? new T[Mathf.Max(buffer.Length << 1, 32)] : new T[32];
		if (buffer != null && size > 0) buffer.CopyTo(newList, 0);
		buffer = newList;
	}

	/// <summary>
	/// Trim the unnecessary memory, resizing the buffer to be of 'Length' size.
	/// Call this function only if you are sure that the buffer won't need to resize anytime soon.
	/// </summary>

	void Trim ()
	{
		if (size > 0)
		{
			if (size < buffer.Length)
			{
				T[] newList = new T[size];
				for (int i = 0; i < size; ++i) newList[i] = buffer[i];
				buffer = newList;
			}
		}
		else buffer = null;
	}

	/// <summary>
	/// Clear the array by resetting its size to zero. Note that the memory is not actually released.
	/// </summary>

	public void Clear () { size = 0; }

    // fast clear and set null
    public void ClearAnSetNull()
    {
        int nSize = size;
        if (nSize <= 0)
            return;
        for (int i = 0; i < nSize; ++i)
        {
            buffer[i] = default(T);
        }
        size = 0;
    }
    public void ClearNullItem()
    {
        int nRealCount = 0;
        for(int i = 0, iMax = size; i< iMax; ++i)
        {
            if(buffer[i] != null)
            {
                if (nRealCount != i)
                    buffer[nRealCount++] = buffer[i];
                else
                    nRealCount++;
            }
        }
        // 删除的节点要清零
        for (int i = nRealCount; i < size; ++i) buffer[i] = default(T);
        size = nRealCount;
    }

    /// <summary>
    /// Clear the array and release the used memory.
    /// </summary>

    public void Release() { size = 0; buffer = null; }

	/// <summary>
	/// Add the specified item to the end of the list.
	/// </summary>

	public void Add (T item)
	{
		if (buffer == null || size == buffer.Length) AllocateMore();
		buffer[size++] = item;
	}

	/// <summary>
	/// Insert an item at the specified index, pushing the entries back.
	/// </summary>

	public void Insert (int index, T item)
	{
		if (buffer == null || size == buffer.Length) AllocateMore();

		if (index < size)
		{
			for (int i = size; i > index; --i) buffer[i] = buffer[i - 1];
			buffer[index] = item;
			++size;
		}
		else Add(item);
	}

	/// <summary>
	/// Returns 'true' if the specified item is within the list.
	/// </summary>

	public bool Contains (T item)
	{
		if (buffer == null) return false;
		for (int i = 0; i < size; ++i) if (buffer[i].Equals(item)) return true;
		return false;
	}

	/// <summary>
	/// Remove the specified item from the list. Note that RemoveAt() is faster and is advisable if you already know the index.
	/// </summary>

	public bool Remove (T item)
	{
		if (buffer != null)
		{
			EqualityComparer<T> comp = EqualityComparer<T>.Default;

			for (int i = 0; i < size; ++i)
			{
				if (comp.Equals(buffer[i], item))
				{
					--size;
					buffer[i] = default(T);
					for (int b = i; b < size; ++b) buffer[b] = buffer[b + 1];
                    buffer[size] = default(T);
                    return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Remove an item at the specified index.
	/// </summary>

	public void RemoveAt (int index)
	{
		if (buffer != null && index < size)
		{
			--size;
			buffer[index] = default(T);
			for (int b = index; b < size; ++b) buffer[b] = buffer[b + 1];
            buffer[size] = default(T);
        }
	}
    public void RemoveAt(int index, int nCount)
    {
        if (buffer != null && index < size)
        {
            if (index + nCount > size)
                nCount = size - index;
            int nOldSize = size;
            size -= nCount;
            int b = index;
            for (; b < size; ++b) buffer[b] = buffer[b + nCount];
            for(; b<nOldSize; ++b)
                buffer[b] = default(T);
        }
    }
    public bool SetItemNull(T item)
    {
        if (buffer != null)
        {
            EqualityComparer<T> comp = EqualityComparer<T>.Default;

            for (int i = 0; i < size; ++i)
            {
                if (comp.Equals(buffer[i], item))
                {
                    buffer[i] = default(T);
                    return true;
                }
            }
        }
        return false;
    }

	/// <summary>
	/// Remove an item from the end.
	/// </summary>

	public T Pop ()
	{
		if (buffer != null && size != 0)
		{
			T val = buffer[--size];
			buffer[size] = default(T);
			return val;
		}
		return default(T);
	}

	/// <summary>
	/// Mimic List's ToArray() functionality, except that in this case the list is resized to match the current size.
	/// </summary>

	public T[] ToArray () { Trim(); return buffer; }

	/// <summary>
	/// List.Sort equivalent.
	/// </summary>

	public bool Sort (System.Comparison<T> comparer)
	{
        bool bSort = false;
        T temp;
        int i = size - 1;
        int j = 0;
        int nSwapIndex = 0;
        while (i > 0)
        {
            nSwapIndex = 0;
            for (j = 0; j < i; j++)
            {
                if(comparer(buffer[j + 1], buffer[j]) < 0)
                {//
                    temp = buffer[j + 1];
                    buffer[j + 1] = buffer[j];
                    buffer[j] = temp;
                    nSwapIndex = j;//记录交换下标
                    bSort = true;
                }
            }
            i = nSwapIndex;
        }
        return bSort;
    }
    public delegate int custom_compare<_TyParam>(T p1, T p2, _TyParam param);
    public bool Sort<_TyParam>(custom_compare<_TyParam> comparer, _TyParam param)
    {
        if (size <= 1)
            return false;
        bool changed = true;
        bool bSort = false;

        while (changed)
        {
            changed = false;

            for (int i = 1; i < size; ++i)
            {
                if (comparer(buffer[i - 1], buffer[i], param) > 0)
                {
                    T temp = buffer[i];
                    buffer[i] = buffer[i - 1];
                    buffer[i - 1] = temp;
                    changed = true;
                    bSort = true;
                }
            }
        }
        return bSort;
    }
#endif
}
