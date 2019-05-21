using PcapngUtils;
using PcapngUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PcapngDissector
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            OpenPcapORPcapNFFile("FileName.pcapng", cts.Token);            
        }

        public static void OpenPcapORPcapNFFile(string filename, CancellationToken token)
        {
            using (var reader = IReaderFactory.GetReader(filename))
            {
                reader.OnReadPacketEvent += reader_OnReadPacketEvent;
                reader.ReadPackets(token);
                reader.OnReadPacketEvent -= reader_OnReadPacketEvent;
            }
        }
        static void reader_OnReadPacketEvent(object context, IPacket packet)
        {

            string Source_Ip = packet.Data[26] + "." + packet.Data[27] + "." + packet.Data[28] + "." + packet.Data[29];
            string Destination_Ip = packet.Data[30] + "." + packet.Data[31] + "." + packet.Data[32] + "." + packet.Data[33];

            //實際封包傳輸內容
            byte[] body = new byte[packet.Data.Length - 42];
            Array.Copy(packet.Data, 42, body, 0, body.Length);

            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(packet.Seconds);
            Console.WriteLine(string.Format("Packet received {0}.{1}", dt.ToString("HH:mm:ss"), packet.Microseconds));            
        }
    }
}
