using UnityEngine;
using Zenject;

namespace Game
{
    /// <summary>
    /// Binds dependencies in the game state
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
