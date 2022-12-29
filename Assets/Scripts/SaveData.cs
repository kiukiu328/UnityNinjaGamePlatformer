[System.Serializable]
public class SaveData
{
    public bool[] LevelUnlocked = new bool[3];

    public SaveData()
    {
        LevelUnlocked[0] = true;
    }

    public void SetLevelActive(int level)
    {
        LevelUnlocked[level - 1] = true;
    }

    public bool GetLevelActive(int level)
    {
        return LevelUnlocked[level - 1];
    }
}
