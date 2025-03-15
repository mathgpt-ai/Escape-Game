using UnityEngine;

public class CanevasFacePlayer : MonoBehaviour
{
    private Transform player;  // R�f�rence au joueur
    private Transform anchor;  // Point d�ancrage du Canvas
    private float maxDistance = 1f; // Rayon maximum autour de l'ancrage
    private float minDistance = 0.2f; // Distance minimale pour �viter que le Canvas soit trop proche

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
            Debug.LogWarning("Aucun joueur avec le tag 'Player' trouv� !");
        }

        // Cr�er un point d�ancrage au centre du parent
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

            // Calculer la position du Canvas bas�e sur la direction et la distance
            float distanceFromAnchor = Mathf.Clamp(distanceToPlayer, minDistance, maxDistance);

            // Si le joueur est � une distance proche, positionner le Canvas � la distance minimale
            // Sinon, positionner le Canvas � une distance maximale du joueur
            Vector3 targetPosition = anchor.position + directionToPlayer * distanceFromAnchor;

            // Mettre � jour la position du Canvas instantan�ment
            transform.position = targetPosition;

            // Faire en sorte que le Canvas regarde toujours le joueur
            transform.LookAt(player);
            transform.Rotate(0, 180, 0); // Ajustement pour que le texte soit lisible
        }
    }
}
