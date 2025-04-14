using UnityEngine;

public class CanevasFacePlayer : MonoBehaviour
{
    private Transform player;  // R�f�rence au joueur
    private Transform anchor;  // Point d'ancrage du Canvas

    [SerializeField]
    private float fixedDistance = 0.5f; // Distance fixe et personnalisable entre le Canvas et le joueur

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

        // Trouver le joueur (assurez-vous qu'il a le tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Aucun joueur avec le tag 'Player' trouv� !");
        }

        // Cr�er un point d'ancrage au centre du parent
        if (transform.parent != null)
        {
            // V�rifie si anchor existe d�j�
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
        // V�rifier si ce GameObject est toujours valide
        if (this == null || !this.gameObject.activeInHierarchy)
            return;

        // V�rifier que toutes les r�f�rences sont valides
        if (!CheckReferences())
            return;

        // Calcul de la direction vers le joueur
        Vector3 directionToPlayer = (player.position - anchor.position).normalized;

        // Calculer la position du Canvas � une distance fixe de l'ancrage dans la direction du joueur
        Vector3 targetPosition = anchor.position + directionToPlayer * fixedDistance;

        // Mettre � jour la position du Canvas instantan�ment
        transform.position = targetPosition;

        // Faire en sorte que le Canvas regarde toujours le joueur
        transform.LookAt(player);
        transform.Rotate(0, 180, 0); // Ajustement pour que le texte soit lisible
    }

    private bool CheckReferences()
    {
        // Si des r�f�rences sont nulles, essayer de les r�initialiser
        if (player == null || anchor == null)
        {
            InitializeReferences();
        }

        // V�rifier � nouveau apr�s tentative de r�initialisation
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