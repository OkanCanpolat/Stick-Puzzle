using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeCreatorConfig", menuName = "Configurations / Shape Creator")]
public class ShapeCreatorConfig : ScriptableObject
{
    public List<ShapeController> Shapes;
    public List<ShapeController> HighProbabilityShapes;
    public List<ShapeController> LowProbabilityShapes;
    public float ShapeSlideSpeed = 0.5f;
    public Vector3 InstantiateOffset = new Vector3(4, 0, 0);

}
