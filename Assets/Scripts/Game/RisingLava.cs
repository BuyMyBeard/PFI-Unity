using UnityEngine;
using UnityEngine.UI;

public class RisingLava : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float minLavaLevel = 0f;
    [SerializeField] float maxLavaLevel = 10f;
    [SerializeField] Slider lavaSlider;

    private AudioManager audioManager;
    AudioSource audioSource;
    private Vector2 direction = Vector2.up;
    private float currentLavaLevel;

    void Awake()
    {
        direction = direction.normalized;
        audioManager = GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        lavaSlider.minValue = minLavaLevel;
        lavaSlider.maxValue = maxLavaLevel; //changer pour le top du niveau
        lavaSlider.onValueChanged.AddListener(OnLavaLevelChanged);
        audioSource.loop = true;
        //audioManager.PlaySFX(0);
        audioSource.Play();
        
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);

        float setValueLava = transform.position.y;
        if (currentLavaLevel != setValueLava)
        {
            currentLavaLevel = setValueLava;
            lavaSlider.value = currentLavaLevel;
        }
    }

    void OnLavaLevelChanged(float value)
    {
        currentLavaLevel = value;
        transform.position = new Vector3(transform.position.x, currentLavaLevel, transform.position.z);
    }
}
