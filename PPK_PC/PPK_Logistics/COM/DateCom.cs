using PPK_Logistics.Config;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace PPK_Logistics.COM
{
    public class DateCom
    {
        private SerialPort ComPort = null;//new SerialPort();//

        public Dictionary<string,SerialPort> ListProt = new Dictionary<string,SerialPort>(4);


        //public List<string> ComName = new List<string>(4);

        public string Name = "";
        public string Notes = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        private string errorPort = "";

        public DateCom()
        {
           
        }

        public string ErrorPort
        {
            get { return errorPort; }
            set { errorPort = value; }
        }

        public bool GetComData()
        {
            return true;
        }

        public Array GetComName()
        {
            return Tools._ComName.ToArray();
        }

        public bool IsOpen(string keystring)
        {
            return ListProt[keystring].IsOpen;
        }

        public bool OpenComParam(string baudRate, string dataBits, Parity parity, string protName, StopBits stopBits, Handshake handshake,string notes="")
        {
            try
            {
                if (!ListProt.ContainsKey(protName))
                {
                    ComPort = new SerialPort();
                    ComPort.PortName = protName;
                    ComPort.BaudRate = int.Parse(baudRate);
                    ComPort.DataBits = int.Parse(dataBits);
                    ComPort.Parity = parity;
                    ComPort.StopBits = stopBits;
                    ComPort.Handshake = handshake;
                    ComPort.Open();
                    //ComPort.DataReceived += ComPort_DataReceived;
                    ComPort.ErrorReceived += ComPort_ErrorReceived;

                    this.Name = protName;
                    if(notes!="") this.Notes = notes;

                    if (notes == "") this.Notes = protName;

                    ListProt.Add(protName, ComPort);
                    //ComName.Add(protName);
                }
                else
                {
                    return false;                         
                }
                    

                return true;
                
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public SerialPort GetSerialPort(string comName)
        {
            return ListProt.First(F=>F.Key.Contains(comName)).Value;
        }


        private void ComPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (e.EventType == SerialError.RXParity)
            {
                ErrorPort = "寄偶校验错误";
            }
            // throw new NotImplementedException();
        }
    }
}