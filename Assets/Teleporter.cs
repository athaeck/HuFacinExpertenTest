using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class Teleporter : MonoBehaviour
{
    public GameObject teleportPositions;
    public int index;

    public InputActionAsset actionAsset;
    public TeleportationProvider provider;

    public InputAction thumbstick;
    public XRRayInteractor rayInteractor;
    private bool isActive;


    public void Start()
    {
        rayInteractor.enabled = false;

        var StaticTeleport = actionAsset.FindActionMap("XRI Righthand Locomotion").FindAction("StaticTeleport");
        StaticTeleport.performed += StaticTeleportEvent;
        /*
        var activate = actionAsset.FindActionMap("XRI Righthand Locomotion").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI Righthand Locomotion").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTelePortCancel;

        thumbstick = actionAsset.FindActionMap("XRI Righthand Locomotion").FindAction("Move");
        thumbstick.Enable();
        */
    }
    /*
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        if (thumbstick.triggered)
        {
            return;
        }
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.enabled = false;
                return;         
        }
        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point
        };

        provider.QueueTeleportRequest(request);
    }
    */
    /*
    void onDestroy()
    {
        StaticTeleport.performed -= StaticTeleportEvent;
        TeleportActivate.performed -= OnTeleportActivate;
        TeleportCancel.performed -= OnTelePortCancel;
    }
    */

    void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        isActive = true;

    }
    void OnTelePortCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        isActive = false;
    }




    void StaticTeleportEvent(InputAction.CallbackContext context)
    {
        Debug.Log(index);
        if (index >= teleportPositions.transform.childCount)
        {
            index = 0;
        }

        gameObject.transform.position = teleportPositions.transform.GetChild(index).transform.position; 
        index++;
        Debug.Log("eeee");
    }   

}
