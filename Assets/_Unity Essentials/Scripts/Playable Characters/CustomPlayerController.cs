using UnityEngine;

public class CustomPlayerController : MonoBehaviour
{
    private Health health;

    private void OnEnable()
    {
        health = GetComponent<Health>();

        if (health != null)
        {
            health.EventOnDeath += DestroyThisGameObject;
            health.EventOnDeath += LoadScene;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.EventOnDeath -= DestroyThisGameObject;
            health.EventOnDeath -= LoadScene;
        }
    }

    private void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }

    private void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
