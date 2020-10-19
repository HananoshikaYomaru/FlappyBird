using UnityEngine;

public class SoundCleaner : MonoBehaviour
{
    private void Update()
    {
        SoundManager.Clean();
    }
}