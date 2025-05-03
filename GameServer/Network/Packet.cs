using System;
using System.Text;

namespace KoishiServer.GameServer.Network
{
    public class Packet
    {
        public ushort CommandId { get; set; }
        public ushort HeaderSize { get; set; } = 0;
        public uint BodySize { get; set; } = 0;
        public byte[] HeaderData { get; set; } = Array.Empty<byte>();
        public byte[] BodyData { get; set; } = Array.Empty<byte>();

        public static readonly byte[] HEAD_MAGIC = new byte[] { 0x9D, 0x74, 0xC7, 0x14 };
        public static readonly byte[] TAIL_MAGIC = new byte[] { 0xD7, 0xA1, 0x52, 0xC8 };

        // public override string ToString()
        // {
        //     StringBuilder sb = new StringBuilder();
        //     sb.AppendLine($"CommandId: 0x{CommandId:X4} ({CommandId})");
        //     sb.AppendLine($"HeaderSize: {HeaderSize} bytes");
        //     sb.AppendLine($"BodySize: {BodySize} bytes");
        //     sb.AppendLine($"HeaderData: {BitConverter.ToString(HeaderData)}");
        //     sb.AppendLine($"BodyData: {BitConverter.ToString(BodyData)}");
        //     return sb.ToString();
        // }

        public static Packet Headless(ushort commandId, byte[] bodyData)
        {
            Packet packet = new Packet
            {
                CommandId = commandId,
                BodyData = bodyData,
                BodySize = (uint)bodyData.Length,
            };

            return packet;
        }
    }
}
