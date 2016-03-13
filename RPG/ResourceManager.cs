﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class ResourceManager
{
    private const string ASSET_PROPS= "rpgdata/item/props";
    private const string ASSET_WEAPON = "rpgdata/item/weapon";
    private const string ASSET_MAP = "rpgdata/map";
    private static Dictionary<int, WeaponDef> weaponDefTable;
    private static Dictionary<int, PropsDef> propsDefTable;
    private static MapTileDef mapDef;
    public static PropsDef GetPropsDef(int ItemID)
    {
        return GetDef(propsDefTable, ASSET_PROPS, ItemID);
    }
    public static WeaponDef GetWeaponDef(int ItemID)
    {
      return GetDef(weaponDefTable,ASSET_WEAPON, ItemID);
    }
    public static MapTileDef GetMapDef()
    {
        if (mapDef == null)
        {
            mapDef=UGameInstance.Instance.LoadAssetFromBundle<MapTileDef>(Path.Combine(Application.streamingAssetsPath, ASSET_MAP), "TileProperty");
        }
        return mapDef;
    }
    private static T GetDef<T>(Dictionary<int,T> TargetDictionary, string AssetBundleURL,int ID)where T : ScriptableObject
    {
        if (TargetDictionary == null)
        {
            TargetDictionary = new Dictionary<int, T>();
        }
        if (TargetDictionary.ContainsKey(ID))
        {
            return TargetDictionary[ID];
        }
        else
        {
            TargetDictionary.Add(ID, UGameInstance.Instance.LoadAssetFromBundle<T>(Path.Combine(Application.streamingAssetsPath, AssetBundleURL), ID.ToString()));
            return TargetDictionary[ID];
        }
    }

    public static void UnloadAllRPGData()
    {
        weaponDefTable.Clear();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        WeaponDef w = GetWeaponDef(1);
        Debug.Log(w.ToJson());
    }
}