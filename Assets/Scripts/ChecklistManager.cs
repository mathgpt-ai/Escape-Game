using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChecklistManager : MonoBehaviour
{
   
    public static ChecklistManager Instance;

    [SerializeField]
    private TextMeshProUGUI[] taskTexts;
    [SerializeField]
    private TextMeshProUGUI texteFinalInstruction;

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
        for (int i = 0; i < taskTexts.Length; i++)
        { 
            if (PlayerPrefs.GetInt("Enigme_"+i, 0) == 0)
            {
                return i;
            }
        }
        // Tout est terminé
        return taskTexts.Length;
    }

    private void MettreAJourChecklist()
    {
        for (int i = 0; i < taskTexts.Length; i++)
        {
            taskTexts[i].gameObject.SetActive(i == currentTaskIndex);
        }
    }

    public void MarquerEnigmeDone(int i)
    {
        if (i == currentTaskIndex && i < taskTexts.Length)
        {
            StartCoroutine(TransitionVersProchaineTache(i));
        }
    }

    private IEnumerator TransitionVersProchaineTache(int i)
    {
        TextMeshProUGUI texte = taskTexts[i];
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

        if (currentTaskIndex < taskTexts.Length)
        {
            TextMeshProUGUI prochainTexte = taskTexts[currentTaskIndex];
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
        else
        {
            // Toutes les tâches sont faites — afficher le texte final
            if (texteFinalInstruction != null)
            {
                CanvasGroup finalGroup = texteFinalInstruction.GetComponent<CanvasGroup>();
                if (finalGroup == null)
                {
                    finalGroup = texteFinalInstruction.gameObject.AddComponent<CanvasGroup>();
                }

                finalGroup.alpha = 0;
                texteFinalInstruction.gameObject.SetActive(true);

                time = 0;
                while (time < duration)
                {
                    time += Time.deltaTime;
                    finalGroup.alpha = Mathf.Lerp(0, 1, time / duration);
                    yield return null;
                }

                finalGroup.alpha = 1;
            }
        }
    }

    public void ResetChecklist()
    {
        for (int i = 0; i < taskTexts.Length; i++)
        {
            PlayerPrefs.DeleteKey("Enigme_" + i);
        }
        PlayerPrefs.Save();
    }
}
