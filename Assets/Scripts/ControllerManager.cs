using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum ControllerState
{
    IDLE,
    GRABBING,
    CLIMBING
}

public class ControllerManager : MonoBehaviour
{
    public XRController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
    }
}
