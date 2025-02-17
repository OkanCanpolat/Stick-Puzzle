using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Configurations / Sound")]

public class SoundConfig : ScriptableObject
{
    public AudioClip WinSound;
    [Range(0, 1)]
    public float WinVolume = 1;

    public AudioClip LoseSound;
    [Range(0, 1)]
    public float LoseVolume = 1;

    public AudioClip OutMovesSound;
    [Range(0, 1)]
    public float OutMovesVolume = 1;

    public AudioClip ButtonSound;
    [Range(0, 1)]
    public float ButtonVolume = 1;

    public AudioClip PaintSound;
    [Range(0, 1)]
    public float PaintVolume = 1;

    public AudioClip BlastSound;
    [Range(0, 1)]
    public float BlastVolume = 1;
}
