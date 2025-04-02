using SomewhatDependencyInjector;
using UnityEngine;

namespace SomewhatManager
{
    public class Bar : ManagerBase
    {
        [Inject] public Foo Foo { get; set; }
        [Inject] private Baz Baz { get; set; }
        
        public override void Init()
        {
            base.Init();
            
            Debug.Log($"Is public property Foo null ? {Foo == null}");
            Debug.Log($"Is private property Baz null ? {Baz == null}");
        }
    }
}
