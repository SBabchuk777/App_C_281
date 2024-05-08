using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Saves
{
    public class GameSaves
    {
        private const string LevelKey = "_Level_Key";
        private const string StartCoinKey = "_Star_Coin_Key";

        private int LevelIndex;
        private int StarCoin;

        private static GameSaves _instance;

        public static GameSaves Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameSaves();
                }
                return _instance;
            }
        }

        public void SaveLevelIndex()
        {
            LevelIndex = ReadData<int>(LevelKey);

            LevelIndex += 1;

            WriteData<int>(LevelKey,LevelIndex);

            SetLevel();
        }

        public void SetLevel()
        {
            LevelIndex = ReadData<int>(LevelKey);

            if (LevelIndex >= 10)
            {
                LevelIndex = 0;
                WriteData<int>(LevelKey, LevelIndex);
            }
        }

        public void AddStarCoin(int coin)
        {
            StarCoin = ReadData<int>(StartCoinKey);
            StarCoin += coin;
            WriteData<int>(StartCoinKey, StarCoin);
        }

        public int GetCoin() => ReadData<int>(StartCoinKey);
        public int GetLevel() => ReadData<int>(LevelKey);

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public T ReadJson<T>(string key) where T : new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string jsonString = PlayerPrefs.GetString(key);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return new T();
            }
        }

        public void WriteJson<T>(string key, T data)
        {
            PlayerPrefs.SetString(key, JsonConvert.SerializeObject(data));
        }

        public void WriteData<T>(string key, T data)
        {
            if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, (int)(object)data);
            }
            else if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, (float)(object)data);
            }
            else if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(key, (string)(object)data);
            }
            else if (typeof(T) == typeof(bool))
            {
                PlayerPrefs.SetInt(key, (bool)(object)data ? 1 : 0);
            }
            else
            {
                Debug.LogError("Wrong data type");
            }

            PlayerPrefs.Save();
        }

        public T ReadData<T>(string key)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(key);
            }

            if (typeof(T) == typeof(float))
            {
                return (T)(object)PlayerPrefs.GetFloat(key);
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(key);
            }

            if (typeof(T) == typeof(bool))
            {
                int intValue = PlayerPrefs.GetInt(key);
                return (T)(object)(intValue != 0);
            }

            Debug.LogError("There are no Saves");
            return default(T);
        }
    }
}
