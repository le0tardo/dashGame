using UnityEngine;
public interface IHittable
{
    public float hitBounce { get; }
    void OnHit(Vector3 hitPosition, float power);
}
