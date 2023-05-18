using UnityEngine;
using UnityEngine.UI;

public class RisingLava : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float minLavaLevel = 0f;
    [SerializeField] float maxLavaLevel = 10f;
    [SerializeField] Slider lavaSlider;
    private AudioManager audioManager;
    private Vector2 direction = Vector2.up;
    private float currentLavaLevel;

    void Awake()
    {
        direction = direction.normalized;
        audioManager = GetComponent<AudioManager>();
    }

    void Start()
    {
        lavaSlider.minValue = minLavaLevel;
        lavaSlider.maxValue = maxLavaLevel;
        lavaSlider.onValueChanged.AddListener(OnLavaLevelChanged);
        audioManager.PlaySFX(0);
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);

        float setValueLava = Mathf.Clamp(transform.position.y, minLavaLevel, maxLavaLevel);
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
