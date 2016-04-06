﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using RPG.UI;
/// <summary>
/// 包含战场上需要用到的逻辑，例如胜利失败条件(操作ChapterDef)，各种事件的触发管理（数据从GS_Battle里面拉取）
/// 添加角色到战场，各种逻辑从此处写。
/// </summary>
public class GM_Battle : UGameMode
{
    public EBattleState BattleState;
    public int Round;
    public EnumCharacterCamp RoundCamp;

    private Transform m_playerParent;
    private Transform m_enemyParent;
    private SLGMap m_slgMap;
    private SLGChapter m_slgChapter;

    /// <summary>
    /// 开始剧情结束后触发的事件
    /// </summary>
    public UnityEvent OnStartSequenceFinish;
    public TurnAnim TurnSwitchText;
    public override void Initialize()
    {
        FindSLGMapChapter();
        FindCharacterParent();

        OnStartSequenceFinish.AddListener(() => { Round++; TurnSwitchText.Show(Round, RoundCamp); });
    }
    public SLGMap GetSLGMap()
    {
        if (m_slgMap == null)
        {
            FindSLGMapChapter();
        }
        return m_slgMap;
    }
    public SLGChapter GetSLGChapter()
    {
        if (m_slgChapter == null)
        {
            FindSLGMapChapter();
        }
        return m_slgChapter;
    }
    private void FindSLGMapChapter()
    {
        GameObject Terrain = GameObject.Find("Terrain");
        if (Terrain == null)
        {
            Debug.Log("没有找到Terrain物体");
        }
        m_slgMap = Terrain.GetComponent<SLGMap>();
        m_slgChapter = Terrain.GetComponent<SLGChapter>();
        if (m_slgMap == null)
        {
            Debug.Log("在物体Terrain上没有找到SLGMap组件");
        }
        if (m_slgChapter == null)
        {
            Debug.Log("在物体Terrain上没有找到SLGChapter组件");
        }
    }
    /// <summary>
    /// 在Scene中查找到Player和Enemy的根节点Transform
    /// </summary>
    public void FindCharacterParent()
    {
        m_playerParent = GameObject.Find("InstCharacter/Player").transform;
        m_enemyParent = GameObject.Find("InstCharacter/Enemy").transform;
    }

    private RPGCharacter AddCharacter(CharacterDef Def, int x, int y, CharacterAttribute CustomAttribute = null)
    {
        GameObject playerObj = Def.BattleModel;
        Transform instObj = Instantiate(playerObj).transform;
        instObj.rotation = Quaternion.identity;
        instObj.parent = m_playerParent;
        RPGCharacter Character = instObj.GetComponent<RPGCharacter>();
        if (CustomAttribute != null)
        {
            Character.SetAttribute(CustomAttribute);
        }
        else
        {
            Character.SetAttribute(Def.DefaultAttribute);
        }
        Character.SetTileCoord(x, y, true);
        return Character;
    }
    /// <summary>
    /// 添加一个Player到场景中
    /// </summary>
    /// <param name="ID">加载人物ID</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="FromRecord">从存档加载人物属性</param>
    public void AddPlayer(int ID, int x, int y, bool FromRecord = true, CharacterAttribute CustomAttribute = null)
    {
        PlayerDef def = ResourceManager.GetPlayerDef(ID);
        RPGPlayer Character = AddCharacter(def, x, y, CustomAttribute) as RPGPlayer;
        Character.SetDefaultData(def);
        GetGameState<UGameState>().AddLocalPlayer(Character);
    }
    /// <summary>
    /// 通过PlayerDef添加Player到场景中
    /// </summary>
    /// <param name="Def"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddPlayer(PlayerDef Def, int x, int y, CharacterAttribute CustomAttribute = null)
    {
        RPGPlayer Character = AddCharacter(Def, x, y, CustomAttribute) as RPGPlayer;
        Character.SetDefaultData(Def);
        GetGameState<UGameState>().AddLocalPlayer(Character);
    }
    /// <summary>
    /// 添加一个Enemy到场景中
    /// </summary>
    /// <param name="ID">加载人物ID</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="FromRecord">从存档加载人物属性</param>
    public void AddEnemy(int ID, int x, int y, bool FromRecord = true, CharacterAttribute CustomAttribute = null)
    {
        EnemyDef def = ResourceManager.GetEnemyDef(ID);
        RPGEnemy Character = AddCharacter(def, x, y, CustomAttribute) as RPGEnemy;
        Character.SetDefaultData(def);
        GetGameState<UGameState>().AddLocalEnemy(Character);
    }
    /// <summary>
    /// 通过EnemyDef添加Enemy到场景中
    /// </summary>
    /// <param name="Def"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddEnemy(EnemyDef Def, int x, int y, CharacterAttribute CustomAttribute = null)
    {
        RPGEnemy Character = AddCharacter(Def, x, y, CustomAttribute) as RPGEnemy;
        Character.SetDefaultData(Def);
        GetGameState<UGameState>().AddLocalEnemy(Character);
    }

    public override void BeginPlay()
    {
        base.BeginPlay();
    }
}