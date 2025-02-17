using UnityEngine;
using Zenject;

public class CameraResizer : MonoBehaviour
{
    [SerializeField] private float padding;
    [SerializeField] private float cameraZOffset = -10;
    [Inject] private GridManager grid;
    [Inject] private SignalBus signalBus;
    private Camera mainCam;
    private void Awake()
    {
        mainCam = GetComponent<Camera>();
        signalBus.Subscribe<GridCreatedSignal>(SetupCamera);
    }

    private void SetupCamera()
    {
        float aspectRatio = Screen.width / (float)Screen.height;

        float width = grid.Column;
        float height = grid.Row;

        mainCam.transform.position = new Vector3((width - 1) / 2, (height - 1) / 2, cameraZOffset);

        if (width >= height)
        {
            mainCam.orthographicSize = (width / 2f + padding) / aspectRatio;
        }

        else
        {
            float size = (width / 2f + padding) / aspectRatio;
            Debug.Log(aspectRatio);
            if (size <= height / 2f)
            {
                mainCam.orthographicSize = height + padding;
            }

            else
            {
                mainCam.orthographicSize = (width / 2f + padding) / aspectRatio;
            }
        }
    }
}
