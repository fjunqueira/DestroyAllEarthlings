using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
            using (var stream = new FileStream(settings.GridPath, FileMode.Open))
            {
                var surrogateSelector = new SurrogateSelector();

                surrogateSelector.AddSurrogate(typeof(Vector3),
                                new StreamingContext(StreamingContextStates.All),
                                new Vector3SerializationSurrogate());

                var formatter = new BinaryFormatter()
                {
                    SurrogateSelector = surrogateSelector
                };

                Container.Bind<PlanetGrid>().FromInstance(formatter.Deserialize(stream) as PlanetGrid);
            }
        }

        [System.Serializable]
        public class Settings
        {
            public string GridPath;
        }
    }
}