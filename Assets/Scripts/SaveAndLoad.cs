using System;
using TigerForge;

public static class SaveAndLoad
{
    private const string _cellKey = "Cell_";
    private const string _pCellKey = "PCell_";
    private const string _waveData = "WaveData";
    private const string _money = "Money";
    private const string _stat = "Stat_";
    private const string _goldenChest = "GoldenChest";

    private static EasyFileSave _file;

    public static bool Load()
    {
        _file = new EasyFileSave();
        return _file.Load();
    }

    public static void SaveCell(System.Type type, CellInfo info)
    {
        string key = $"{_cellKey}{info.CellIndex}";

        if (type == typeof(PlacementCell))
        {
            key = $"{_pCellKey}{info.CellIndex}";
        }

        _file.AddBinary(key, info);
    }

    public static void SaveWaveInfo(WaveInformation info)
    {
        _file.AddBinary(_waveData, info);
    }

    public static void SaveMoney(int amount)
    {
        _file.Add(_money, amount);
    }

    public static void SaveStat(Stat stat)
    {
        _file.AddBinary($"{_stat}{stat.Type}", stat);
    }

    public static void SaveChestInfo(int value)
    {
        _file.Add(_goldenChest, value);
    }

    public static CellInfo LoadCell(int index, System.Type type) 
    {
        string key = $"{_cellKey}{index}";

        if (type == typeof(PlacementCell))
        {
            key = $"{_pCellKey}{index}";
        }

        return (CellInfo)_file.GetBinary(key);
    }

    public static WaveInformation LoadWaveInfo()
    {
        return (WaveInformation)_file.GetBinary(_waveData);
    }

    public static int LoadMoney()
    {
        return _file.GetInt(_money);
    }

    public static Stat LoadStat(StatType type)
    {
        return (Stat)_file.GetBinary($"{_stat}{type}");
    }

    public static int LoadChestValue() 
    {
        return _file.GetInt(_goldenChest);
    }

    public static bool Save()
    {
        return _file.Save();
    }

    public static void Dispose()
    {
        _file.Dispose();
    }
}

[Serializable]
public class WaveInformation
{
    public int WaveNumber { get; private set; }
    public int WaveIndex { get; private set; }

    public WaveInformation(int number, int index) 
    {
        WaveNumber = number;
        WaveIndex = index;
    }
}

[Serializable]
public class CellInfo 
{
    public bool HasItem { get; private set; }
    public int CellIndex { get; private set; }
    public int TurretIndex { get; private set; }
    public int BoxIndex { get; private set; }

    public CellInfo(bool hasItem, int cellIndex, int turretIndex = -1, int boxIndex = -1)
    {
        HasItem = hasItem;
        CellIndex = cellIndex;
        TurretIndex = turretIndex;
        BoxIndex = boxIndex;
    }
}