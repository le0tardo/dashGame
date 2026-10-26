using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager inst;

    public static event Action OnRoomChanged;

    [SerializeField] public Transform currentRoom;
    [SerializeField] public List<Transform> visitedRooms;
    [SerializeField] public float roomDifficulty = 0;
    [SerializeField] GameObject cameraPivot;
    [SerializeField] DoorwayLightScript doorwayLights;

    [SerializeField] float cameraLerpT = 0.25f;
    Coroutine activePan;

    private void Awake()
    {
        inst = this;
    }
    public void ChangeRoom(Transform newRoom)
    {
        StopAllCoroutines();

        Vector3 newRoomPosition = new Vector3
            (
                newRoom.transform.position.x,
                cameraPivot.transform.position.y,
                newRoom.transform.position.z
            );


        doorwayLights.SnapLightsToNewRoom(newRoomPosition);

        //cameraPivot.transform.position = newRoomPosition; //snap to 
        if (activePan != null)
        {
            StopCoroutine(activePan);
        }
        activePan = StartCoroutine(PanToRoom(newRoomPosition));

        OnRoomChanged?.Invoke(); //this triggers listeners? like petcombat 

    }

    public void AddRoomToList(Transform room)
    {
        if (!visitedRooms.Contains(room))
        {
            visitedRooms.Add(room);
            RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();
            if (roomBehaviour.enemies.Count > 0) roomDifficulty++;
        }
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

        doorwayLights.UpdateDoorwayLights();

        activePan = null;

    }
}
