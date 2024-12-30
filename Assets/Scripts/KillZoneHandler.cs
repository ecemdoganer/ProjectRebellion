using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene

public class KillZoneHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the Kill Zone
        if (other.CompareTag("Player"))
        {
            // Restart the scene (reloads from the start point)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}