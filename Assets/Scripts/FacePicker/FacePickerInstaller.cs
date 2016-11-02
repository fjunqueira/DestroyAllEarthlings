using UnityEngine;
using System.Collections;
using Zenject;

namespace SpaceCentipedeFromHell
{
    public class FacePickerInstaller : MonoInstaller
    {
        [SerializeField]
        Settings settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(settings.MeshCollider).WhenInjectedInto<FacePicker>();
            Container.BindInstance(settings.Camera).WhenInjectedInto<FacePicker>();
            Container.Bind<FacePicker>().AsSingle();
        }
    }

    [System.Serializable]
    public class Settings
    {
        public MeshCollider MeshCollider;
        public Camera Camera;
    }
}