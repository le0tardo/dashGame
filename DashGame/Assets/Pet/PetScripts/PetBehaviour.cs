using System.Collections.Generic;
using UnityEngine;

public class PetBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerTransform;
    [SerializeField] float distanceToPlayer;
    Vector3 desiredPosition;
    [SerializeField]Vector3 currentVelocity;

    [Header("Movement")]
    [SerializeField]float smoothTime = 0.33f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float stopDist = 0.5f;

    [Header("Combat")]
    [SerializeField] float attackPower=1f;
    [SerializeField] float attackSpeed=1f;
    [SerializeField] List<GameObject>targets = new List<GameObject>();
    [SerializeField] GameObject target=null;


    private void Update()
    {
        desiredPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        
        distanceToPlayer=Vector3.Distance(transform.position,desiredPosition);

        if (distanceToPlayer > stopDist)
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        transform.position = Vector3.SmoothDamp(transform.position,desiredPosition,ref currentVelocity, smoothTime, maxSpeed);
    }

    public void NewRoom() //how to call this..?
    {
        RoomBehaviour room = RoomManager.inst.currentRoom.GetComponent<RoomBehaviour>();
        if (room != null)
        {
            print("this room contains " + room.enemies.Count + " enemies");
        }
    }
}
