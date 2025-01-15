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
    [SerializeField] private Button _claimChestButton;
    [SerializeField] private Image _muteImage;
    [Space]
    [SerializeField] private Sprite _mutedSprite;
    [SerializeField] private Sprite _unmutedSprite;
    [Space]
    [SerializeField] private GameObject _waveWinScreen;
    [SerializeField] private GameObject _waveLoseScreen;
    [SerializeField] private GameObject _powerUpsView;
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
    private InputReader _inputReader;

    private int _chestProgress = 0;

    [Inject]
    public void Construct(PurchaseHandler purchaseHandler, Money money, GlobalStats stats, WaveSpawner waveSpawner, ItemBoxPool pool, Base playerBase, AudioPlayer audioPlayer, InputReader reader)
    {
        _purchaseHandler = purchaseHandler;
        _money = money;
        _globalStats = stats;
        _waveSpawner = waveSpawner;
        _itemBoxPool = pool;
        _playerBase = playerBase;
        _audioPlayer = audioPlayer;
        _inputReader = reader;
    }

    private void Start()
    {
        _money.OnBalanceUpdate += UpdateBalanceUI;
        _purchaseHandler.OnStatChanged += UpdateStatsUI;
        _waveSpawner.OnWaveCompleted += ShowFinishWaveView;
        _waveSpawner.OnWaveProgressUpdate += UpdateWaveProgress;
        _waveSpawner.OnWaveConfigured += ConfigureWave;
        _playerBase.OnBaseDestroyed += ShowWaveLoseScreen;
        _playerBase.OnHealthUpdate += UpdateBaseInfo;

        UpdateBalanceUI(_money.CurrentBalance);
        _chestProgress = SaveAndLoad.LoadChestValue();
        UpdateBaseInfo(_playerBase.CurrentHealth, _playerBase.MaxHealth);
        _purchaseHandler.InitUpgradeInfo();
        _waveText.text = $"Wave {_waveSpawner.CurrentWaveNumber}";
        CheckItemsUnlock(_waveSpawner.CurrentWaveNumber);
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
            statUI.LevelField.text = "lvl MAX";
            return;
        }

        statUI.TextField.text = $"{Mathf.RoundToInt(stat.CurrentValue * 100)}% + {Mathf.RoundToInt(upgrade * 100)}%";
        statUI.LevelField.text = $"lvl {stat.Level}";

        if (type == StatType.BaseHealth)
        {
            statUI.TextField.text = $"{stat.CurrentValue} + {upgrade}";
        }
    }

    private void ShowFinishWaveView(int wave)
    {
        _waveText.text = $"Wave {wave}";
        _powerUpsView.gameObject.SetActive(false);
        SetUICover(true);
        _waveWinScreen.gameObject.SetActive(true);
        _waveReward.text = $"+{_waveSpawner.CurrentReward}";
        _money.AddMoney(_waveSpawner.CurrentReward);
        _audioPlayer.PlaySound(_winSound, _winSoundVolume);
        _chestProgress++;

        if (_chestProgress == _goldenChestProgress.maxValue)
        {
            _claimChestButton.image.color = Color.white;
            _claimChestButton.interactable = true;
            _chestProgress = 0;
        }

        SaveAndLoad.SaveChestInfo(_chestProgress);

        _goldenChestProgress.value = _chestProgress;
        CheckItemsUnlock(wave);

        SaveAndLoad.Save();
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
        _powerUpsView.gameObject.SetActive(false);
        SetUICover(true);
        _waveLoseScreen.gameObject.SetActive(true);
        SaveAndLoad.Save();
    }

    public void SetUICover(bool value)
    {
        _inputReader.IsUICover = value;
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
        _purchaseHandler.TryBuyBox();
    }

    public void OnClickMuteButton()
    {
        _audioPlayer.Mute();
        if (_audioPlayer.IsMuted)
        {
            _muteImage.sprite = _mutedSprite;
            _muteImage.color = Color.red;
            return;
        }

        _muteImage.sprite = _unmutedSprite;
        _muteImage.color = Color.white;
    }

    public void OnClaimGoldenBoxButtonClick()
    {
        var isGoldenBoxSpawned = _itemBoxPool.TryCreateBox(true);

        if (isGoldenBoxSpawned)
        {
            _claimChestButton.image.color = Color.black;
            _claimChestButton.interactable = false;
            return;
        }
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
        return _purchaseHandler.TryBuyUpgrade(type);
    }

    public void OnWaveRestart()
    {
        SetUICover(false);
        _playerBase.Restart();
        _waveSpawner.RestartCurrentWave();
    }

    public void OnNextWaveStart()
    {
        SetUICover(false);
        _waveSpawner.SpawnWave();
    }

    private void OnDisable()
    {
        _money.OnBalanceUpdate -= UpdateBalanceUI;
        _purchaseHandler.OnStatChanged -= UpdateStatsUI;
        _waveSpawner.OnWaveCompleted -= ShowFinishWaveView;
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
        [field: SerializeField] public TextMeshProUGUI LevelField { get; private set; }
        [field: SerializeField] public GameObject BuyButton { get; private set; } 
        [field: SerializeField] public GameObject Cover { get; private set; } 
        [field: SerializeField] public int WaveToOpen { get; private set; }
    }
}
