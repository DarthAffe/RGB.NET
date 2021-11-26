using System;

namespace RGB.NET.Core;

/// <summary>
/// Contains helper methods for converting things.
/// </summary>
public static class ConversionHelper
{
    #region Methods

    // Source: https://web.archive.org/web/20180224104425/https://stackoverflow.com/questions/623104/byte-to-hex-string/3974535
    /// <summary>
    /// Converts an array of bytes to a HEX-representation.
    /// </summary>
    /// <param name="bytes">The array of bytes.</param>
    /// <returns>The HEX-representation of the provided bytes.</returns>
    public static string ToHex(params byte[] bytes)
    {
        char[] c = new char[bytes.Length * 2];

        for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
        {
            byte b = ((byte)(bytes[bx] >> 4));
            c[cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);

            b = ((byte)(bytes[bx] & 0x0F));
            c[++cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);
        }

        return new string(c);
    }

    // Source: https://web.archive.org/web/20180224104425/https://stackoverflow.com/questions/623104/byte-to-hex-string/3974535
    /// <summary>
    /// Converts the HEX-representation of a byte array to that array.
    /// </summary>
    /// <param name="hexString">The HEX-string to convert.</param>
    /// <returns>The correspondending byte array.</returns>
    public static byte[] HexToBytes(ReadOnlySpan<char> hexString)
    {
        if ((hexString.Length == 0) || ((hexString.Length % 2) != 0))
            return Array.Empty<byte>();

        byte[] buffer = new byte[hexString.Length / 2];
        for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
        {
            // Convert first half of byte
            char c = hexString[sx];
            buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? ((c - 'a') + 10) : ((c - 'A') + 10)) : (c - '0')) << 4);

            // Convert second half of byte
            c = hexString[++sx];
            buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? ((c - 'a') + 10) : ((c - 'A') + 10)) : (c - '0'));
        }

        return buffer;
    }

    #endregion
}