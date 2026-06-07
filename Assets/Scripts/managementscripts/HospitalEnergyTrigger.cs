using UnityEngine;

public class HospitalEnergyTrigger : MonoBehaviour
{
    public int hospitalEnergyPoints = 500;
    public Vector3 returnPosition = new Vector3(400f, 0.05f, 35f);

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            TopBarManager topBar = FindObjectOfType<TopBarManager>();
            if (topBar != null)
            {
                topBar.AddScore(hospitalEnergyPoints);
                Debug.Log("Player received 500 hospital energy points");
            }

            PlayerRoadMovement player = other.GetComponent<PlayerRoadMovement>();
            if (player != null)
            {
                player.transform.position = returnPosition;

                player.ReachClinic();
                Debug.Log("Remy is now running after treatment");

                FoodSpawner spawner = FindObjectOfType<FoodSpawner>();
                if (spawner != null)
                {
                    spawner.StartFoodSpawning();
                    Debug.Log("Food spawning started after medication");
                }
            }
            else
            {
                Debug.LogError("PlayerRoadMovement not found on Remy");
            }
        }
    }
}