using UnityEngine;

namespace SupersonicWisdomSDK
{
    public class SwPlayerPrefsStore : ISwKeyValueStore
    {
        #region --- Public Methods ---

        public void DeleteAll()
        {
            SwInfra.Logger.Log("SwPlayerPrefsStore | DeleteAll");
            PlayerPrefs.DeleteAll();
        }

        public ISwKeyValueStore DeleteKey(string key)
        {
            if (key.SwIsNullOrEmpty()) return this;
            
            SwInfra.Logger.Log($"SwPlayerPrefsStore | DeleteKey | {key}");
            PlayerPrefs.DeleteKey(key);
            
            return this;
        }

        public bool GetBoolean(string key, bool defaultValue = false)
        {
            if (key.SwIsNullOrEmpty()) return defaultValue;
            
            var value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
            SwInfra.Logger.Log($"SwPlayerPrefsStore | GetBoolean | {key} | {value}");

            return value;
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            if (key.SwIsNullOrEmpty()) return defaultValue;
            
            var value = PlayerPrefs.GetFloat(key, defaultValue);
            SwInfra.Logger.Log($"SwPlayerPrefsStore | GetFloat | {key} | {value}");
            
            return value;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (key.SwIsNullOrEmpty()) return defaultValue;
            
            var value = PlayerPrefs.GetInt(key, defaultValue);
            SwInfra.Logger.Log($"SwPlayerPrefsStore | GetInt | {key} | {value}");
            
            return value;
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (key.SwIsNullOrEmpty()) return defaultValue;
            
            var value = PlayerPrefs.GetString(key, defaultValue);
            SwInfra.Logger.Log($"SwPlayerPrefsStore | GetString | {key} | {value ?? "Null"}");
            
            return value;
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Save ()
        {
            PlayerPrefs.Save();
        }

        public ISwKeyValueStore SetBoolean(string key, bool value)
        {
            if (key.SwIsNullOrEmpty()) return this;
            
            SwInfra.Logger.Log($"SwPlayerPrefsStore | SetBoolean | {key} | {value}");
            PlayerPrefs.SetInt(key, value ? 1 : 0);

            return this;
        }

        public ISwKeyValueStore SetFloat(string key, float value)
        {
            if (key.SwIsNullOrEmpty()) return this;
            
            SwInfra.Logger.Log($"SwPlayerPrefsStore | SetFloat | {key} | {value}");
            PlayerPrefs.SetFloat(key, value);

            return this;
        }

        public ISwKeyValueStore SetInt(string key, int value)
        {
            if (key.SwIsNullOrEmpty()) return this;
            
            SwInfra.Logger.Log($"SwPlayerPrefsStore | SetInt | {key} | {value}");
            PlayerPrefs.SetInt(key, value);

            return this;
        }

        public ISwKeyValueStore SetString(string key, string value)
        {
            if (key.SwIsNullOrEmpty()) return this;
            
            SwInfra.Logger.Log($"SwPlayerPrefsStore | SetString | {key} | {value ?? "Null"}");
            PlayerPrefs.SetString(key, value);

            return this;
        }

        #endregion
    }
}