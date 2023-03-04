using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AdamTest
{
    public class AdvantechUtil
    {
        public AdvantechUtil() { }

        public AdvantechUtil(string ip)
        {
            m_szIP = ip;
        }

        private string m_szIP = "192.168.1.31";
        private int m_iCount, m_iPort;
        private int m_iStart, m_iLength;
        private AdamSocket adamTCP;
        private bool m_bRegister = true, m_bStart;
        public string ErrorMEssage = null;

        public bool IsConnected
        {
            get
            {
                return adamTCP != null && adamTCP.Connected;
            }
        }

        public void Disconnect()
        {
            if (adamTCP != null && adamTCP.Connected)
            {
                adamTCP.Disconnect();
            }
            adamTCP = null;
        }

        public void Connect()
        {
            int iIdx, iPos, iStart;

            m_iPort = 502;
            m_iStart = 1;
            m_iLength = 25;
            m_bRegister = true;
            adamTCP = new AdamSocket();
            adamTCP.SetTimeout(1000, 1000, 1000);

            if (m_bRegister)
            {
                iStart = 40000 + m_iStart;
                for (iIdx = 0; iIdx < m_iLength; iIdx++)
                {
                    iPos = iStart + iIdx;
                }
            }
            else
            {
                iStart = m_iStart;
                for (iIdx = 0; iIdx < m_iLength; iIdx++)
                {
                    iPos = iStart + iIdx;
                }
            }

            if (adamTCP.Connect(m_szIP, ProtocolType.Tcp, m_iPort))
            {
                System.Diagnostics.Trace.WriteLine($"Adam cihazına bağlanıldı {m_szIP}");
                m_iCount = 0;
                m_bStart = true;
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("Connect to " + m_szIP + " failed");
            }
        }

        public int Read(int pindIndex = 0)
        {
            //int iIdx;

            if (!IsConnected) Connect();

            if (m_bRegister)
            {
                uint[] iData;
                if (adamTCP.Modbus().ReadHoldingRegs(m_iStart, m_iLength, out iData))
                {
                    m_iCount++;

                    return Convert.ToInt32(iData[pindIndex]);

                    //for (iIdx = 0; iIdx < m_iLength; iIdx++)
                    //{
                    //listViewModbusCur.Items[iIdx].SubItems[2].Text = iData[iIdx].ToString();
                    //listViewModbusCur.Items[iIdx].SubItems[3].Text = iData[iIdx].ToString("X04");
                    //if (iIdx == 0) Sensor1.Text = iData[iIdx].ToString();
                    //if (iIdx == 1) Sensor2.Text = iData[iIdx].ToString();
                    //if (iIdx == 2) Sensor3.Text = iData[iIdx].ToString();
                    //if (iIdx == 3) Sensor4.Text = iData[iIdx].ToString();
                    //if (iIdx == 4) Sensor5.Text = iData[iIdx].ToString();
                    //if (iIdx == 5) Sensor6.Text = iData[iIdx].ToString();
                    //if (iIdx == 6) Sensor7.Text = iData[iIdx].ToString();
                    //if (iIdx == 7) Sensor8.Text = iData[iIdx].ToString();
                    //if (iIdx == 8) Sensor9.Text = iData[iIdx].ToString();
                    //if (iIdx == 9) Sensor10.Text = iData[iIdx].ToString();
                    //if (iIdx == 10) Sensor11.Text = iData[iIdx].ToString();
                    //if (iIdx == 11) Sensor12.Text = iData[iIdx].ToString();
                    //}
                }
                else
                {
                    Disconnect();
                    System.Diagnostics.Trace.WriteLine("Read registers failed!");
                }
            }
            else
            {
                bool[] bData;
                if (adamTCP.Modbus().ReadCoilStatus(m_iStart, m_iLength, out bData))
                {
                    m_iCount++;
                    System.Diagnostics.Trace.WriteLine("Read coil " + m_iCount.ToString() + " times...");

                    //for (iIdx = 0; iIdx < m_iLength; iIdx++)
                    //{
                    //    listViewModbusCur.Items[iIdx].SubItems[2].Text = bData[iIdx].ToString();
                    //}
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("Read coil failed!");
                }
            }
            return 0;
        }

        public void Write()
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("Sıfırlama Adam cihazı sıfırlanacak");
                if (!IsConnected) Connect();

                int[] Siralama = { 34, 38, 42, 46, 50, 54, 58, 62, 66, 70, 74, 78 };

                for (int i = 0; i < Siralama.Length; i++)
                {
                    adamTCP.Modbus(1).ForceSingleCoil(Siralama[i], 1);
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.WriteLine(exc);
            }
        }

        //private int m_iDiTotal = 12;
        //private int m_iDoTotal = 6;

        private int m_iDiTotal = 12;
        private int m_iDoTotal = 6;

        public bool[] RefreshDio()
        {
            bool[] flagArray;
            bool[] flagArray2;
            bool[] flagArray4;
            bool[] flagArray5;
            uint[] numArray;
            var num = 1;
            var num2 = 17; //0x11;
            var num3 = 36; //0x24;
            var destinationArray = new bool[num3];
            if (adamTCP.Modbus().ReadCoilStatus(num, m_iDiTotal, out flagArray) && adamTCP.Modbus().ReadCoilStatus(num2, m_iDoTotal, out flagArray2))
            {
                Array.Copy(flagArray, 0, destinationArray, 0, 6);
                Array.Copy(flagArray, 6, destinationArray, 12, 6);
                Array.Copy(flagArray2, 0, destinationArray, 24, m_iDoTotal);
            }
            //if (adamTCP.Modbus().ReadCoilStatus(num, m_iDiTotal, out flagArray4) && adamTCP.Modbus().ReadCoilStatus(num2, m_iDoTotal, out flagArray5))
            //{
            //    Array.Copy(flagArray4, 0, destinationArray, 6, 6);
            //    Array.Copy(flagArray4, 6, destinationArray, 18, 6);
            //    Array.Copy(flagArray5, 0, destinationArray, m_iDiTotal + 18, m_iDoTotal);
            //}

            //if (adamTCP.Modbus().ReadHoldingRegs(num, m_iDiTotal, out numArray))
            //{
            //    var s1 = numArray[10].ToString();
            //    var s2 = numArray[11].ToString();
            //}
            return destinationArray;
        }

        public void WriteDio(int iICh, int iOnOff)
        {
            int num;
            if ((iICh >= 24) && (iICh < 30))
            {
                num = (17 + iICh) - (m_iDiTotal + 12);
                if (adamTCP.Modbus().ForceSingleCoil(num, iOnOff))
                {
                    RefreshDio();
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("Set digital output failed!");
                }
            }
            if ((iICh >= 30) && (iICh < 36))
            {
                num = (17 + iICh) - (m_iDiTotal + 18);
                if (adamTCP.Modbus().ForceSingleCoil(num, iOnOff))
                {
                    RefreshDio();
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("Set digital output failed!");
                }
            }
        }
    }
}
