using SomewhatDependencyInjector;
using UnityEngine;

namespace SomewhatManager
{
    public class Foo : ManagerBase
    {
        [Inject] public Baz baz;

        [Inject] private Bar _bar;

        public override void Init()
        {
            base.Init();
            
            Debug.Log($"Is private field _bar null ? {_bar == null}");
            Debug.Log($"Is public field baz null ? {baz == null}");
        }
    }
}
