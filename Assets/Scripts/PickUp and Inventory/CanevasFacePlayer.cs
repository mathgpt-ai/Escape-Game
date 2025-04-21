using UnityEngine;

public class CanevasFacePlayer : MonoBehaviour
{
    private Transform player;  // Référence au joueur
    private Transform anchor;  // Point d'ancrage du Canvas

    [SerializeField]
    private float fixedDistance = 0.5f; 

    private bool initialized = false;

    private void OnEnable()
    {
        InitializeReferences();
    }

    private void Start()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        if (initialized)
            return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Aucun joueur avec le tag 'Player' trouvé !");
        }

        if (transform.parent != null)
        {

            if (anchor == null)
            {
                GameObject anchorObj = new GameObject("AnchorPoint");
                anchor = anchorObj.transform;
                anchor.SetParent(transform.parent);
                anchor.localPosition = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("Ce Canvas n'a pas de parent !");
        }

        initialized = true;
    }

    private void Update()
    {
        // Vérifier si ce GameObject est toujours valide
        if (this == null || !this.gameObject.activeInHierarchy)
            return;

        // Vérifier que toutes les références sont valides
        if (!CheckReferences())
            return;

        // Calcul de la direction vers le joueur
        Vector3 directionToPlayer = (player.position - anchor.position).normalized;

        // Calculer la position du Canvas à une distance fixe de l'ancrage dans la direction du joueur
        Vector3 targetPosition = anchor.position + directionToPlayer * fixedDistance;

        // Mettre à jour la position du Canvas instantanément
        transform.position = targetPosition;

        // Faire en sorte que le Canvas regarde toujours le joueur
        transform.LookAt(player);
        transform.Rotate(0, 180, 0); // Ajustement pour que le texte soit lisible
    }

    private bool CheckReferences()
    {
        // Si des références sont nulles, essayer de les réinitialiser
        if (player == null || anchor == null)
        {
            InitializeReferences();
        }

        // Vérifier à nouveau après tentative de réinitialisation
        return player != null && anchor != null;
    }

    private void OnDestroy()
    {
        // Nettoyer l'ancrage si on supprime ce composant
        if (anchor != null)
        {
            Destroy(anchor.gameObject);
        }
    }
}