using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public Vector3 moveDirection=Vector3.zero;
    float speed = 16f;
    Vector3 startPos;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform myEnemy;

    void Awake()
    {
        myEnemy = transform.parent;
        GameObject bulletParent = GameObject.Find("BulletPool");
        if (bulletParent != null) { transform.SetParent(bulletParent.transform); }
        else {  transform.SetParent(null); }
    }
    public void SetDirection(Vector3 newDirection)
    {
        moveDirection = newDirection.normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * speed * Time.deltaTime;

            if (Physics.SphereCast(transform.position, transform.localScale.x, moveDirection, out RaycastHit pHit, 1, playerLayer))
            {
                print("bullet hit player");
                DisableBullet();

            }
            if (Physics.SphereCast(transform.position, transform.localScale.x, moveDirection, out RaycastHit wHit, 1, wallLayer))
            {
                print("bullet hit wall");
                DisableBullet();
            }
        }
    }

    private void DisableBullet()
    {
        transform.position = myEnemy.transform.position;
        moveDirection = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
