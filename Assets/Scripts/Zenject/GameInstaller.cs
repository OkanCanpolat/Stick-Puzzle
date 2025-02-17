using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public LevelData LevelData;
    public BlastConfig BlastConfig;
    public WinScreenConfig WinScreenConfig;
    public ShapeCreatorConfig ShapeCreatorConfig;
    public SoundConfig SoundConfig;
    public LoseScreenConfig LoseScreenConfig;
    public GridManagerConfig GridManagerConfig;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        GoalInstaller.Install(Container, LevelData);
        ShapePlacementInstaller.Install(Container);

        Container.DeclareSignal<ShapePlacedSignal>().OptionalSubscriber();
        Container.DeclareSignal<LevelLoseSignal>().OptionalSubscriber();
        Container.DeclareSignal<BlastSignal>().OptionalSubscriber();
        Container.DeclareSignal<LevelWinSignal>().OptionalSubscriber();
        Container.DeclareSignal<GridCreatedSignal>().OptionalSubscriber();
        Container.DeclareSignal<ShapesCreatedSignal>().OptionalSubscriber();

        Container.Bind<IShapeCreator>().To<RandomShapeCreator>().AsSingle();
        Container.Bind<ShapeObjectPool>().AsSingle();
        Container.Bind<IAudioService>().To<AudioManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ShapeCreator>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GridManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PointGoalUIController>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<LevelData>().FromInstance(LevelData).AsSingle();
        Container.Bind<BlastConfig>().FromInstance(BlastConfig).AsSingle();
        Container.Bind<WinScreenConfig>().FromInstance(WinScreenConfig).AsSingle();
        Container.Bind<ShapeCreatorConfig>().FromInstance(ShapeCreatorConfig).AsSingle();
        Container.Bind<SoundConfig>().FromInstance(SoundConfig).AsSingle();
        Container.Bind<LoseScreenConfig>().FromInstance(LoseScreenConfig).AsSingle();
        Container.Bind<GridManagerConfig>().FromInstance(GridManagerConfig).AsSingle();

        Container.BindFactory<ShapeController, Vector3, Quaternion, Transform, ShapeController, Factory>().FromFactory<CustomFactory>();
    }
}

public class GoalInstaller : Installer<LevelData, GoalInstaller>
{
    private LevelData levelData;

    public GoalInstaller(LevelData levelData)
    {
        this.levelData = levelData;
    }
    public override void InstallBindings()
    {
        switch (levelData.GoalType)
        {
            case GoalType.Point:
                Container.Bind<ILevelGoal>().To<PointGoal>().AsSingle();
                break;

            default:
                Container.Bind<ILevelGoal>().To<PointGoal>().AsSingle();
                break;
        }
    }
}
public class ShapePlacementInstaller : Installer<ShapePlacementInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IShapePlacementController>().To<Hor_I_1X>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.Hor_I_1X);
        Container.Bind<IShapePlacementController>().To<Ver_I_1X>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.Ver_I_1X);
        Container.Bind<IShapePlacementController>().To<L_DR>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.L_Down_Right);
        Container.Bind<IShapePlacementController>().To<U_Down>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.U_Down);
        Container.Bind<IShapePlacementController>().To<L_2x1_DR>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.L_2x1_DR);
        Container.Bind<IShapePlacementController>().To<U_Up>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.U_Up);
        Container.Bind<IShapePlacementController>().To<I_Hor_2X>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.Hor_I_2X);
        Container.Bind<IShapePlacementController>().To<I_Ver_2X>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.Ver_I_2X);
        Container.Bind<IShapePlacementController>().To<U_Right>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.U_Right);
        Container.Bind<IShapePlacementController>().To<U_Left>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.U_Left);
        Container.Bind<IShapePlacementController>().To<L_DL>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.L_DL);
        Container.Bind<IShapePlacementController>().To<L_UL>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.L_UL);
        Container.Bind<IShapePlacementController>().To<L_UR>().AsSingle().When(context => context.ObjectType == typeof(ShapeController) && (context.ObjectInstance as ShapeController).Type == ShapeType.L_UR);
    }
}


public class ShapePlacedSignal
{
    public ShapeController Shape;
    public ShapePlacedSignal(ShapeController shape)
    {
        Shape = shape;
    }
}
public class LevelLoseSignal { }
public class LevelWinSignal { }
public class BlastSignal { }
public class GridCreatedSignal { }
public class ShapesCreatedSignal { }