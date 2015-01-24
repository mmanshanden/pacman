using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Network
{
    /// <summary>
    /// Helper class
    /// </summary>
    public class MessageParser
    {
        /// <summary>
        /// Writes the x float and y float to message.
        /// </summary>
        /// <param name="vector">Vector to write</param>
        /// <param name="msg">Message to write to</param>
        public static void WriteVector2(Vector2 vector, NetOutgoingMessage msg)
        {
            msg.Write(vector.X);
            msg.Write(vector.Y);
        }

        /// <summary>
        /// Reads vector's x and y values from a message
        /// </summary>
        /// <param name="msg">Message to read from</param>
        /// <returns>Vector read from message</returns>
        public static Vector2 ReadVector2(NetIncomingMessage msg)
        {
            return new Vector2(msg.ReadFloat(), msg.ReadFloat());
        }
    }
}
