﻿using Arch.Core;
using Arch.Core.Utils;
using VContainer.Unity;
using VS.Runtime.Utilities;

namespace VS.Runtime.Core
{
    public class CoreFlow : IStartable
    {
        private readonly LoadingService _loadingService;
        private readonly SceneService _sceneService;

        public CoreFlow(LoadingService loadingService, SceneService sceneService)
        {
            _loadingService = loadingService;
            _sceneService = sceneService;
        }

        public async void Start()
        {
            World world = World.Create();
            world.Create();
        }
    }
}