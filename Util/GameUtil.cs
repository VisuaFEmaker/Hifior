﻿using UnityEngine;
using System.Collections;
namespace Utils
{
    public class GameUtil
    {
        private static IEnumerator IEnumDelayFunc(UnityEngine.Events.UnityAction Action, float Time)
        {
            yield return new WaitForSeconds(Time);
            Action();
        }
        public static void DelayFunc(MonoBehaviour MonoInstance, UnityEngine.Events.UnityAction Action, float Time)
        {
            MonoInstance.StartCoroutine(IEnumDelayFunc(Action, Time));
        }
        public static GameObject Instantiate(GameObject ob, Transform parent)
        {
            if (ob == null)
            {
                return null;
            }
            Vector3 vector = ob.transform.localPosition;
            Quaternion quaternion = ob.transform.localRotation;
            Vector3 vector2 = ob.transform.localScale;
            GameObject obj2 = Object.Instantiate(ob) as GameObject;
            obj2.transform.parent = parent;
            obj2.transform.localPosition = vector;
            obj2.transform.localRotation = quaternion;
            obj2.transform.localScale = vector2;
            return obj2;
        }

        #region 游戏控制
        public static void Pause()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
        
        public static void Quit()
        {
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        #endregion

        public static void ScreenShot(string name)
        {
            Application.CaptureScreenshot(name + ".png");
        }
    }
}