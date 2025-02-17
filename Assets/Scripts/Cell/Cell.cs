using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Cell : MonoBehaviour
{
    [HideInInspector] public int Column;
    [HideInInspector] public int Row;
    [HideInInspector] public bool IsPainted;
    [HideInInspector] public bool IsShining;
    [SerializeField] private GameObject shineObject;
    [SerializeField] private GameObject blastAnimationObject;
    [Inject] private BlastConfig blastConfig;
    [Inject] private SoundConfig soundConfig;
    [Inject] private IAudioService audioService;
    private List<Line> neighbourLines;
    private SpriteRenderer spriteRenderer;
    private float scaleTime = 0.5f;

    public List<Line> NeighbourLines => neighbourLines;
    private void Awake()
    {
        neighbourLines = new List<Line>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ControlPaint(Color color)
    {
        foreach (var item in neighbourLines)
        {
            if (item.IsEmpty == true) return;
        }

        foreach (var line in neighbourLines)
        {
            line.ConnectedCellCount++;
        }

        audioService.PlaySoundOnce(soundConfig.PaintSound, soundConfig.PaintVolume);
        spriteRenderer.enabled = true;
        IsPainted = true;
        spriteRenderer.color = color;
        iTween.ScaleFrom(gameObject, iTween.Hash("scale", Vector3.zero, "time", scaleTime, "easetype", iTween.EaseType.easeOutCirc));
    }
    public void ControlPaintDummy()
    {
        foreach (var item in neighbourLines)
        {
            if (item.IsEmpty == true) return;
        }
        IsPainted = true;
    }
    public void RevertDummyPaint()
    {
        if (!IsPainted) return;

        foreach (var item in neighbourLines)
        {
            if (item.IsEmpty == true)
            {
                IsPainted = false;
                return;
            }
        }
    }
    public void RegisterLines(params Line[] lines)
    {
        foreach (var line in lines)
        {
            neighbourLines.Add(line);
        }
    }

    public void Blast()
    {
        StartCoroutine(CoBlast());
    }
    public void Shine(bool value)
    {
        IsShining = value;
        shineObject.SetActive(value);
    }

    private IEnumerator CoBlast()
    {
        IsPainted = false;
        blastAnimationObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", blastConfig.AlphaAnimationTime, "onupdate", "OnAlphaChange"));
        foreach (var line in neighbourLines)
        {
            line.ConnectedCellCount--;

            if (!line.IsEmpty && line.ConnectedCellCount == 0)
            {
                line.IsEmpty = true;
                line.OnDisplacement();
            }
        }
        yield return new WaitForSeconds(blastConfig.AlphaAnimationTime);

        spriteRenderer.enabled = false;
        blastAnimationObject.SetActive(false);
        OnAlphaChange(1f);
    }
    private void OnAlphaChange(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
