﻿using DrugVR_Scribe;
using UnityEngine;
using wvr;

public class HandLighterSwitchControl : MonoBehaviour
{    
    [SerializeField]
    private WVR_DeviceType deviceToListen = WVR_DeviceType.WVR_DeviceType_Controller_Right;
    [SerializeField]
    private WVR_InputId inputToListen = WVR_InputId.WVR_InputId_Alias1_Touchpad;

    [SerializeField]
    private LighterTriggerProgress lighterProgress;
    [SerializeField]
    private HandWaveProgressNew handWaveProgress;
    private bool modelSwitch = true;

    [SerializeField]
    private GameObject lighterObject;
    private Transform lighterTransform;
    //private readonly Quaternion LighterFixedQuaternion = Quaternion.Euler(0, 0, 0);

    [SerializeField]
    private float lighterHeadDistanceMultiplier;

    private MyControllerSwtich controllerSwitch;
    private LaserPointer controllerLaser;

    private WaveVR_Controller.Device waveVrDevice;
    private bool isLighterOn = false;

    private Transform headTransform;


    /* MonoBehaviour */

    

    private void Awake()
    {
        controllerSwitch = GameManager.Instance.ControllerSwitch;
        controllerLaser = controllerSwitch.LaserPointerRef;

        lighterTransform = lighterObject.transform;

        headTransform = GameManager.Instance.HeadObject.transform;

        GameManager.Instance.MenuControl.OnShowMenu += HandleShowMenu;
        GameManager.Instance.MenuControl.OnHideMenu += HandleHideMenu;
    }

    private void OnEnable()
    {
        handWaveProgress.OnSelectionComplete += HandleHandWaveSelectionComplete;
        lighterProgress.OnSelectionComplete += HandleLighterSelectionComplete;
    }

    private void OnDisable()
    {
        handWaveProgress.OnSelectionComplete -= HandleHandWaveSelectionComplete;
        lighterProgress.OnSelectionComplete -= HandleLighterSelectionComplete;
    }

    private void OnDestroy()
    {
        GameManager.OnSceneChange -= HandleSceneChange;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.MenuControl.OnShowMenu -= HandleShowMenu;
            GameManager.Instance.MenuControl.OnHideMenu -= HandleHideMenu; 
        }
    }

    private void Start()
    {
        GameManager.OnSceneChange += HandleSceneChange;

        lighterProgress.enabled = true;
        handWaveProgress.enabled = false;

        ReplaceControllerByLighter();        

        // The following statement is no longer needed
        // as update lighterTransform.position in the Update() method.
        //lighterTransform.parent = controllerPosTrkMan.transform;

        waveVrDevice = WaveVR_Controller.Input(deviceToListen);        
    }

    private void Update()
    {
        if (modelSwitch)
        {
            bool isPress = waveVrDevice.GetPress(inputToListen);

            // switch mode       
            if (isPress)
            {
                lighterProgress.enabled = false;
                handWaveProgress.enabled = true;
                lighterProgress.CheckAndFadeOut();

                ReplaceLighterByController();
            }
            else
            {
                lighterProgress.enabled = true;
                handWaveProgress.enabled = false;
                handWaveProgress.CheckAndFadeOutAndReset();

                ReplaceControllerByLighter();
            }
        }
        if (isLighterOn)
        {
            // update lighter transform
            lighterTransform.position = (controllerSwitch.ControllerTransform.position - headTransform.position) * lighterHeadDistanceMultiplier + headTransform.position;
            lighterTransform.rotation =
                Quaternion.LookRotation(lighterTransform.position - headTransform.position);
            // fix x-, z-axis rotation
            lighterTransform.rotation = Quaternion.Euler(0, lighterTransform.eulerAngles.y, 0);
        }
    }

    /* end of MonoBehaviour */


    private void ReplaceControllerByLighter()
    {
        if (!isLighterOn)
        {
            lighterObject.SetActive(true);

            // The following two statements are longer needed
            // as we no longer set the parent of lighterTransform to
            // the transform of FocusController GameObject.
            //controllerPT.TrackRotation = false;            
            //lighterTransform.rotation = LighterFixedQuaternion;

            controllerSwitch.HideController();
            
            isLighterOn = true;         
        }
    }

    private void ReplaceLighterByController()
    {
        if (isLighterOn)
        {
            lighterObject.SetActive(false);

            // The following two statements are longer needed
            // as we no longer set the parent of lighterTransform to
            // the transform of FocusController GameObject.
            //controllerPT.TrackRotation = true;

            controllerSwitch.ShowController(false, false);

            isLighterOn = false;
        }
    }


    /* event handlers */

    private void HandleSceneChange(DrugVR_SceneENUM nextScene)
    {
        ReplaceLighterByController();

        enabled = false;        
        Destroy(lighterObject);
    }

    private void HandleHandWaveSelectionComplete()
    {
        handWaveProgress.enabled = false;
        modelSwitch = false;
    }

    private void HandleLighterSelectionComplete()
    {
        lighterProgress.enabled = false;
        modelSwitch = false;
    }

    private void HandleShowMenu()
    {
        handWaveProgress.gameObject.SetActive(false);
        lighterProgress.gameObject.SetActive(false);
        enabled = false;
    }

    private void HandleHideMenu()
    {
        handWaveProgress.gameObject.SetActive(true);
        lighterProgress.gameObject.SetActive(true);
        enabled = true;
    }

    /* end of event handlers */
}
