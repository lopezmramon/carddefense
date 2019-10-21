using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class LocalizeCollection : GenericEnumCollection<GameObject, LocalizeDefs> { }
[Serializable]
public class ElementSpriteCollection : GenericEnumCollection<Sprite, Element> { }
[Serializable]
public class ElementTowerCollection : GenericEnumCollection<TowerController, Element> { }
[Serializable]
public class ProjectileCollection : GenericEnumCollection<ProjectileController, Element> { }
public class EnumWrapper 
{
	
}
