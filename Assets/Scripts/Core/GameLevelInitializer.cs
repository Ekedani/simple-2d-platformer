using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        
        private ExternalDevicesInputReader _externalDevicesInputReader;
        private PlayerBrain _playerBrain;
        private void Awake()
        {
            _externalDevicesInputReader = new ExternalDevicesInputReader();
            _playerBrain = new PlayerBrain
            (
                _playerEntity, new List<IEntityInputSource>
                {
                    _externalDevicesInputReader,
                }
            );
        }

        private void Update()
        {
            _externalDevicesInputReader.OnUpdate();
        }

        private void FixedUpdate()
        {
            _playerBrain.OnFixedUpdate();
        }
    }
}