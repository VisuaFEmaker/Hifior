﻿using System.Collections.Generic;
using UnityEngine;

public static class PositionMath
{
    public const float TileLength = 1.6f;
    public static readonly Vector2Int CameraViewSize = new Vector2Int(15, 15);
    public static Vector3 TilePositionToLocalPosition(Vector2Int pos)
    {
        return new Vector3(pos.x * TileLength, pos.y * -TileLength, 0);
    }
    public static Vector2Int GridPositionToTilePosition(Vector3Int pos)
    {
        return new Vector2Int(pos.x, -pos.y - 1);
    }

    public static int TileWidth = 30;//地图x
    public static int TileHeight = 20;//地图y
    /// <summary>
    /// 前一个图块类型
    /// </summary>
    private static int[,] Tile_type_prev;
    /// <summary>
    /// 图块占用
    /// </summary>
    private static EnumOccupyStatus[,] _Tile_occupy;
    private static Vector2Int _CharacterCenter;//角色坐标
    private static Vector2Int _MouseTileXY;
    private static List<Vector2Int> _PList;//Plist里记录的是真实可以移动到达的区域部分
    private static Dictionary<Vector2Int, Vector2Int> _FootData;//存储指定坐标处的剩余路径
    private static Dictionary<Vector2Int, int> _TempFootData = new Dictionary<Vector2Int, int>();//int表示剩余的移动范围消耗点
    private static List<Vector2Int> _AttackRangeData = new List<Vector2Int>();//int表示剩余的攻击范围消耗点
    private static List<Vector2Int> _MoveRoute = new List<Vector2Int>();//存储移动路径
    private static int _ItemRangeMin;//所有可用装备的最小范围
    private static int _ItemRangeMax; //所有可用装备的最大范围
    private static int _Mov; //移动力
    private static int _Career;
    private static EnumWeaponRangeType _WeaponRangeType;
    /// <summary>
                                                        /// 包含己方单位的可用坐标
                                                        /// </summary>
    private static bool[,] _bMoveAcessList;
    /// <summary>
    /// 排除了己方单位的可用坐标,也就是未被占用的坐标
    /// </summary>
    private static bool[,] _bMoveAcessListExcluded;
    private static bool _bPlayer;
    private static MeshRenderer[] _tileMeshRenders;

    private static List<Vector2Int> m_AttackRangeList = new List<Vector2Int>();

    private static List<Vector2Int> m_MoveRangeList = new List<Vector2Int>();

    private static List<Vector2Int> m_SkillSelectRangeList = new List<Vector2Int>();

    private static List<Vector2Int> m_SkillEffectRangeList = new List<Vector2Int>();

    private static List<Vector2Int> m_CompanionRange = new List<Vector2Int>();

    private static void ResetMoveAcessList()
    {
        _PList.Clear();
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        for (int i = 0; i < TileWidth; i++)
            for (int j = 0; j < TileHeight; j++)
            {
                {
                    _bMoveAcessList[i, j] = false;
                    _bMoveAcessListExcluded[i, j] = false;
                }
            }
    }

