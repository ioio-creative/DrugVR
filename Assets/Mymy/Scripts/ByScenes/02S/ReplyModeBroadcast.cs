﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class ReplyModeBroadcast : MonoBehaviour
{
    public enum ReplyMode
    {
        Reply,
        NotReply,
        Null
    }


    public event Action<ReplyMode> OnReplyModeIndicated;
    public bool IsReplyModeIndicated { get { return m_IsReplyModeIndicated; } }


    [SerializeField]
    private float m_XDisplacementThreshold;
    [SerializeField]
    private float m_YDisplacementThreshold;
    [SerializeField]
    private RectTransform m_RectTransformToListen;
    [SerializeField]
    private Image m_PopupMsgImage;
    private bool m_IsReplyModeIndicated = false;
    private Vector3 m_OriginalRectTransformPos;


    /* MonoBahaviour */

    private void Start()
    {
        m_OriginalRectTransformPos = m_RectTransformToListen.position;
    }

    private void Update()
    {
        //Debug.Log("m_RectTransformToListen.position.x: " + m_RectTransformToListen.position.x);
        //Debug.Log("m_RectTransformToListen.position.y: " + m_RectTransformToListen.position.y);

        ReplyMode replyMode = ReplyMode.Null;

        // set replyMode based on where m_RectTransformToListen is
        // "swiped" to
        if (m_RectTransformToListen.position.x > m_XDisplacementThreshold)
        {
            replyMode = ReplyMode.NotReply;
        }
        else if (m_RectTransformToListen.position.y > m_YDisplacementThreshold)
        {
            replyMode = ReplyMode.Reply;
        }

        // broadcast replyMode if m_RectTransformToListen is "swiped"
        // beyond certain thresholds
        if (replyMode != ReplyMode.Null)
        {
            // make m_PopupMsgImage transparent
            Color popupMsgImageOriginalColor = m_PopupMsgImage.color;
            m_PopupMsgImage.color = new Color
            (
                popupMsgImageOriginalColor.r,
                popupMsgImageOriginalColor.g,
                popupMsgImageOriginalColor.b,
                0
            );

            // TODO: Maybe we can play some sound here!

            if (OnReplyModeIndicated != null)
            {
                OnReplyModeIndicated(replyMode);
            }
        }
    }

    /* end of MonoBahaviour */


    public void Reset()
    {
        m_RectTransformToListen.position = m_OriginalRectTransformPos;
    }
}