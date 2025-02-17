using Zenject;

public class MouseLockState : IMouseEventState
{
    private SignalBus signalBus;
    private ShapeController shapeController;
   
    public MouseLockState(SignalBus signalBus, ShapeController shapeController)
    {
        this.signalBus = signalBus;
        this.shapeController = shapeController;
        signalBus.Subscribe<ShapesCreatedSignal>(() => shapeController.StateMachine.ChangeState(shapeController.ReadySelectionState));
        signalBus.Subscribe<LevelLoseSignal>(() => shapeController.StateMachine.ChangeState(shapeController.MouseLockState));
    }

    public void OnDrag()
    {
    }

    public void OnPointerDown()
    {
    }

    public void OnPointerUp()
    {
    }
}
