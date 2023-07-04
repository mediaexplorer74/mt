// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DictionaryWrapper`2
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
  internal class DictionaryWrapper<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable,
    IWrappedDictionary,
    IDictionary,
    ICollection
  {
    private readonly IDictionary _dictionary;
    private readonly IDictionary<TKey, TValue> _genericDictionary;
    private object _syncRoot;

    public DictionaryWrapper(IDictionary dictionary)
    {
      ValidationUtils.ArgumentNotNull((object) dictionary, nameof (dictionary));
      this._dictionary = dictionary;
    }

    public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
    {
      ValidationUtils.ArgumentNotNull((object) dictionary, nameof (dictionary));
      this._genericDictionary = dictionary;
    }

    public void Add(TKey key, TValue value)
    {
      if (this._dictionary != null)
      {
        this._dictionary.Add((object) key, (object) value);
      }
      else
      {
        if (this._genericDictionary == null)
          throw new NotSupportedException();
        this._genericDictionary.Add(key, value);
      }
    }

    public bool ContainsKey(TKey key) => this._dictionary != null ? this._dictionary.Contains((object) key) : this._genericDictionary.ContainsKey(key);

    public ICollection<TKey> Keys => this._dictionary != null ? (ICollection<TKey>) this._dictionary.Keys.Cast<TKey>().ToList<TKey>() : this._genericDictionary.Keys;

    public bool Remove(TKey key)
    {
      if (this._dictionary == null)
        return this._genericDictionary.Remove(key);
      if (!this._dictionary.Contains((object) key))
        return false;
      this._dictionary.Remove((object) key);
      return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      if (this._dictionary == null)
        return this._genericDictionary.TryGetValue(key, out value);
      if (!this._dictionary.Contains((object) key))
      {
        value = default (TValue);
        return false;
      }
      value = (TValue) this._dictionary[(object) key];
      return true;
    }

    public ICollection<TValue> Values => this._dictionary != null ? (ICollection<TValue>) this._dictionary.Values.Cast<TValue>().ToList<TValue>() : this._genericDictionary.Values;

    public TValue this[TKey key]
    {
      get => this._dictionary != null ? (TValue) this._dictionary[(object) key] : this._genericDictionary[key];
      set
      {
        if (this._dictionary != null)
          this._dictionary[(object) key] = (object) value;
        else
          this._genericDictionary[key] = value;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      if (this._dictionary != null)
        ((IList) this._dictionary).Add((object) item);
      else
        this._genericDictionary?.Add(item);
    }

    public void Clear()
    {
      if (this._dictionary != null)
        this._dictionary.Clear();
      else
        this._genericDictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => this._dictionary != null ? ((IList) this._dictionary).Contains((object) item) : this._genericDictionary.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      if (this._dictionary != null)
      {
        IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            DictionaryEntry entry = enumerator.Entry;
            array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey) entry.Key, (TValue) entry.Value);
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      else
        this._genericDictionary.CopyTo(array, arrayIndex);
    }

    public int Count => this._dictionary != null ? this._dictionary.Count : this._genericDictionary.Count;

    public bool IsReadOnly => this._dictionary != null ? this._dictionary.IsReadOnly : this._genericDictionary.IsReadOnly;

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      if (this._dictionary == null)
        return ((ICollection<KeyValuePair<TKey, TValue>>) this._genericDictionary).Remove(item);
      if (!this._dictionary.Contains((object) item.Key))
        return true;
      if (!object.Equals(this._dictionary[(object) item.Key], (object) item.Value))
        return false;
      this._dictionary.Remove((object) item.Key);
      return true;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this._dictionary != null ? this._dictionary.Cast<DictionaryEntry>().Select<DictionaryEntry, KeyValuePair<TKey, TValue>>((Func<DictionaryEntry, KeyValuePair<TKey, TValue>>) (de => new KeyValuePair<TKey, TValue>((TKey) de.Key, (TValue) de.Value))).GetEnumerator() : this._genericDictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    void IDictionary.Add(object key, object value)
    {
      if (this._dictionary != null)
        this._dictionary.Add(key, value);
      else
        this._genericDictionary.Add((TKey) key, (TValue) value);
    }

    object IDictionary.this[object key]
    {
      get => this._dictionary != null ? this._dictionary[key] : (object) this._genericDictionary[(TKey) key];
      set
      {
        if (this._dictionary != null)
          this._dictionary[key] = value;
        else
          this._genericDictionary[(TKey) key] = (TValue) value;
      }
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() => this._dictionary != null ? this._dictionary.GetEnumerator() : (IDictionaryEnumerator) new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());

    bool IDictionary.Contains(object key) => this._genericDictionary != null ? this._genericDictionary.ContainsKey((TKey) key) : this._dictionary.Contains(key);

    bool IDictionary.IsFixedSize => this._genericDictionary == null && this._dictionary.IsFixedSize;

    ICollection IDictionary.Keys => this._genericDictionary != null ? (ICollection) this._genericDictionary.Keys.ToList<TKey>() : this._dictionary.Keys;

    public void Remove(object key)
    {
      if (this._dictionary != null)
        this._dictionary.Remove(key);
      else
        this._genericDictionary.Remove((TKey) key);
    }

    ICollection IDictionary.Values => this._genericDictionary != null ? (ICollection) this._genericDictionary.Values.ToList<TValue>() : this._dictionary.Values;

    void ICollection.CopyTo(Array array, int index)
    {
      if (this._dictionary != null)
        this._dictionary.CopyTo(array, index);
      else
        this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[]) array, index);
    }

    bool ICollection.IsSynchronized => this._dictionary != null && this._dictionary.IsSynchronized;

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    public object UnderlyingDictionary => this._dictionary != null ? (object) this._dictionary : (object) this._genericDictionary;

    private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : 
      IDictionaryEnumerator,
      IEnumerator
    {
      private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

      public DictionaryEnumerator(
        IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
      {
        ValidationUtils.ArgumentNotNull((object) e, nameof (e));
        this._e = e;
      }

      public DictionaryEntry Entry => (DictionaryEntry) this.Current;

      public object Key => this.Entry.Key;

      public object Value => this.Entry.Value;

      public object Current
      {
        get
        {
          KeyValuePair<TEnumeratorKey, TEnumeratorValue> current = this._e.Current;
          __Boxed<TEnumeratorKey> key = (object) current.Key;
          current = this._e.Current;
          __Boxed<TEnumeratorValue> local = (object) current.Value;
          return (object) new DictionaryEntry((object) key, (object) local);
        }
      }

      public bool MoveNext() => this._e.MoveNext();

      public void Reset() => this._e.Reset();
    }
  }
}
