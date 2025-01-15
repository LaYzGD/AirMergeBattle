using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GlobalStats _stats;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private MergeGrid _mergeGrid;
    [SerializeField] private PlacementGrid _placementGrid;
    [SerializeField] private CellItemsPool _cellItemsPool;
    [SerializeField] private ItemBoxPool _itemBoxPool;
    [SerializeField] private Base _playerBase;
    [SerializeField] private Money _money;
    [SerializeField] private ProjectilePool _projectilePool;
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private PurchaseHandler _purchaseHandler;
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private VFXPool _vFXPool;
    [SerializeField] private Animations _animations;
    [SerializeField] private AllTurretUpgrades _allTurretUpgrades;

    public override void InstallBindings()
    {
        _stats.SetUp();
        Container.BindInterfacesAndSelfTo<GlobalStats>().FromInstance(_stats).AsSingle();
        Container.BindInterfacesAndSelfTo<InputReader>().FromInstance(_inputReader).AsSingle();
        Container.BindInterfacesAndSelfTo<AllTurretUpgrades>().FromInstance(_allTurretUpgrades).AsSingle();
        Container.BindInterfacesAndSelfTo<MergeGrid>().FromInstance(_mergeGrid).AsSingle();
        Container.BindInterfacesAndSelfTo<PlacementGrid>().FromInstance(_placementGrid).AsSingle();
        Container.BindInterfacesAndSelfTo<CellItemsPool>().FromInstance(_cellItemsPool).AsSingle();
        Container.BindInterfacesAndSelfTo<ItemBoxPool>().FromInstance(_itemBoxPool).AsSingle();
        Container.BindInterfacesAndSelfTo<Money>().FromInstance(_money).AsSingle();
        Container.BindInterfacesAndSelfTo<Base>().FromInstance(_playerBase).AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectilePool>().FromInstance(_projectilePool).AsSingle();
        Container.BindInterfacesAndSelfTo<WaveSpawner>().FromInstance(_waveSpawner).AsSingle();
        Container.BindInterfacesAndSelfTo<PurchaseHandler>().FromInstance(_purchaseHandler).AsSingle();
        Container.BindInterfacesAndSelfTo<AudioPlayer>().FromInstance(_audioPlayer).AsSingle();
        Container.BindInterfacesAndSelfTo<VFXPool>().FromInstance(_vFXPool).AsSingle();
        Container.BindInterfacesAndSelfTo<Animations>().FromInstance(_animations).AsSingle();
    }
}
