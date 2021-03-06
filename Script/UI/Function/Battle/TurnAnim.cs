﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TurnAnim : IPanel
    {
        private Text text;
        protected override void Awake()
        {
            base.Awake();

            text = GetComponent<Text>();
            Hide();
        }
        public void Show(int Round, EnumCharacterCamp CampTurn)
        {
            if (CampTurn == EnumCharacterCamp.Player)
                text.text = "回合" + Round + "\n" + "我方行动回合";
            if (CampTurn == EnumCharacterCamp.Enemy)
                text.text = "回合" + Round + "\n" + "敌方行动回合";

            if (CampTurn == EnumCharacterCamp.NPC)
                text.text = "回合" + Round + "\n" + "我方同盟行动回合";

            gameObject.SetActive(true);
            StartCoroutine(Anim());
        }
        IEnumerator Anim()
        {
            GetComponent<Animation>().Play("turnAnimation");
            yield return new WaitForSeconds(2.0f);
            GetComponent<Animation>().Stop("turnAnimation");
            base.Hide();
        }
    }
}