using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private MergeGrid _mergeGrid;
    [SerializeField] private PlacementGrid _placementGrid;
    [SerializeField] private CellItemsPool _cellItemsPool;
    [SerializeField] private ItemBoxPool _itemBoxPool;
    [SerializeField] private Base _playerBase;
    [SerializeField] private Money _money;
    [SerializeField] private ProjectilePool _projectilePool;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputReader>().FromInstance(_inputReader).AsSingle();
        Container.BindInterfacesAndSelfTo<MergeGrid>().FromInstance(_mergeGrid).AsSingle();
        Container.BindInterfacesAndSelfTo<PlacementGrid>().FromInstance(_placementGrid).AsSingle();
        Container.BindInterfacesAndSelfTo<CellItemsPool>().FromInstance(_cellItemsPool).AsSingle();
        Container.BindInterfacesAndSelfTo<ItemBoxPool>().FromInstance(_itemBoxPool).AsSingle();
        Container.BindInterfacesAndSelfTo<Money>().FromInstance(_money).AsSingle();
        Container.BindInterfacesAndSelfTo<Base>().FromInstance(_playerBase).AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectilePool>().FromInstance(_projectilePool).AsSingle();
    }
}
