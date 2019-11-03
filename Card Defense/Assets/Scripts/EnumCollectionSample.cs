using System;
using UnityEngine;

public enum Defs { Health, Damage, PressStart }

/// <summary>
/// Generic wrapper because unity don't really like generics
/// </summary>
[Serializable]
public class Sample : GenericEnumCollection<string, Defs> {}

public class EnumCollectionSample : MonoBehaviour
{
    public Sample SampleCollection;
}
