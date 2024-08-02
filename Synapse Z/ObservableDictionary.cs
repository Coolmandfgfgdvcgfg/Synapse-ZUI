using System;
using System.Collections.Generic;

public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public event EventHandler DictionaryChanged;

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        OnDictionaryChanged();
    }

    public new bool Remove(TKey key)
    {
        var result = base.Remove(key);
        if (result)
        {
            OnDictionaryChanged();
        }
        return result;
    }

    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            base[key] = value;
            OnDictionaryChanged();
        }
    }

    protected virtual void OnDictionaryChanged()
    {
        DictionaryChanged?.Invoke(this, EventArgs.Empty);
    }
}
