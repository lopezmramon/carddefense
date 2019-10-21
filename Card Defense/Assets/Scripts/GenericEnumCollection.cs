using System;

[Serializable]
public class GenericEnumCollection<T,TEnum> where T : class where TEnum : struct, IConvertible, IComparable, IFormattable
{
	public T[] Collection;

	public GenericEnumCollection()
	{
		var attributes = (TEnum[])Enum.GetValues(typeof(TEnum));
		Collection = new T[attributes.Length];
	}

	public T GetAttribute(TEnum attribute)
	{
		return GetAttribute(Convert.ToInt32(attribute));
	}

	public T GetAttribute(int index)
	{
		if (Collection != null && index >= 0 && index < Collection.Length)
			return Collection[index];
		return null;
	}
}