using UnityEngine;
using System.Collections;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class PlanetInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(settings.MeshFilter).WhenInjectedInto<NavigationMesh>();
            Container.Bind<MeshNormalizer>().AsSingle();
            Container.Bind<NavigationMesh>().AsSingle();
        }

        [System.Serializable]
        public class Settings
        {
            public MeshFilter MeshFilter;
        }
    }
}