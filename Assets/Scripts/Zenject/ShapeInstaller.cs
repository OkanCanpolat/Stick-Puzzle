using UnityEngine;
using Zenject;

public class ShapeInstaller : MonoInstaller
{
    public ShapeController shapeController;
    public override void InstallBindings()
    {
        Container.Bind<MouseEventStateMachine>().AsSingle();
        Container.Bind<ShapeController>().FromInstance(shapeController).AsSingle();
        Container.Bind(typeof(IMouseEventState), typeof(MouseLockState)).WithId("MouseLockState").To<MouseLockState>().AsSingle();
        Container.Bind(typeof(IMouseEventState), typeof(MouseReadySelectionState)).WithId("ReadySelectionState").To<MouseReadySelectionState>().AsSingle();
    }
}