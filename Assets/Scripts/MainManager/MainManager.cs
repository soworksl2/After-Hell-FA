using UnityEngine;

namespace AfterHellFA.Extra.MainManager
{
    public class MainManager : MonoBehaviour
    {
        private static bool _mainManagerAlreadyExists = false;

        private Manager[] _childsManagers;

        private void Start()
        {
            if (!_mainManagerAlreadyExists)
            {
                Object.DontDestroyOnLoad(this.gameObject);
                _mainManagerAlreadyExists = true;
            }
            else
            {
                Object.Destroy(this.gameObject);
            }
        }
    }

}
