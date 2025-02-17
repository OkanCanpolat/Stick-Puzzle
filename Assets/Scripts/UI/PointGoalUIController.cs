using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointGoalUIController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text pointGoalText;
    [SerializeField] private TMP_Text currentPointText;
    [SerializeField] private float sliderSpeed = 1f;
    private PointGoal pointGoal;
    private int targetPoint;
    
    public void Init(PointGoal pointGoal)
    {
        this.pointGoal = pointGoal;
        pointGoal.OnPointChanged += Slide;
        slider.maxValue = targetPoint = pointGoal.GoalConfig.TargetPoint;
        targetPoint = pointGoal.GoalConfig.TargetPoint;
        pointGoalText.text = targetPoint.ToString();
        currentPointText.text = "0";
        slider.value = 0f;
    }

    public void OnPointChanged(float point)
    {
        currentPointText.text = ((int)point).ToString();
        slider.value = point;
    }
    private void Slide(int point)
    {
        StartCoroutine(CoSlide(point));
    }
    private IEnumerator CoSlide(int point)
    {
        float currentValue = slider.value;
        float targetValue = (float)point / targetPoint;
        iTween.ValueTo(gameObject, iTween.Hash("from", currentValue, "to", point, "time", sliderSpeed, "onupdate", "OnPointChanged"));
        yield return null;
    }
}
