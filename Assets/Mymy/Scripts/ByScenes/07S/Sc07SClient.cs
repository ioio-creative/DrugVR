﻿using DrugVR_Scribe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Sc07SClient : VideoSceneClientBase
{
    [SerializeField]
    DrugVR_SceneENUM nextSceneToLoad = DrugVR_SceneENUM.Sc07B;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.SkyVideoPlayer.isLooping = true;
    }

    protected override void HandleSystemVideoEnd(VideoPlayer source)
    {
        // This scene won't go to next scene based on Video ends
    }

    public static void GoToSceneOnChoice()
    {
        if (Scribe.Side06 == false)
        {
            GoToMethScene();
        }
        else
        {
            GoToEndSceneOnChoice();
        }

    }

    private static void GoToMethScene()
    {
        managerInst.GoToScene(DrugVR_SceneENUM.Sc07B);
    }

    private static void GoToEndSceneOnChoice()
    {
        if (Scribe.Side05 && Scribe.Side06)
        {
            managerInst.GoToScene(DrugVR_SceneENUM.Sc08);               
        }
        else if (Scribe.Side05 || Scribe.Side06)
        {
            //ToDo: Different sound cues based on Side05 and Side06

            managerInst.GoToScene(DrugVR_SceneENUM.Sc09);
        }
        else if (!Scribe.Side05 || !Scribe.Side06)
        {
            managerInst.GoToScene(DrugVR_SceneENUM.Sc10);
        }

    }
}
