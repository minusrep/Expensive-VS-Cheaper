using UnityEngine;

namespace DoubleB.Runtime.Runtime.Audio
{
    public class AudioView : MonoBehaviour
    {
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
    }
}