using UnityEngine;

public class WallShotBehaviour : MonoBehaviour
{

    Vector3 restPosition;
    [SerializeField] float speed=20f;
    public bool fired = false;

    private void Start()
    {
        restPosition = transform.localPosition;
    }

    public void FireArrow()
    {
        if (!fired) fired=true;
    }

    void StopArrow()
    {
        transform.position = restPosition;
    }

    private void Update()
    {
        if (fired)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("arrow hit player");
            StopArrow();
            fired = false;
        }
    }
}
