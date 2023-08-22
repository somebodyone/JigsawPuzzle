using DTT.MiniGame.Jigsaw.UI;
using UnityEngine;

namespace DTT.MiniGame.Jigsaw.Demo
{
    /// <summary>
    /// Handles the audio of the puzzle board.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JigsawBoardAudioHandler : MonoBehaviour
    {
        /// <summary>
        /// Reference to the <see cref="JigsawBoardUI"/>.
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to the jigsaw board")]
        private JigsawBoardUI _board;

        /// <summary>
        /// Audio clip for when the user connects a puzzle piece to the board.
        /// </summary>
        [SerializeField]
        [Tooltip("Audio clip for when the user connects a puzzle piece to the board")]
        private AudioClip _connectClip;

        /// <summary>
        /// Reference to the audiosource component.
        /// </summary>
        private AudioSource _audioSource;

        /// <summary>
        /// Gets all necessary components.
        /// </summary>
        private void Awake() => _audioSource = GetComponent<AudioSource>();

        /// <summary>
        /// Subscribes to the necessary events.
        /// </summary>
        private void OnEnable() => _board.PieceDroppedOnBoard += ConnectClip;

        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable() => _board.PieceDroppedOnBoard -= ConnectClip;

        /// <summary>
        /// Plays the <see cref="_connectClip"/>.
        /// </summary>
        /// <param name="piece">Connected puzzle piece</param>
        private void ConnectClip(JigsawPuzzlePieceUI piece) => _audioSource.PlayOneShot(_connectClip);
    }
}