    #region 图块占用处理函数
    public static void SetTileEnemyOccupied(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            _Tile_occupy[x, y] = EnumOccupyStatus.Enemy;
    }
    public static void SetTilePlayerOccupied(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            _Tile_occupy[x, y] = EnumOccupyStatus.Player;
    }
    public static void ResetTileOccupyStatus(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            _Tile_occupy[x, y] = EnumOccupyStatus.None;
    }
    public static bool IsOccupyByEnemy(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return _Tile_occupy[x, y] == EnumOccupyStatus.Enemy;
        return false;
    }
    public static bool IsOccupyByPlayer(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return _Tile_occupy[x, y] == EnumOccupyStatus.Player;
        return false;
    }
    public static bool IsOccupyByNone(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return _Tile_occupy[x, y] == EnumOccupyStatus.None;
        return false;
    }
    public static EnumOccupyStatus GetTileOccupyStatus(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return _Tile_occupy[x, y];
        return EnumOccupyStatus.None;
    }
    private static bool IsOccupiedBySameParty(int x, int y)
    {
        if (_Tile_occupy[x, y] == EnumOccupyStatus.None)
            return false;
        if (_bPlayer)
        {
            return (_Tile_occupy[x, y] == EnumOccupyStatus.Player);
        }
        else
        {
            return (_Tile_occupy[x, y] == EnumOccupyStatus.Enemy);
        }
    }
    private static bool IsOccupiedByDiffentParty(int x, int y)
    {
        if (_Tile_occupy[x, y] == EnumOccupyStatus.None)
            return false;
        if (_bPlayer)
        {
            return (_Tile_occupy[x, y] == EnumOccupyStatus.Enemy);
        }
        else
        {
            return (_Tile_occupy[x, y] == EnumOccupyStatus.Player);
        }
    }
    private static bool IsEffectivelyCoordinate(Vector2Int p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < TileWidth && p.y < TileHeight ? true : false;
    }
    private static bool IsEffectivelyCoordinateWithWarning(int x, int y)
    {
        bool effective = x >= 0 && y >= 0 && x < TileWidth && y < TileHeight;
        if (effective)
        {
            return true;
        }
        else
        {
            Debug.LogError("输入的坐标不是有效的坐标" + new Vector2Int(x, y));
            return false;
        }
    }
    private static bool IsEffectivelyCoordinate(int x, int y)
    {
        return x >= 0 && y >= 0 && x < TileWidth && y < TileHeight;
    }
    public static int GetMapPassValue(int job, int x, int y)//得到此处的人物通过消耗
    {
        if (IsOccupiedByDiffentParty(x, y))//图块被敌方占用，则我方不可通过,敌方按正常计算
        {
            return 100;
        }
        else
            return 1;
    }
    private static void _FindDistance(int job, int movement)
    {
        List<Vector2Int> buffer = new List<Vector2Int>(_TempFootData.Keys);
        foreach (Vector2Int key in buffer)
        {
            DirectionScan(key, new Vector2Int(key.x, key.y - 1), _TempFootData[key]); //北
            DirectionScan(key, new Vector2Int(key.x, key.y + 1), _TempFootData[key]); //南
            DirectionScan(key, new Vector2Int(key.x - 1, key.y), _TempFootData[key]); //西
            DirectionScan(key, new Vector2Int(key.x + 1, key.y), _TempFootData[key]); //东
        }
    }

    private static void DirectionScan(Vector2Int lastcord, Vector2Int cord, int surplusConsum)
    {
        if (IsEffectivelyCoordinate(cord))
        {
            int value = surplusConsum - GetMapPassValue(_Career, cord.x, cord.y);//该坐标处剩余可行步数
            if (value >= 0)
            {
                if (!_bMoveAcessList[cord.x, cord.y])
                {
                    _bMoveAcessList[cord.x, cord.y] = true;
                    if (!IsOccupiedBySameParty(cord.x, cord.y))//被相同方占用此处可以继续查找，但是不可以到达此处
                    {
                        _PList.Add(new Vector2Int(cord.x, cord.y));
                        _bMoveAcessListExcluded[cord.x, cord.y] = true;
                    }
                    _TempFootData.Add(cord, value);
                    _FootData.Add(cord, lastcord);
                }
                else
                {
                    if (value > _TempFootData[cord])
                    { //各方向到达同一地点，只取最小机动力消耗
                        _TempFootData[cord] = value;//更新最小消耗
                        _FootData[cord] = lastcord;//更新当前节点最小消耗的父节点
                    }
                }
            }
        }
    }

