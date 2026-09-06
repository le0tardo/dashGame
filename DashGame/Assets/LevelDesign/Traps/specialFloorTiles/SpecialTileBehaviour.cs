using UnityEngine;

public class SpecialTileBehaviour : MonoBehaviour
{
    enum TileType
    {
        Turn,
        Multiply,
        Divide,
        Stop
    }

    [SerializeField] TileType tileType;

    [SerializeField] PlayerMove player;

    [Header("glow")]
    [SerializeField] private Renderer tileRenderer;
    [SerializeField] private Color black = Color.black;
    [SerializeField] private Color white = Color.white;
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioClip tileSound;
    private string emissionPropertyName = "_EmissionColor";
    private MaterialPropertyBlock propertyBlock;
    private int emissionPropertyID;
    private void Awake()
    {
        if (tileRenderer == null)
        {
            tileRenderer = GetComponentInChildren<Renderer>();
        }

        propertyBlock = new MaterialPropertyBlock();
        emissionPropertyID = Shader.PropertyToID(emissionPropertyName);

        SetEmissionColor(black);
    }

    private void Start()
    {
        player = LevelManager.inst.playerMove;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (tileType)
            {
                case TileType.Turn:
                    TurnTile();
                    break;
                case TileType.Multiply:
                    MultiplyTile();
                    break;
                case TileType.Divide:
                    DivideTile();
                    break;
                case TileType.Stop:
                    StopTile();
                    break;
            }

            SetEmissionColor(white);
            if(particles!=null)particles.Play();
            if (tileSound != null) AudioManager.inst.PlayCustomSound(tileSound,0.5f);
        }
    }

    void TurnTile()
    {
        if (player != null)
        {
            player.transform.rotation = transform.rotation;
            float currentSpeed = player.currentVelocity.magnitude;
            player.currentVelocity = transform.forward * currentSpeed;
        }
    }

    void MultiplyTile()
    {
        if (player != null)
        {
            player.currentVelocity = Vector3.ClampMagnitude(player.currentVelocity * 4f, player.maxVelocity);
        }
    }
    void DivideTile()
    {
        if (player != null)
        {
            if (player.currentVelocity.magnitude > 0)
            {
                player.currentVelocity *= 0.25f;
            }
        }
    }

    void StopTile()
    {
        if (player != null)
        {
            player.currentVelocity=Vector3.zero;
        }
    }

    private void SetEmissionColor(Color color)
    {
        tileRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(emissionPropertyID, color);
        tileRenderer.SetPropertyBlock(propertyBlock);

        Invoke("ResetEmissionColor", 0.25f);
    }

    void ResetEmissionColor()
    {
        tileRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(emissionPropertyID, black);
        tileRenderer.SetPropertyBlock(propertyBlock);
    }
}
