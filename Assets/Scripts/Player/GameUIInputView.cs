using System;
using UnityEngine;

namespace Player
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        public float HorizontalDirection { get; }
        public bool Jump { get; }
        
        private void Awake()
        {
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {
            throw new NotImplementedException();
        }

        
        public void ResetOneTimeActions()
        {
            throw new NotImplementedException();
        }
    }
}