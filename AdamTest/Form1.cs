using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil();
            adam.Connect();
            var aa = adam.Read(0);
            adam.Write();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ADAM-4050/LAN cihazının IP adresi ve port numarası
            string ipAddress = "192.168.1.31";
            int port = 502;

            // DI1 ve DI2 girişlerine ait modbus adresleri
            int di1Address = 1;
            int di2Address = 2;

            // TCP/IP bağlantısı oluşturma
            TcpClient tcpClient = new TcpClient(ipAddress, port);
            NetworkStream networkStream = tcpClient.GetStream();

            // DI1 girişinin durumunu okuma
            byte[] di1Command = { 0x01, 0x01, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };
            networkStream.Write(di1Command, 0, di1Command.Length);
            byte[] di1Response = new byte[12];
            networkStream.Read(di1Response, 0, di1Response.Length);
            bool di1Status = (di1Response[9] & 0x01) == 0x01;

            // DI2 girişinin durumunu okuma
            byte[] di2Command = { 0x01, 0x01, 0x00, 0x01, 0x00, 0x01, 0xC5, 0xCA };
            networkStream.Write(di2Command, 0, di2Command.Length);
            byte[] di2Response = new byte[12];
            networkStream.Read(di2Response, 0, di2Response.Length);
            bool di2Status = (di2Response[9] & 0x01) == 0x01;

            // TCP/IP bağlantısını kapatma
            networkStream.Close();
            tcpClient.Close();

            // DI1 ve DI2 girişlerinin durumlarını yazdırma
            Console.WriteLine("DI1: {0}", di1Status);
            Console.WriteLine("DI2: {0}", di2Status);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ADAM-4050/LAN cihazının IP adresi ve port numarası
            string ipAddress = "192.168.1.31";
            int port = 502;

            // DO1 ve DO2 çıkışlarına ait modbus adresleri
            int do1Address = 1;
            int do2Address = 2;

            // TCP/IP bağlantısı oluşturma
            TcpClient tcpClient = new TcpClient(ipAddress, port);
            NetworkStream networkStream = tcpClient.GetStream();

            // DO1 çıkışını 1 yapma
            byte[] do1CommandOn = { 0x01, 0x06, 0x00, (byte)do1Address, 0xFF, 0x00, 0x8C, 0x41 };
            networkStream.Write(do1CommandOn, 0, do1CommandOn.Length);
            byte[] do1ResponseOn = new byte[4];
            networkStream.Read(do1ResponseOn, 0, do1ResponseOn.Length);

            // DO2 çıkışını 0 yapma
            byte[] do2CommandOff = { 0x01, 0x06, 0x00, (byte)do2Address, 0x00, 0x00, 0xCD, 0x8A };
            networkStream.Write(do2CommandOff, 0, do2CommandOff.Length);
            byte[] do2ResponseOff = new byte[4];
            networkStream.Read(do2ResponseOff, 0, do2ResponseOff.Length);

            // TCP/IP bağlantısını kapatma
            networkStream.Close();
            tcpClient.Close();

            Console.WriteLine("DO1: 1");
            Console.WriteLine("DO2: 0");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil();
            adam.Connect();
            adam.RefreshDio();
            adam.WriteDio(24, 0);
            adam.WriteDio(25, 0);
            adam.WriteDio(26, 0);
            adam.WriteDio(27, 0);
            adam.WriteDio(24, 1);
        }
    }
}
