using System;
using UnityEngine;

using CirclesWar.Data;

namespace CirclesWar
{
    public class ConfigLoader
    {
        public GameConfig GetConfig()
		{
            var asset = Resources.Load<TextAsset>("GameConfig");

            var configMap = JsonUtility.FromJson<ConfigMap>(asset.text);

            if (configMap != null)
            {
                var gameConfig = configMap.GameConfig;
                Debug.Log("config load success");
                return gameConfig;
            }
            else
            {
                Debug.LogError("config load error");
            }
            return null;
		}

        [Serializable]
        class ConfigMap
        {
            public GameConfig GameConfig;
        }
	}
}
