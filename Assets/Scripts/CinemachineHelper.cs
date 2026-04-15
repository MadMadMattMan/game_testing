using Unity.Cinemachine;
using UnityEngine;

public class CinemachineHelper : MonoBehaviour
{
    private void Awake() {
        GetComponent<CinemachineCamera>().Target.TrackingTarget = GameObject.FindWithTag("Player").transform;
    }
}
