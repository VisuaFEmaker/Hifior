﻿using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 道具不具有攻击范围，仅限于自己使用对自己产生效果，对敌方产生效果的均视为武器，包括杖
/// </summary>
public class PropsDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;

    public Sprite Icon;
    public int SinglePrice;
    /// <summary>
    /// 如果小于1 ，则代表该物体不可以被使用，只可以装备
    /// </summary>
    public int UseNumber;
    public EnumPropsEffectType PropsEffect;
    public int Power;
    /// <summary>
    /// 人物专用
    /// </summary>
    public List<int> DedicatedCharacter;
    /// <summary>
    /// 职业专用
    /// </summary>
    public List<int> DedicatedJob;
    public bool EquipItem;
    public bool ImportantProps;
    public bool NoExchange;
    public bool Sellable=true;
    
    //添加的额外属性
    public CharacterAttribute AdditionalAttribute;
    //成长率提高
    public CharacterAttributeGrow AdditionalAttributeGrow;
    public int GetPrice()
    {
        return SinglePrice * UseNumber;
    }
    public int GetSinglePrice()
    {
        if (Sellable)
            return SinglePrice;
        else
            return 0;
    }
    public string Tooltip
    {
        get
        {
            return CommonProperty.Name + " " +" 耐久 "+UseNumber+"\n" + CommonProperty.Description;
        }
    }
    public int GetUsageTime()
    {
        return UseNumber;
    }
}
