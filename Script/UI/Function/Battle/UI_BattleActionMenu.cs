﻿using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace RPG.UI
{
    public class UI_BattleActionMenu : IActionMenu
    {
        public Text Text_ActionPoint;
        public void SetActionPoint(int p)
        {
            Text_ActionPoint.text = "AP:" + p;
        }
    }
}