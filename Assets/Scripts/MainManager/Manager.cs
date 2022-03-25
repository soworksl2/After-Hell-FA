using System.Collections;
using UnityEngine;

namespace AfterHellFA.Extra.MainManager
{
    public abstract class Manager : ScriptableObject
    {
        [SerializeField]
        private bool _hasUpdate;

        public bool HasUpdate => this._hasUpdate;

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }
    }
}