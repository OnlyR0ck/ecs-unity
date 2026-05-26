using System;
using System.Collections.Generic;
using UnityEngine;
using VS.Runtime.Shared.UI;

namespace VS.Core.Configs.Features
{
    [CreateAssetMenu(menuName = "VS/ViewSourceConfig", fileName = "ViewSourceConfig")]
    public class ViewSourceConfig : ScriptableObject, IViewSourceProvider
    {
        [SerializeField] private List<BaseView> _prefabs;

        public BaseView GetPrefab<TView>() where TView : BaseView
        {
            foreach (var prefab in _prefabs)
                if (prefab is TView)
                    return prefab;
            throw new Exception($"{typeof(TView).Name} not registered in ViewSourceConfig");
        }
    }
}
