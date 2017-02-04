using System.Diagnostics;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a Id of a <see cref="Led"/> on a <see cref="LogitechRGBDevice"/>.
    /// </summary>
    [DebuggerDisplay("{" + nameof(LedId) + "}")]
    public class LogitechLedId : ILedId
    {
        #region Properties & Fields

        internal readonly LogitechLedIds LedId;

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => LedId != LogitechLedIds.Invalid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="LogitechLedId"/> of the represented <see cref="Led"/>.</param>
        public LogitechLedId(IRGBDevice device, LogitechLedIds ledId)
        {
            this.Device = device;
            this.LedId = ledId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="LogitechLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="LogitechLedId"/>. For example "Enter".</returns>
        public override string ToString()
        {
            return LedId.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="LogitechLedId" /> and is equivalent to this <see cref="LogitechLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="LogitechLedId" /> equivalent to this <see cref="LogitechLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            LogitechLedId compareLedId = obj as LogitechLedId;
            if (ReferenceEquals(compareLedId, null))
                return false;

            if (ReferenceEquals(this, compareLedId))
                return true;

            if (GetType() != compareLedId.GetType())
                return false;

            return compareLedId.LedId == LedId;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="LogitechLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="LogitechLedId" />.</returns>
        public override int GetHashCode()
        {
            return LedId.GetHashCode();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="LogitechLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="LogitechLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="LogitechLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LogitechLedId ledId1, LogitechLedId ledId2)
        {
            return ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="LogitechLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="LogitechLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="LogitechLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LogitechLedId ledId1, LogitechLedId ledId2)
        {
            return !(ledId1 == ledId2);
        }

        #endregion
    }
}
