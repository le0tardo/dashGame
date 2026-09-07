using UnityEngine;

public class ButtonTileBehaviour : MonoBehaviour
{
    [SerializeField] bool toggleable;
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
            if (other.CompareTag("Player")||other.CompareTag("ButtonPresser"))
            {
                SetButtonColor(pressedColor);
                rend.gameObject.transform.position = new Vector3(transform.position.x, pressed_y, transform.position.z);
                if (click != null) AudioManager.inst.PlayCustomSound(click, 0.5f);
                if (ring != null) ring.Play();

                if (getButtonInterface != null)
                {
                    IButtonAction gottenButtonInterface=getButtonInterface.GetComponent<IButtonAction>();
                    if (gottenButtonInterface != null) 
                    {
                        gottenButtonInterface.ButtonAction(true);
                    }
                }

                isPressed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!toggleable) return;
        if (other.CompareTag("Player") || other.CompareTag("ButtonPresser"))
        {
            if (isPressed)
            {
                SetButtonColor(defaultColor);
                rend.gameObject.transform.position = new Vector3(transform.position.x, -1f, transform.position.z);

                if (click != null) AudioManager.inst.PlayCustomSound(click, 0.1f);
                if (ring != null) ring.Play();

                if (getButtonInterface != null)
                {
                    IButtonAction gottenButtonInterface = getButtonInterface.GetComponent<IButtonAction>();
                    if (gottenButtonInterface != null)
                    {
                        gottenButtonInterface.ButtonAction(false);
                    }
                }

                isPressed = false;
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
