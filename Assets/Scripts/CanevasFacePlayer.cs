using UnityEngine;

public class CanevasFacePlayer : MonoBehaviour
{
    private Transform player;  // Référence au joueur
    private Transform anchor;  // Point d’ancrage du Canvas
    private float maxDistance = 1f; // Rayon maximum autour de l'ancrage
    private float minDistance = 0.2f; // Distance minimale pour éviter que le Canvas soit trop proche

    private void Start()
    {
        // Trouver le joueur (assurez-vous qu'il a le tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Aucun joueur avec le tag 'Player' trouvé !");
        }

        // Créer un point d’ancrage au centre du parent
        if (transform.parent != null)
        {
            anchor = new GameObject("AnchorPoint").transform;
            anchor.SetParent(transform.parent);
            anchor.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Ce Canvas n'a pas de parent !");
        }
    }

    private void Update()
    {
        if (player != null && anchor != null)
        {
            // Calcul de la direction vers le joueur
            Vector3 directionToPlayer = (player.position - anchor.position).normalized;
            float distanceToPlayer = Vector3.Distance(anchor.position, player.position);

            // Calculer la position du Canvas basée sur la direction et la distance
            float distanceFromAnchor = Mathf.Clamp(distanceToPlayer, minDistance, maxDistance);

            // Si le joueur est à une distance proche, positionner le Canvas à la distance minimale
            // Sinon, positionner le Canvas à une distance maximale du joueur
            Vector3 targetPosition = anchor.position + directionToPlayer * distanceFromAnchor;

            // Mettre à jour la position du Canvas instantanément
            transform.position = targetPosition;

            // Faire en sorte que le Canvas regarde toujours le joueur
            transform.LookAt(player);
            transform.Rotate(0, 180, 0); // Ajustement pour que le texte soit lisible
        }
    }
}
