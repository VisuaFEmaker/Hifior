﻿using System.Collections.Generic;
using UnityEngine;
namespace RPG.AI
{
    public abstract class BaseSingleTargetAI : BaseAI
    {
        /// <summary>
        /// 找到想要攻击的目标单位
        /// </summary>
        /// <returns></returns>
        protected abstract RPGCharacter Target();
        public BaseSingleTargetAI(RPGCharacter ch) : base(ch)
        {
        }
        protected ETargetCamp targetCamp;
        public void SetTargetCamp(ETargetCamp _targetCamp) { targetCamp = _targetCamp; }
        public List<RPGCharacter> GetTargetCharacter()
        {
            List<RPGCharacter> all = new List<RPGCharacter>();
            switch (targetCamp)
            {
                case ETargetCamp.All:
                    all = gameMode.ChapterManager.GetAllCharacters();
                    break;
                case ETargetCamp.Player:
                    all = gameMode.ChapterManager.GetAllCharacters(EnumCharacterCamp.Player);
                    break;
                case ETargetCamp.Enemy:
                    all = gameMode.ChapterManager.GetAllCharacters(EnumCharacterCamp.Enemy);
                    break;
            }
            return all;
        }
        public override void Action()
        {
            Debug.Log("开始AI行动");
            sequenceEvents = new List<Sequence.SequenceEvent>();
            //判定人物是否在一半视野内，如果不在则进行移动
            bool isInHalfView = gameMode.slgCamera.IsTargetInHalfView(unit.GetTileCoord());
            if (isInHalfView) return;
            var moveCamera = AddSequenceEvent<Sequence.CameraMoveToTile>();
            moveCamera.TilePos = unit.GetTileCoord();
            moveCamera.MoveTime = ConstTable.CAMERA_MOVE_TIME();
            var waitTime = AddSequenceEvent<Sequence.WaitTime>();
            waitTime.duration = 1.0f;

        }
    }
}