using System;

namespace RGB.NET.Devices.WS281X;

/// <summary>
/// Represents a generic serial connection.
/// </summary>
public interface ISerialConnection : IDisposable
{
    /// <summary>
    /// Gets the COM-port used by the serial connection.
    /// </summary>
    string Port { get; }

    /// <summary>
    /// Gets the baud-rate used by the serial connection.
    /// </summary>
    int BaudRate { get; }

    /// <summary>
    /// Gets the connection-status of the serial connection.
    /// <c>true</c> if connected; otherwise <c>false</c>.
    /// </summary>
    bool IsOpen { get; }

    /// <summary>
    /// Opens the serial connection.
    /// </summary>
    void Open();

    /// <summary>
    /// Discards the in-buffer of the serial connection.
    /// </summary>
    void DiscardInBuffer();

    /// <summary>
    /// Reads a single byte from the serial connection
    /// </summary>
    /// <returns>The byte read.</returns>
    byte ReadByte();

    /// <summary>
    /// Blocks till the provided char is received from the serial connection.
    /// </summary>
    /// <param name="target">The target-character to read to.</param>
    void ReadTo(char target);

    /// <summary>
    /// Writes the provided data to the serial connection.
    /// </summary>
    /// <param name="buffer">The buffer containing the data to write.</param>
    /// <param name="offset">The offset of the data in the buffer.</param>
    /// <param name="length">The amount of data to write.</param>
    void Write(byte[] buffer, int offset, int length);

    /// <summary>
    /// Write the provided text to the serial connection followed by a line break.
    /// </summary>
    /// <param name="line">The text to write.</param>
    void WriteLine(string line);
}