using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//https://github.com/mackysoft/Unity-SerializeReferenceExtensions

// Is the name of item
public enum EItem
{
    None,
    Apple,
    Blueberry,
    Orange
}

[CreateAssetMenu,Serializable]
public class Item : ScriptableObject
{
    public Sprite Icon;

    //Player Effects
    [Header("Player Effects")]
    [SerializeReference, SubclassSelector] public List<ItemEffect> addedEffect; 
    //[SerializeReference, SubclassSelector] public List<ItemEffect> dashingEffect; 
    //[SerializeReference, SubclassSelector] public List<ItemEffect> pickupEffect; 
    //[SerializeReference, SubclassSelector] public List<ItemEffect> startWaveEffects;
    //[SerializeReference, SubclassSelector] public List<ItemEffect> endWaveEffects;
    //[SerializeReference, SubclassSelector] public List<ItemEffect> updateEffects;
    [SerializeReference, SubclassSelector] public List<ItemEffect> shootEffects;
    //[SerializeReference, SubclassSelector] public List<ItemEffect> deadEffects;
    //[SerializeReference, SubclassSelector] public List<ItemEffect> takeDamageEffects;

    //Bullet Effects
    [Header("Bullet Effects")]
    //[SerializeReference, SubclassSelector] public List <BulletEffect> projectileHitEffects;
    [SerializeReference, SubclassSelector] public List <ItemEffect> wallEffects;
    //[SerializeReference, SubclassSelector] public List <BulletEffect> trailEffects;

    public EItem Type { get
        {
            if (Enum.TryParse<EItem>(name, out EItem type)) return type;
            return EItem.None;
        }
    }
}
