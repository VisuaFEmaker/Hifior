﻿using UnityEngine;
using System.Collections.Generic;

public static class Log
{
    /// <summary>
    /// 有必要输出的Log日志通过此函数打印，非必要的自己调试使用Debug.Log,提交需删除
    /// </summary>
    /// <param name="obj"></param>
    public static void Write(object obj)
    {
        Debug.Log(obj.ToString());
    }
    public static void Write(params object[] objs)
    {
        if (objs.Length < 1)
            Debug.Log("数组为0，无数据显示");
        else
        {
            string log = null;
            for (int i = 0; i < objs.Length - 1; i++)
            {
                log += objs[i].ToString() + '\n';
            }
            log += objs[objs.Length - 1].ToString();
            Debug.Log(log);
        }
    }
    public static void Write<T>(List<T> obj)
    {
        foreach (T item in obj)
        {
            Debug.Log(item.ToString() + "\n");
        }
    }
    public static void Write<T>(List<T> obj, System.Type enumType)
    {
        foreach (T item in obj)
        {
            Debug.Log(System.Enum.GetName(enumType, item) + "\n");
        }
    }
    public static void Write<T1, T2>(Dictionary<T1, T2> obj)
    {
        foreach (KeyValuePair<T1, T2> item in obj)
        {
            Debug.Log(item.Key + "  " + item.Value + "\n");
        }
    }
}
