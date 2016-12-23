using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SpaceCentipedeFromHell
{
    public class Formatter
    {
        public static BinaryFormatter CreateFormatter()
        {
            var surrogateSelector = new SurrogateSelector();

            surrogateSelector.AddSurrogate(typeof(Vector3),
                            new StreamingContext(StreamingContextStates.All),
                            new Vector3SerializationSurrogate());

            return new BinaryFormatter()
            {
                SurrogateSelector = surrogateSelector
            };
        }
    }
}