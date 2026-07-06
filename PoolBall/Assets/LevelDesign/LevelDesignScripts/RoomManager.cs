using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager inst;
    [SerializeField] public Transform currentRoom;
    [SerializeField] GameObject cameraPivot;

    [SerializeField] float cameraLerpT = 0.25f;
    Coroutine activePan;

    private void Awake()
    {
        inst = this;
    }
    public void ChangeRoom(Transform newRoom)
    {
        Vector3 newRoomPosition = new Vector3
            (
                newRoom.transform.position.x,
                cameraPivot.transform.position.y,
                newRoom.transform.position.z
            );
        //cameraPivot.transform.position = newRoomPosition; //snap to 
        if (activePan != null)
        {
            StopCoroutine(activePan);
        }
        activePan = StartCoroutine(PanToRoom(newRoomPosition));
    }

    private System.Collections.IEnumerator PanToRoom(Vector3 targetPosition)
    {
        Vector3 startPosition = cameraPivot.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < cameraLerpT)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float percent = Mathf.Clamp01(elapsedTime / cameraLerpT);

            float smoothPercent = Mathf.SmoothStep(0f, 1f, percent);

            cameraPivot.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothPercent);
            yield return null;
        }

        cameraPivot.transform.position = targetPosition;
        activePan = null;
    }
}
