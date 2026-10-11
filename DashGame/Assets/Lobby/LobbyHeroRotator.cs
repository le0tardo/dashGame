using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyHeroRotator : MonoBehaviour,IDragHandler
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float rotationSpeed = 0.4f;
    [SerializeField] private bool invertRotation = false;


    public void OnDrag(PointerEventData eventData)
    {
        if (characterTransform == null) return;

        float direction = invertRotation ? eventData.delta.x : -eventData.delta.x;
        float rotationAmount = direction * rotationSpeed;

        characterTransform.Rotate(Vector3.up, rotationAmount, Space.World);
    }
}
