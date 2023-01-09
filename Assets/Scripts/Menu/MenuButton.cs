using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void OnPointerClick(AudioClip clip)
    {
        AudioManager.Instance.PlaySound(clip.name);
    }
}