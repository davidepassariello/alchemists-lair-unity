using UnityEngine;

public class AlchemistItem : MonoBehaviour
{
    [Header("Soglie di tempo")]
    public float shortGazeThreshold = 1.0f;
    public float longGazeThreshold = 2.5f;

    [Header("Effetti sguardo breve")]
    public Light itemLight;
    public Color glowColor = new Color(0.5f, 0.2f, 1f);
    public float shortGazeLightIntensity = 2.0f;

    [Header("Effetti sguardo prolungato (standard)")]
    public float longGazeLightIntensity = 5.0f;
    public float floatHeight = 0.4f;
    public float rotationSpeed = 40.0f;
    public Vector3 rotationAxis = Vector3.up; 
    public ParticleSystem itemParticles;

    [Header("Modalità horror (teschio)")]
    public bool isHorrorMode = false;
    public float approachDistance = 1.5f; // Quanto viene vicino in avanti

    [Header("Audio")]
    public AudioClip soundLong;

    // Stato interno
    private float currentGazeTime = 0f;
    private bool isHovered = false;
    private bool isActivated = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private AudioSource audioSource;
    private float targetLightIntensity = 0f;
    private Transform playerCamera;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        playerCamera = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (itemLight != null)
        {
            itemLight.intensity = 0f;
            itemLight.color = glowColor;
        }

        if (itemParticles != null) itemParticles.Stop();
    }

    void Update()
    {
        if (itemLight != null)
        {
            itemLight.intensity = Mathf.Lerp(itemLight.intensity, targetLightIntensity, Time.deltaTime * 3f);
        }

        if (isActivated)
        {
            if (isHorrorMode)
            {
                // HORROR MODE: Si avvicina alla telecamera e la fissa all'improvviso
                Vector3 directionToCamera = (playerCamera.position - startPosition).normalized;
                Vector3 targetPos = startPosition + directionToCamera * approachDistance;
                targetPos.y += floatHeight; // Si alza comunque un pochino

                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 4f);
                
                // Rotazione veloce verso gli occhi del giocatore
                Quaternion targetRot = Quaternion.LookRotation(playerCamera.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 8f);
            }
            else
            {
                // STANDARD MODE: fluttua e ruota (libro e moneta)
                Vector3 targetPos = startPosition + Vector3.up * floatHeight;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2f);
                transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Ritorna alla posizione iniziale
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * 3f);
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * 3f);
        }
    }

    public void OnGazeStay(float deltaTime)
    {
        currentGazeTime += deltaTime;

        if (!isHovered && currentGazeTime >= shortGazeThreshold)
        {
            isHovered = true;
            targetLightIntensity = shortGazeLightIntensity;
        }

        if (!isActivated && currentGazeTime >= longGazeThreshold)
        {
            isActivated = true;
            targetLightIntensity = longGazeLightIntensity;
            if (soundLong != null) audioSource.PlayOneShot(soundLong);
            if (itemParticles != null) itemParticles.Play();
        }
    }

    public void OnGazeExit()
    {
        currentGazeTime = 0f;
        isHovered = false;
        isActivated = false;
        targetLightIntensity = 0f;
        if (itemParticles != null) itemParticles.Stop();
    }
}