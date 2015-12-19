﻿using UnityEngine;
using UnityEditor;
/// <summary>
/// 道具不具有攻击范围，仅限于自己使用对自己产生效果，对敌方产生效果的均视为武器，包括杖
/// </summary>
[AddComponentMenu("RPGEditor/Item/Props")]
public class PropsDef : ExtendScriptableObject
{
    /*[ContextMenu("Log Json")]
    void de()
    {
        Debug.LogError(ToJson());
    }*/
    public PropertyIDNameDesc CommonProperty;
    
    public Sprite Icon;
    public int SinglePrice;
    public int UseNumber;
    public EnumPropsEffectType PropsEffect;
    public int Power;
    public int DedicatedCharacter;
    public bool ImportantProps;
    public bool NoExchange;

    //添加的额外属性
    public CharacterAttribute AdditionalAttribute;
    //成长率提高
    public CharacterAttributeGrow AdditionalAttributeGrow;
}
