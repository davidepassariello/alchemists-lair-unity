using UnityEngine;

// Questo script va attaccato alla MAIN CAMERA.
// Si occupa di sparare un raycast in avanti ogni frame e di capire
// se stiamo "guardando" un oggetto interattivo (che ha il componente AlchemistItem).
public class GazeDetector : MonoBehaviour
{
    [Header("Impostazioni Raycast")]
    [Tooltip("Distanza massima a cui il raggio riesce a rilevare oggetti")]
    public float maxGazeDistance = 20f;

    [Tooltip("Layer da considerare per il raycast (lascia Everything se non hai layer particolari)")]
    public LayerMask gazeLayerMask = ~0; // ~0 = tutti i layer

    // Riferimento all'oggetto attualmente guardato (Modificato da GazeInteractable a AlchemistItem)
    private AlchemistItem currentTarget = null;

    void Update()
    {
        // Raggio che parte dal centro della camera e va avanti
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxGazeDistance, gazeLayerMask))
        {
            // Proviamo a prendere il componente AlchemistItem dall'oggetto colpito
            AlchemistItem interactable = hit.collider.GetComponent<AlchemistItem>();

            if (interactable != null)
            {
                if (interactable == currentTarget)
                {
                    // Stiamo ancora guardando lo stesso oggetto: aggiorniamo il timer
                    interactable.OnGazeStay(Time.deltaTime);
                }
                else
                {
                    // Abbiamo cambiato bersaglio: resettiamo il precedente e iniziamo il nuovo
                    ClearCurrentTarget();
                    currentTarget = interactable;
                }
                return;
            }
        }

        // Se arriviamo qui: il raggio non ha colpito nulla di interattivo
        ClearCurrentTarget();
    }

    private void ClearCurrentTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.OnGazeExit();
            currentTarget = null;
        }
    }

    // Disegna il raggio nella Scene view solo per debug (visibile quando l'oggetto Camera è selezionato)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxGazeDistance);
    }
}