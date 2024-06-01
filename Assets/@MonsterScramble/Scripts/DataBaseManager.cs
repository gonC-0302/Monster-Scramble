using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;
    public MonsterDataSO dataSO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}