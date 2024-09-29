using UnityEngine;

public class ApplicationStarter : MonoBehaviour
{
    private Test_GameManager _gameManager;
    private Test_InputManager _inputManager;
    private DependencyInjector _dependencyInjector;
    private void Awake()
    {
        _dependencyInjector = new DependencyInjector();

        var managersContainer = new GameObject("Managers").transform;
        CreateManager<Test_GameManager>(ref _gameManager, managersContainer);
        CreateManager<Test_InputManager>(ref _inputManager, managersContainer);

        _dependencyInjector.InjectDependencies(this);
    }

    private void CreateManager<T>(ref T manager, Transform parent) where T : ManagerBase
    {
        var managerType = typeof(T);
        
        var managerGameObject = new GameObject(managerType.Name);
        managerGameObject.transform.SetParent(parent);

        manager = managerGameObject.AddComponent<T>();
        _dependencyInjector.Register(manager);
    }
}
