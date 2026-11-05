using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensibilità Mouse")]
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 startingRotation;

    void Start()
    {
        // Blocca il cursore del mouse al centro dello schermo e lo nasconde
        Cursor.lockState = CursorLockMode.Locked;
        
        // Salviamo la rotazione iniziale che hai impostato nell'editor
        startingRotation = transform.localEulerAngles;
        xRotation = startingRotation.x;
        yRotation = startingRotation.y;
    }

    void Update()
    {
        // Cattura il movimento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calcola la rotazione verticale (su e giù) e la blocca per non far "ribaltare" la testa
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // Calcola la rotazione orizzontale (destra e sinistra)
        yRotation += mouseX;

        // Applica la rotazione alla telecamera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}