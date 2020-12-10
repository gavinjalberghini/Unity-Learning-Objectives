using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HeadsetFollow : MonoBehaviour
{
    private Transform headset;

    protected virtual void Awake()
    {
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    private void OnEnable()
    {
        headset = VRTK_DeviceFinder.HeadsetTransform();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(headset.position.x, headset.position.y + 1, headset.position.z);
        transform.Rotate(transform.position, 0.5f);
    }
}
