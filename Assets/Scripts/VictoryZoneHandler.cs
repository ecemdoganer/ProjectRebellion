using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene

public class VictoryZoneHandler : MonoBehaviour
{
    public GameObject victoryText; 
    public float displayDuration = 5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            if (victoryText != null)
            {
                victoryText.SetActive(true); 
            }

            
            Invoke("RestartGame", displayDuration);
        }
    }

    private void RestartGame()
    {
        // Disable the "YOU WON" text before restarting
        
        
            victoryText.SetActive(false); 
        

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}