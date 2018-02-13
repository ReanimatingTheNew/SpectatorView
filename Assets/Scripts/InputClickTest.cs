﻿using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class InputClickTest : MonoBehaviour,
                              IInputClickHandler
{
    public UnityEvent onClick;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }
}