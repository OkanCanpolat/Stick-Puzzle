using UnityEngine;
using Zenject;

public class Background : MonoBehaviour
{
    [SerializeField] private float scaleOffset = 1;
    [Inject] private SignalBus signalBus;
    [Inject] private GridManager gridManager;
    private void Awake()
    {
        signalBus.Subscribe<GridCreatedSignal>(OnGridComplete);
    }
    public void OnGridComplete()
    {
        float x = (gridManager.Column - 1) / 2f;
        float y = (gridManager.Row - 1) / 2f;

        Vector3 pos = new Vector3(x, y, 0);
        transform.position = pos;
        Vector3 scale = new Vector3(gridManager.Column - 1 + scaleOffset, gridManager.Row - 1 + scaleOffset, 1);
        transform.localScale = scale;
    }
}
