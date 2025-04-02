using SomewhatDependencyInjector;
using UnityEngine;

namespace SomewhatManager
{
    public class Baz : ManagerBase
    {
        [Inject] private Foo _foo;
        [Inject] private Bar Bar { get; set; }
        
        public override void Init()
        {
            base.Init();
            
            Debug.Log($"Is private field Foo null ? {_foo == null}");
            Debug.Log($"Is private property Bar null ? {Bar == null}");
        }
    }
}
