using UnityEngine;

public class GoalPortal : MonoBehaviour
{
    [Tooltip("Đối tượng sẽ bị dịch chuyển (thường là Player)")]
    public Transform targetToTeleport;

    [Tooltip("Vị trí đích (Transform)")]
    public Transform destination;

    [Tooltip("Chỉ teleport khi collider có tag này")]
    public string triggerTag = "Player";

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(triggerTag) && targetToTeleport != null && destination != null)
        {
            // Thiết lập vị trí
            targetToTeleport.position = destination.position;
        }
    }
}