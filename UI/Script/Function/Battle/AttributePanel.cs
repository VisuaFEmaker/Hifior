﻿using UnityEngine;

namespace RPG.UI
{
    public class AttributePanel : IPanel
    {
        public GameObject charStateBiggerPanel;
        public GameObject attackInfoPanel;
        public GameObject abilityPanel;
        public GameObject itemPanel;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(RPGCharacter ch)
        {
            gameObject.SetActive(true);
            charStateBiggerPanel.GetComponent<CharStateBiggerPanel>().Init(ch);
            attackInfoPanel.GetComponent<AttackInfoPanel>().Init(ch);
            abilityPanel.GetComponent<AbilityPanel>().Init(ch);
            itemPanel.GetComponent<ItemPanel>().Init(ch);
        }
        public override void OnCancelKeyDown()
        {
            if (UIController.ItemTipPanel.gameObject.activeSelf)
                UIController.ItemTipPanel.Hide();
            else
                base.Hide();
        }
    }
}