using DCFApixels.DragonECS;

namespace VS.Runtime.Core.Components
{
    public struct Score : IEcsWorldComponent<Score>
    {
        public int Total;
        public void Init(ref Score component, EcsWorld world) { component.Total = 0; }
        public void OnDestroy(ref Score component, EcsWorld world) { }
    }
}