    private static void AttackScan(Vector2Int MoveablePoint)//输入的是需要遍历的边缘路径,攻击最小和最大范围
    {
        //查找四个方向的可用攻击范围坐标
        int x = MoveablePoint.x;
        int y = MoveablePoint.y;
        int left = (x - _ItemRangeMax) < 0 ? 0 : x - _ItemRangeMax;
        int right = (x + _ItemRangeMax) > TileWidth - 1 ? TileWidth - 1 : x + _ItemRangeMax;
        int up = (y - _ItemRangeMax < 0) ? 0 : y - _ItemRangeMax;
        int bottom = (y + _ItemRangeMax) > TileHeight - 1 ? TileHeight - 1 : y + _ItemRangeMax;
        if (_WeaponRangeType == EnumWeaponRangeType.菱形菱形)
        {
            for (int i = left; i <= right; i++)
            {
                for (int j = up; j <= bottom; j++)
                {
                    if (_bMoveAcessListExcluded[i, j])
                        continue;
                    int absLen = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                        continue;
                    else
                    {
                        _bMoveAcessList[i, j] = true;
                        Vector2Int p = new Vector2Int(i, j);
                        _AttackRangeData.Add(p);
                    }
                }
            }
        }
        if (_WeaponRangeType == EnumWeaponRangeType.十字形)//为1则是只能上下左右寻找目标
        {
            for (int i = left; i <= right; i++)//得到x轴上所有的范围
            {
                if (_bMoveAcessListExcluded[i, y])
                    continue;
                int absLen = Mathf.Abs(i - x);
                if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                    continue;
                else
                {
                    _bMoveAcessList[i, y] = true;
                    Vector2Int p = new Vector2Int(i, y);
                    _AttackRangeData.Add(p);
                }

            }
            for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
            {
                if (_bMoveAcessListExcluded[x, i])
                    continue;
                int absLen = Mathf.Abs(i - y);
                if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                    continue;
                else
                {
                    _bMoveAcessList[x, i] = true;
                    Vector2Int p = new Vector2Int(x, i);
                    _AttackRangeData.Add(p);
                }
            }
        }
    }
    public static void InitActionScope(EnumCharacterCamp camp, int career, int Movement,Vector2Int pos, EnumWeaponType weaponRangeType, Vector2Int atkRange)
    {
        _bPlayer = (camp == EnumCharacterCamp.Player);//0,2为我方的单位

        _Mov = Movement;
        /* if (Gamechar.SkillGroup.isHaveStaticSkill(18))//探险家，无视地形，将其职业设为天马
             _Job = 15;//medifyneed
         if (Gamechar.SkillGroup.isHaveStaticSkill(19))
             _Mov += 2;*/
        _Career = 1;
        _CharacterCenter = pos;

        ResetMoveAcessList();
        _bMoveAcessList[_CharacterCenter.x, _CharacterCenter.y] = true;
        _bMoveAcessListExcluded[_CharacterCenter.x, _CharacterCenter.y] = true;
        _FootData.Add(_CharacterCenter, new Vector2Int(-1, -1));
        _TempFootData.Add(_CharacterCenter, _Mov);

        int countPoint = 0;

        while (countPoint < _Mov)
        {
            _FindDistance(_Career, _Mov);//递归查询距离   _FindDistance(Table._JobTable.getBranch(gamechar.attribute.Job), _Mov, 0, 0);
            countPoint++;
        }

        if (atkRange.x>0&& atkRange.y>0)//装备武器不为空
        {
            _AttackRangeData.Clear();
            _ItemRangeMin = atkRange.x;
            _ItemRangeMax = atkRange.y;
            if (_ItemRangeMax != 0 && _ItemRangeMax - _ItemRangeMin > -1)//武器有距离
            {
                for (int i = 0; i < _PList.Count; i++)//遍历可移动的区域
                {
                    AttackScan(_PList[i]);//递归查询距离   _FindDistance(Table._JobTable.getBranch(gamechar.attribute.Job), _Mov, 0, 0);
                }
            }
        }
    }
    #endregion
}
