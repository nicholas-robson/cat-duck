using UnityEngine;

public class Exit : MonoBehaviour
{
    public string nextScene;
    public int entranceIndex;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            GameManager.LoadScene(nextScene, entranceIndex);
        }
    }
}
