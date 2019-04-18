﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    [AddComponentMenu("Sequence/Add Character")]
    public class AddCharacter : SequenceEvent
    {
        public EnumCharacterCamp Camp;
        public int ID;
        public Vector2Int Coord;
        public bool UseDefaultAttribute = true;
        public CharacterAttribute Attribute;
        public List<int> Items;
        public override void OnEnter()
        {
           if(Camp== EnumCharacterCamp.Player)
            {
                if (UseDefaultAttribute) Attribute = null;
                RPGCharacter ch = RPGPlayer.Create(ID,Attribute);
                gameMode.AddUnitToMap(ch,Coord);
            }
            if (Camp == EnumCharacterCamp.Enemy)
            {
                if (UseDefaultAttribute) Attribute = null;
                RPGCharacter ch = RPGEnemy.Create(ID, Attribute);
                gameMode.AddUnitToMap(ch, Coord);
            }
            Continue();
        }
    }
}