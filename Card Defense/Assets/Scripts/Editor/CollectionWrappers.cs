using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ElementSpriteCollection))]
public class ElementSpriteCollectionDrawer : EnumCollectionDrawer<Sprite, Element> { }
[CustomPropertyDrawer(typeof(ElementTowerCollection))]
public class ElementTowerCollectionDrawer : EnumCollectionDrawer<TowerController, Element> { }
[CustomPropertyDrawer(typeof(ProjectileCollection))]
public class ProjectileCollectionDrawer : EnumCollectionDrawer<ProjectileController, Element> { }
[CustomPropertyDrawer(typeof(EnemyCollection))]
public class EnemyCollectionDrawer : EnumCollectionDrawer<EnemyController, EnemyType> { }
[CustomPropertyDrawer(typeof(ViewCollection))]
public class ViewCollectionDrawer : EnumCollectionDrawer<GameObject, View> { }

public class CollectionWrappers : Editor
{
  
}
