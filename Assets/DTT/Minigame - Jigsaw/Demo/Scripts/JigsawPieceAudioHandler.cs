using DTT.MiniGame.Jigsaw.UI;
using UnityEngine;

namespace DTT.MiniGame.Jigsaw.Demo
{
    /// <summary>
    /// Handles the audio of the puzzle piece.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JigsawPieceAudioHandler : MonoBehaviour
    {
        /// <summary>
        /// Reference to the puzzle piece.
        /// </summary>
        [SerializeField]
        private JigsawPuzzlePieceUI _piece;

        /// <summary>
        /// Audio clip for when the user picks up the puzzle piece.
        /// </summary>
        [SerializeField]
        private AudioClip _pickUpClip;

        /// <summary>
        /// Audio clip for when the user drops the puzzle piece.
        /// </summary>
        [SerializeField]
        private AudioClip _dropClip;

        /// <summary>
        /// Reference to the audiosource component.
        /// </summary>
        private AudioSource _audioSource;

        /// <summary>
        /// Gets all necessary components.
        /// </summary>
        private void Awake() => _audioSource = GetComponent<AudioSource>();

        /// <summary>
        /// Subscribes to necessary events.
        /// </summary>
        private void OnEnable()
        {
            _piece.PickUp += PlayPickUp;
            _piece.Drop += PlayDrop;
        }

        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            _piece.PickUp -= PlayPickUp;
            _piece.Drop -= PlayDrop;
        }

        /// <summary>
        /// Plays the <see cref="_pickUpClip"/>.
        /// </summary>
        /// <param name="piece">Picked up piece</param>
        private void PlayPickUp(JigsawPuzzlePieceUI piece) => _audioSource.PlayOneShot(_pickUpClip);

        /// <summary>
        /// Plays the <see cref="_dropClip"/>.
        /// </summary>
        /// <param name="piece">Picked up piece</param>
        private void PlayDrop(JigsawPuzzlePieceUI piece) => _audioSource.PlayOneShot(_dropClip);
    }
}