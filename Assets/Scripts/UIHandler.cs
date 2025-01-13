using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _balanceTexts;
    [SerializeField] private StatUIText[] _statsUI;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _waveReward;
    [Space]
    [SerializeField] private Slider _waveProgress;
    [SerializeField] private Slider _baseHealth;
    [SerializeField] private Slider _goldenChestProgress;
    [SerializeField] private GameObject _claimChestButton;
    [Space]
    [SerializeField] private GameObject _waveWinScreen;
    [SerializeField] private GameObject _waveLoseScreen;
    [Space]
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private float _clickSoundVolume;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private float _winSoundVolume;

    private PurchaseHandler _purchaseHandler;
    private GlobalStats _globalStats;
    private Money _money;
    private WaveSpawner _waveSpawner;
    private ItemBoxPool _itemBoxPool;
    private Base _playerBase;
    private AudioPlayer _audioPlayer;

    private int _chestProgress = 0;

    [Inject]
    public void Construct(PurchaseHandler purchaseHandler, Money money, GlobalStats stats, WaveSpawner waveSpawner, ItemBoxPool pool, Base playerBase, AudioPlayer audioPlayer)
    {
        _purchaseHandler = purchaseHandler;
        _money = money;
        _globalStats = stats;
        _waveSpawner = waveSpawner;
        _itemBoxPool = pool;
        _playerBase = playerBase;
        _audioPlayer = audioPlayer;
    }

    private void Start()
    {
        _money.OnBalanceUpdate += UpdateBalanceUI;
        _purchaseHandler.OnStatChanged += UpdateStatsUI;
        _waveSpawner.OnWaveFinished += ShowFinishWaveView;
        _waveSpawner.OnWaveProgressUpdate += UpdateWaveProgress;
        _waveSpawner.OnWaveConfigured += ConfigureWave;
        _playerBase.OnBaseDestroyed += ShowWaveLoseScreen;
        _playerBase.OnHealthUpdate += UpdateBaseInfo;

        UpdateBalanceUI(_money.CurrentBalance);
        _purchaseHandler.InitUpgradeInfo();
    }

    private void UpdateBalanceUI(int balance)
    {
        foreach (var balanceText in _balanceTexts)
        {
            balanceText.text = $"{balance}";
        }
    }

    private void UpdateStatsUI(StatType type, float upgrade)
    {
        var statUI = _statsUI.FirstOrDefault(s => s.Type == type);

        if (statUI == null)
        {
            return;
        }

        var stat = _globalStats.GetStat(type);

        if (stat.CurrentValue == stat.MaxValue)
        {
            statUI.BuyButton.SetActive(false);
            statUI.TextField.text = "MAX";
            return;
        }

        statUI.TextField.text = $"{Mathf.RoundToInt(stat.CurrentValue * 100)}% + {Mathf.RoundToInt(upgrade * 100)}%";

        if (type == StatType.BaseHealth)
        {
            statUI.TextField.text = $"{stat.CurrentValue} + {upgrade}";
        }
    }

    private void ShowFinishWaveView(int wave)
    {
        _waveText.text = $"Wave {wave}";

        _waveWinScreen.gameObject.SetActive(true);
        _waveReward.text = $"+{_waveSpawner.CurrentReward}";
        _money.AddMoney(_waveSpawner.CurrentReward);
        _audioPlayer.PlaySound(_winSound, _winSoundVolume);
        _chestProgress++;
        if (_chestProgress == _goldenChestProgress.maxValue)
        {
            _claimChestButton.SetActive(true);
            _chestProgress = 0;
            _goldenChestProgress.gameObject.SetActive(false);
        }

        _goldenChestProgress.value = _chestProgress;
        CheckItemsUnlock(wave);
    }

    private void UpdateBaseInfo(float currentHealth, float maxHealth)
    {
        _baseHealth.value = currentHealth;
        _baseHealth.maxValue = maxHealth;
    }

    private void ConfigureWave(int maxEnemies)
    {
        _waveProgress.value = 0;
        _waveProgress.maxValue = maxEnemies;
    }

    private void ShowWaveLoseScreen()
    {
        _waveLoseScreen.gameObject.SetActive(true);
    }

    private void UpdateWaveProgress(int progress)
    {
        _waveProgress.value = progress;
    }

    private void CheckItemsUnlock(int wave)
    {
        foreach(var item in _statsUI) 
        {
            item.Cover.SetActive(wave < item.WaveToOpen);
        }
    }

    public void PlayClickSound()
    {
        _audioPlayer.PlaySound(_clickSound, _clickSoundVolume);
    }

    public void OnCreateBoxButtonClick() 
    {
        var isBoxBought = _purchaseHandler.TryBuyBox();

        if (isBoxBought) 
        {
            //Play Sound

            return;
        }

        //Play sound
    }

    public void OnClaimGoldenBoxButtonClick()
    {
        var isGoldenBoxSpawned = _itemBoxPool.TryCreateBox(true);

        if (isGoldenBoxSpawned)
        {
            _claimChestButton.gameObject.SetActive(false);
            _goldenChestProgress.gameObject.SetActive(true);
            //Play sound
            return;
        }

        //play sound
    }

    public void OnBuyChestUpgrade() 
    {
        BuyUpgrade(StatType.SilverBoxDropChance);
    }

    public void OnBuyBaseUpgrade()
    {
        var upgrade = BuyUpgrade(StatType.BaseHealth);

        if (upgrade) 
        {
            _playerBase.IncreaseMaxHealth();
        }
    }

    public void OnBuyProjectileUpgrade()
    {
        BuyUpgrade(StatType.ProjectileSpeed);
    }

    public void OnBuyDamageUpgrade()
    {
        BuyUpgrade(StatType.StructureDamage);
    }

    public void OnBuySpeedUpgrade()
    {
        BuyUpgrade(StatType.StructureDelay);
    }

    private bool BuyUpgrade(StatType type)
    {
        var isUpgradeBought = _purchaseHandler.TryBuyUpgrade(type);

        if (isUpgradeBought) 
        {
            //Play Sound

            return true;
        }

        //Play Sound
        return false;
    }

    public void OnWaveRestart()
    {
        _playerBase.Restart();
        _waveSpawner.RestartCurrentWave();
    }

    public void OnNextWaveStart()
    {
        _waveSpawner.SpawnWave();
    }

    private void OnDisable()
    {
        _money.OnBalanceUpdate -= UpdateBalanceUI;
        _purchaseHandler.OnStatChanged -= UpdateStatsUI;
        _waveSpawner.OnWaveFinished -= ShowFinishWaveView;
        _waveSpawner.OnWaveProgressUpdate -= UpdateWaveProgress;
        _waveSpawner.OnWaveConfigured -= ConfigureWave;
        _playerBase.OnBaseDestroyed -= ShowWaveLoseScreen;
        _playerBase.OnHealthUpdate -= UpdateBaseInfo;
    }

    [Serializable]
    private class StatUIText
    {
        [field: SerializeField] public StatType Type { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextField { get; private set; }
        [field: SerializeField] public GameObject BuyButton { get; private set; } 
        [field: SerializeField] public GameObject Cover { get; private set; } 
        [field: SerializeField] public int WaveToOpen { get; private set; }
    }
}
