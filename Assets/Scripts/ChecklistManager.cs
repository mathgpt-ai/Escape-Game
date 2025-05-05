using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChecklistManager : MonoBehaviour
{
   
    public static ChecklistManager Instance;

    [SerializeField]
    private TextMeshProUGUI[] enigmeTexts;

    private int currentTaskIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Cacher toutes les tâches sauf la première non terminée
        currentTaskIndex = TrouverTacheActuelle();
        Debug.Log("Index tâche actuelle : " + currentTaskIndex);
        MettreAJourChecklist();
    }

    private int TrouverTacheActuelle()
    {
        for (int i = 0; i < enigmeTexts.Length; i++)
        {
            if (PlayerPrefs.GetInt("Enigme_", 0) == 0)
            {
                return i;
            }
        }
        // Tout est terminé
        return enigmeTexts.Length;
    }

    private void MettreAJourChecklist()
    {
        for (int i = 0; i < enigmeTexts.Length; i++)
        {
            enigmeTexts[i].gameObject.SetActive(i == currentTaskIndex);
        }
    }

    public void MarquerEnigmeDone(int i)
    {
        if (i == currentTaskIndex && i < enigmeTexts.Length)
        {
            StartCoroutine(TransitionVersProchaineTache(i));
        }
    }

    private IEnumerator TransitionVersProchaineTache(int i)
    {
        TextMeshProUGUI texte = enigmeTexts[i];
        CanvasGroup group = texte.GetComponent<CanvasGroup>();

        if (group == null)
        {
            group = texte.gameObject.AddComponent<CanvasGroup>();
        }

        // Faire un fade out
        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }

        group.alpha = 0;
        texte.gameObject.SetActive(false);

        // Passer à la prochaine tâche
        currentTaskIndex++;

        if (currentTaskIndex < enigmeTexts.Length)
        {
            TextMeshProUGUI prochainTexte = enigmeTexts[currentTaskIndex];
            CanvasGroup prochainGroup = prochainTexte.GetComponent<CanvasGroup>();

            if (prochainGroup == null)
            {
                prochainGroup = prochainTexte.gameObject.AddComponent<CanvasGroup>();
            }

            prochainGroup.alpha = 0;
            prochainTexte.gameObject.SetActive(true);

            // Fade in
            time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                prochainGroup.alpha = Mathf.Lerp(0, 1, time / duration);
                yield return null;
            }
            prochainGroup.alpha = 1;
        }
    }

    public void ResetChecklist()
    {
        for (int i = 0; i < enigmeTexts.Length; i++)
        {
            PlayerPrefs.DeleteKey("Enigme_" + i);
        }
        PlayerPrefs.Save();
    }
}
