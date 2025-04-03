using System.Collections.Generic;
using SomewhatDependencyInjector;
using SomewhatManager;
using UnityEngine;

namespace SomewhatLauncher
{
    public class ApplicationLauncher : MonoBehaviour
    {
        private DependencyInjector _dependencyInjector;
        private List<ManagerBase> _managers;
    
        private Foo _foo;
        private Bar _bar;
        private Baz _baz;
    
        private void Awake()
        {
            _managers = new List<ManagerBase>();
            _dependencyInjector = new DependencyInjector();

            CreateManagers();
            InjectManagers();
            InitializeManagers();
        }

        private void CreateManagers()
        {
            void CreateManager<T>(out T manager, Transform parent) where T : ManagerBase
            {
                var managerType = typeof(T);
        
                var managerGameObject = new GameObject(managerType.Name);
                managerGameObject.transform.SetParent(parent);

                manager = managerGameObject.AddComponent<T>();
                _dependencyInjector.Register(manager);
                _managers.Add(manager);
            }
            
            var managersContainer = new GameObject("Managers").transform;
            
            CreateManager<Foo>(out _foo, managersContainer);
            CreateManager<Bar>(out _bar, managersContainer);
            CreateManager<Baz>(out _baz, managersContainer);
        }

        private void InjectManagers()
        {
            foreach (var manager in _managers)
            {
                _dependencyInjector.InjectDependencies(manager);
            }
        }

        private void InitializeManagers()
        {
            foreach (var manager in _managers)
            {
                manager.Init();
            }
        }
    }
}
