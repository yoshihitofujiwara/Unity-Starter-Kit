using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CommandDispacher : MonoBehaviour
{
    public ReactiveProperty<bool> isDebugMode = new ReactiveProperty<bool>(false);
    List<IDisposable> _streams = new List<IDisposable>();


    // Start is called before the first frame update
    /*--------------------------------------------------------------------------
        LifeCycleMethods
    --------------------------------------------------------------------------*/
    void Awake()
    {
        // Debug.Log("[Awake]");
        // BindEvent();
    }

    void OnEnable()
    {
        BindEvent();
    }

    void OnDisable()
    {
        UnBindEvent();
    }

    void OnDestroy()
    {
        UnBindEvent();
    }


    /*--------------------------------------------------------------------------
        @Methods
    --------------------------------------------------------------------------*/
    void BindEvent()
    {
        if(0 != _streams.Count)
        {
            return;
        }

        // 固定キー
        _streams.Add(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape)).Subscribe(_ => Application.Quit()));

        // _streams.Add(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.F1)).Subscribe(_ => isDebugMode.Value = !isDebugMode.Value));
        // isDebugMode.Subscribe(_ => {
        //     Debug.Log(_);
        // });
    }


    void UnBindEvent()
    {
    }
}
