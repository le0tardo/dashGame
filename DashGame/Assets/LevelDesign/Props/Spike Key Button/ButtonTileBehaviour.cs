using UnityEngine;

public class ButtonTileBehaviour : MonoBehaviour
{
    [SerializeField] bool isPressed=false;
    [SerializeField] float pressed_y = -1.15f;
    [SerializeField] AudioClip click;
    [SerializeField] ParticleSystem ring;

    [Header("Color Settings")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color pressedColor = Color.gray;
    private string colorPropertyName = "_BaseColor";
    [SerializeField] Renderer rend;
    private MaterialPropertyBlock propertyBlock;
    private int colorPropertyID;

    [SerializeField] GameObject getButtonInterface;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        colorPropertyID = Shader.PropertyToID(colorPropertyName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            if (other.CompareTag("Player"))
            {
                SetButtonColor(pressedColor);
                rend.gameObject.transform.position = new Vector3(transform.position.x, pressed_y, transform.position.z);
                if (click != null) AudioManager.inst.PlayCustomSound(click, 0.5f);
                if (ring != null) ring.Play();

                //button affect??
                if (getButtonInterface != null)
                {
                    IButtonAction gottenButtonInterface=getButtonInterface.GetComponent<IButtonAction>();
                    if (gottenButtonInterface != null) 
                    {
                        gottenButtonInterface.ButtonAction();
                    }

                }

                isPressed = true;
            }
        }
    }

    private void SetButtonColor(Color color)
    {
        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(colorPropertyID, color);
        rend.SetPropertyBlock(propertyBlock);
    }
}
