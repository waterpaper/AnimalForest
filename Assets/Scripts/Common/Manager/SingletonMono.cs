using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour
    where T: class
{
    //싱글톤 변수입니다.
    private static T _instance;

    //싱글톤 접근
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                var singleObj = GameObject.Find(typeof(T).ToString());

                if (singleObj!=null)
                    DontDestroyOnLoad(singleObj);
            }

            return _instance;
        }
    }
}
