using DTT.MinigameBase.LevelSelect;
using UnityEngine;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// Class that implements a version of the Minigame Base level select.
    /// </summary>
    public class JigsawLevelSelectHandler : LevelSelectHandler<JigsawConfig, JigsawResult, JigsawManager>
    {
        /// <summary>
        /// A reference to the configurations that will be used to set up levels.
        /// </summary>
        [SerializeField]
        private JigsawConfig[] _configs;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="result"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        protected override float CalculateScore(JigsawResult result) => result.score;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="levelNumber"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        protected override JigsawConfig GetConfig(int levelNumber) => _configs[levelNumber % _configs.Length];
    }
}