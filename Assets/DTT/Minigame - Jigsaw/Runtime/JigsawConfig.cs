using UnityEngine;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// Contains the configuration for the jigsaw game.
    /// </summary>
    [CreateAssetMenu(fileName = "New jigsaw config", menuName = "DTT/Mini Game/Jigsaw/Config")]
    public class JigsawConfig : ScriptableObject
    {
        /// <summary>
        /// The image that will be on the jigsaw pieces.
        /// </summary>
        public Sprite Image
        {
            get
            { return _image;
            }
            set
            {
                _image = value;
            }
        }

        /// <summary>
        /// The size of the puzzle.
        /// </summary>
        public Vector2Int Size
        {
            get { return _size; }
            set
            {
                _size = value;
            }
        }

        [SerializeField]
        [Tooltip("The image that will be on the jigsaw pieces.")]
        private Sprite _image;
        
        /// <summary>
        /// The size of the puzzle.
        /// </summary>
        [SerializeField]
        [Tooltip("The size of the jigsaw grid.")]
        private Vector2Int _size;
    }
}