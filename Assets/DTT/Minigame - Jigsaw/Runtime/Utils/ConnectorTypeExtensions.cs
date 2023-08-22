namespace DTT.MiniGame.Jigsaw.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="ConnectorType"/> class.
    /// </summary>
    public static class ConnectorTypeExtensions
    {
        /// <summary>
        /// Returns the opposite connector of a tab or a blank.
        /// </summary>
        /// <param name="c">The connector type you want the opposite of.</param>
        /// <returns>The opposite connector type.</returns>
        public static ConnectorType GetOppositeConnector(this ConnectorType c) =>
            c == ConnectorType.TAB ? ConnectorType.BLANK : ConnectorType.TAB;
    }
}