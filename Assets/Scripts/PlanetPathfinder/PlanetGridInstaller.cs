using UnityEngine;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class PlanetGridInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings = null;

        public override void InstallBindings()
        {
            //Container.BindInstance(settings.NavigationMesh).WhenInjectedInto<NavigationMesh>();
            //Container.Bind<MeshNormalizer>().AsSingle();
            Container.BindInstance(settings.PlanetRadius);
            Container.Bind<PlanetGrid>().AsSingle();
        }

        [System.Serializable]
        public class Settings
        {
            public NavigationMesh NavigationMesh;
            public float PlanetRadius;
        }
    }
}