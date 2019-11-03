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
[Serializable]
public class EnemyCollection : GenericEnumCollection<Transform, EnemyType> { }
[Serializable]
public class ViewCollection : GenericEnumCollection<GameObject, View> { }

public class EnumWrapper 
{
	
}
