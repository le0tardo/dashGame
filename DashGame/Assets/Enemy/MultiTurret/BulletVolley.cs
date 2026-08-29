using UnityEngine;

public class BulletVolley : MonoBehaviour
{
    [SerializeField] VolleyBullet[] bullets;

    [Header("Volley Pattern")]
    [SerializeField] float bulletSpeed=5f;
    [SerializeField] float spawnRadius=1f;
    [SerializeField] float spreadAngle=360f; 
    [SerializeField] float angleOffset=0f; //rotating spiral

    public void FireVolley(int amount, Vector3 centerPoint)
    {
        if (bullets == null || bullets.Length == 0) return;

        int count = bullets.Length;
        bool isFullCircle = Mathf.Approximately(spreadAngle, 360f);
        float angleStep = isFullCircle ? (360f / amount) : (spreadAngle / Mathf.Max(1, count - 1));
        float currentAngle = angleOffset - (isFullCircle ? 0f : (spreadAngle / 2f));

        if(amount>count)amount = count;

        for (int i = 0; i < amount; i++)
        {
            VolleyBullet bullet = bullets[i];
            if (bullet == null) continue;

            // Calculate direction on X-Z plane
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)).normalized;

            // Set spawn position and rotation
            bullet.transform.position = centerPoint + (dir * spawnRadius);
            bullet.transform.rotation = Quaternion.LookRotation(dir);

            // Pass direction and speed directly to the bullet's script
            bullet.Launch(dir, bulletSpeed);

            currentAngle += angleStep;

            Invoke("Sleep", 3.3f);
        }
    }

    void Sleep()
    {
        Vector3 resetPos=transform.position;
        foreach (var bullet in bullets)
        {
            transform.position = resetPos;
            bullet.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
