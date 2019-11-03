using System;

[Serializable]
public class GenericEnumCollection<T, TEnum> where TEnum : struct, IConvertible, IComparable, IFormattable
{
    public T[] Collection;

    public GenericEnumCollection()
    {
        var attributes = (TEnum[])Enum.GetValues(typeof(TEnum));
        Collection = new T[attributes.Length];
    }

    public T this[TEnum e]
    {
        get { return Collection[Convert.ToInt32(e)]; }
        set { Collection[Convert.ToInt32(e)] = value; }
    }

    public T this[int i]
    {
        get { return Collection[i]; }
        set { Collection[i] = value; }
    }
}