using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DTT.MiniGame.Jigsaw.UI
{
    /// <summary>
    /// Handles the displaying the time on a text object.
    /// </summary>
    public class TimerUI : MonoBehaviour
    {
        /// <summary>
        /// The text object to display the time onto.
        /// </summary>
        [SerializeField]
        [Tooltip("The text object to display the time onto")]
        private Text _text;
        
        /// <summary>
        /// The object to retrieve the time to display to from.
        /// </summary>
        [SerializeField]
        [Tooltip("The object to retrieve the time to display to from")]
        private Object _elapsed;

        /// <summary>
        /// The object to retrieve the time to display to from.
        /// </summary>
        private IElapsed elapsed => (IElapsed)_elapsed;

        /// <summary>
        /// Sets the time to the text object.
        /// </summary>
        private void Update() => _text.text = FormatTime(elapsed.TimeElapsed);

        /// <summary>
        /// Formats the time to what is readable.
        /// </summary>
        /// <param name="time">The time in seconds to format.</param>
        /// <returns>A formatted time string. In the form of; '00:00'.</returns>
        private string FormatTime(float time)
        {
            float displaySeconds = elapsed.TimeElapsed % 60;
            float displayMinutes = Mathf.Clamp(Mathf.Floor(elapsed.TimeElapsed / 60), 0, 59);
            return $"{displayMinutes:00}:{displaySeconds:00}";
        }
    }
}