﻿using UnityEngine;
using System.Collections;

namespace Sequence
{
    [AddComponentMenu("Sequence/Play Music")]
    [HierarchyIcon("Music.png", 2)]
    public class PlayMusic : SequenceEvent
    {
        [Tooltip("要播放的音乐片段")]
        public AudioClip MusicClip;

        [Tooltip("播放的起点，如果音乐是压缩的则可能不准确")]
        public float AtTime;

        public override void OnEnter()
        {
            SoundManage musicController = SoundManage.Instance;
            if (musicController != null)
            {
                float startTime = Mathf.Max(0, AtTime);
                musicController.PlayMusic(MusicClip, startTime);
            }

            Continue();
        }
        public override string GetSummary()
        {
            if (MusicClip == null)
            {
                return null;
            }

            return MusicClip.name;
        }
    }
}