using System.Threading.Tasks;
using Client.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Client.Managers
{
    public class GameManager : MonoBehaviour
    {
        
        void Start()
        {
            EventManager.Subscribe("", Zone);
        }

        private void Zone(BasePacket e)
        {
            
        }

        async Task LoadScene(string sceneName)
        {                        
            var scene = await AssetBundle.LoadFromFileAsync($"AssetBundles/StandaloneWindows/{sceneName}");
            BaseScenePaths = BaseSceneBundle.GetAllScenePaths().ToList();
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            SceneManager.LoadSceneAsync(BaseScenePaths[0]);
            async = Application.LoadLevelAsync(levelName);
            async.allowSceneActivation = false;
            yield return async;
            
            

        }
        
        void OnDestroy()
        {
            EventManager.Unsubscribe("", Zone);    
        }
    }
}