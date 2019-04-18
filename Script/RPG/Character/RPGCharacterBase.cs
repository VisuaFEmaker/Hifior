﻿using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 敌方，我方均继承自该类
/// </summary>
public class RPGCharacterBase
{
    protected Transform transform;
    protected CharacterLogic logic;
    public CharacterLogic Logic { get { return logic; } }
    public ActionAI AI { get { return logic.Info.AI; } }

    public void SetTransform(Transform t)
    {
        transform = t;
    }
    public Transform GetTransform() { return transform; }
    public GameObject GetGameObject() { return transform.gameObject; }

    public virtual void SetDataFromDef(PlayerDef DefaultData)
    {
        logic = new CharacterLogic(DefaultData);
        logic.careerDef = ResourceManager.GetCareerDef(logic.characterDef.Career);
        if (logic.careerDef == null) Debug.LogError("不存在的career def");
    }
    public virtual void SetAI(ActionAI ai)
    {
        logic.Info.AI = ai;
    }
    /// <summary>
    /// 是否是领导者，在Player里和Enemy类里重写
    /// </summary>
    /// <returns></returns>
    public virtual bool IsLeader()
    {
        return false;
    }
    public EnumCharacterCamp GetCamp()
    {
        return logic.Info.Camp;
    }
    public void SetCamp(EnumCharacterCamp Camp)
    {
        logic.Info.Camp = Camp;
    }

}
