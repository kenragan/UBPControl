using System;
using System.Text;
using System.Collections.Specialized;

using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.Net;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.CrestronIO;

namespace SonyUBPControl
{
    public class PlayerControl
    {

        /// <summary>
        /// SIMPL+ can only execute the default constructor. If you have variables that require initialization, please
        /// use an Initialize method
        /// </summary>
        public PlayerControl()
        {
            this.commandCollection = new NameValueCollection();
            this.PopulateCommandCollection();
        }
        
        private NameValueCollection commandCollection;  //collection of device commands
        private string ipAddress;    //IP address for device to be controlled
        private string macAddress;   //MAC address of device to be controlled
        
        private string ctrlMacAddress;  //MAC address of crestron control processor
        private string controllerName;  //descriptive name for the control system that will be stored in the device's registration settings
                
        private string registrationURL; //URL for registration
        private string registrationPort;    //Port for registration
        private string controlURL;  //URL for control
        private string controlPort; //Port for control
        
        
        public void Initialize(string ipAddress, string macAddress)
        {
            this.ipAddress = ipAddress;
            this.macAddress = macAddress;
            this.macAddress = this.macAddress.Replace('-', ':');
            
            //retrieve the Crestron Controller Mac Address which is needed when registering with the device to be controlled
            this.ctrlMacAddress = CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_MAC_ADDRESS, 0); //TODO: make option to pick which ethernet adapter for controllers like CP3N and PRO3
            this.ctrlMacAddress = this.ctrlMacAddress.Replace('-', ':');

            //default values for registration
            this.registrationPort = "50002";
            this.controllerName = "Crestron-MC3";
            string deviceId = Uri.EscapeDataString("CrestronCtrl:" + ctrlMacAddress);
            string registrationArgs = String.Format("name={0}&registrationType={1}&deviceId={2}&wolSupport={3}", controllerName, "initial", deviceId, "true");
            this.registrationURL = String.Format("http://{0}:{1}/register?{2}", this.ipAddress, this.registrationPort, registrationArgs);

            //default values for control
            string ircc_URL = "upnp/control/IRCC";
            this.controlPort = "50001";
            this.controlURL = String.Format("http://{0}:{1}/{2}", this.ipAddress, this.controlPort, ircc_URL);
        }


        /// <summary>
        /// Returns the device specific code string needed to execute the specified command name
        /// </summary>
        /// <param name="CommandName">Name of the Command</param>
        /// <returns></returns>
        private string LookUpCode(string CommandName)
        {
            return this.commandCollection[CommandName];
        }
        
        /// <summary>
        /// Add the shorthand descriptive hame for commands here and the corresponding command code for the device
        /// </summary>
        private void PopulateCommandCollection()
        {
            commandCollection.Clear();
            commandCollection.Add("Confirm", "AAAAAwAAHFoAAAA9Aw==");
            commandCollection.Add("Up", "AAAAAwAAHFoAAAA5Aw==");
            commandCollection.Add("Down", "AAAAAwAAHFoAAAA6Aw==");
            commandCollection.Add("Right", "AAAAAwAAHFoAAAA8Aw==");
            commandCollection.Add("Left", "AAAAAwAAHFoAAAA7Aw==");
            commandCollection.Add("Home", "AAAAAwAAHFoAAABCAw==");
            commandCollection.Add("Options", "AAAAAwAAHFoAAAA/Aw==");
            commandCollection.Add("Return", "AAAAAwAAHFoAAABDAw==");
            commandCollection.Add("Num1", "AAAAAwAAHFoAAAAAAw==");
            commandCollection.Add("Num2", "AAAAAwAAHFoAAAABAw==");
            commandCollection.Add("Num3", "AAAAAwAAHFoAAAACAw==");
            commandCollection.Add("Num4", "AAAAAwAAHFoAAAADAw==");
            commandCollection.Add("Num5", "AAAAAwAAHFoAAAAEAw==");
            commandCollection.Add("Num6", "AAAAAwAAHFoAAAAFAw==");
            commandCollection.Add("Num7", "AAAAAwAAHFoAAAAGAw==");
            commandCollection.Add("Num8", "AAAAAwAAHFoAAAAHAw==");
            commandCollection.Add("Num9", "AAAAAwAAHFoAAAAIAw==");
            commandCollection.Add("Num0", "AAAAAwAAHFoAAAAJAw==");
            commandCollection.Add("Power", "AAAAAwAAHFoAAAAVAw==");
            commandCollection.Add("PowerOff", "AAAAAwAAHFoAAAAvAw==");
            commandCollection.Add("Display", "AAAAAwAAHFoAAABBAw==");
            commandCollection.Add("Audio", "AAAAAwAAHFoAAABkAw==");
            commandCollection.Add("SubTitle", "AAAAAwAAHFoAAABjAw==");
            commandCollection.Add("Favorites", "AAAAAwAAHFoAAABeAw==");
            commandCollection.Add("Yellow", "AAAAAwAAHFoAAABpAw==");
            commandCollection.Add("Blue", "AAAAAwAAHFoAAABmAw==");
            commandCollection.Add("Red", "AAAAAwAAHFoAAABnAw==");
            commandCollection.Add("Green", "AAAAAwAAHFoAAABoAw==");
            commandCollection.Add("Play", "AAAAAwAAHFoAAAAaAw==");
            commandCollection.Add("Stop", "AAAAAwAAHFoAAAAYAw==");
            commandCollection.Add("Pause", "AAAAAwAAHFoAAAAZAw==");
            commandCollection.Add("Rewind", "AAAAAwAAHFoAAAAbAw==");
            commandCollection.Add("Forward", "AAAAAwAAHFoAAAAcAw==");
            commandCollection.Add("Prev", "AAAAAwAAHFoAAABXAw==");
            commandCollection.Add("Next", "AAAAAwAAHFoAAABWAw==");
            commandCollection.Add("Replay", "AAAAAwAAHFoAAAB2Aw==");
            commandCollection.Add("Advance", "AAAAAwAAHFoAAAB1Aw==");
            commandCollection.Add("Angle", "AAAAAwAAHFoAAABlAw==");
            commandCollection.Add("TopMenu", "AAAAAwAAHFoAAAAsAw==");
            commandCollection.Add("PopUpMenu", "AAAAAwAAHFoAAAApAw==");
            commandCollection.Add("Eject", "AAAAAwAAHFoAAAAWAw==");
            commandCollection.Add("Karaoke", "AAAAAwAAHFoAAABKAw==");
            commandCollection.Add("Netflix", "AAAAAwAAHFoAAABLAw==");
            commandCollection.Add("Mode3D", "AAAAAwAAHFoAAABNAw==");
            commandCollection.Add("Confirm", "AAAAAwAAHFoAAAA9Aw==");
            commandCollection.Add("Up", "AAAAAwAAHFoAAAA5Aw==");
            commandCollection.Add("Down", "AAAAAwAAHFoAAAA6Aw==");
            commandCollection.Add("Right", "AAAAAwAAHFoAAAA8Aw==");
            commandCollection.Add("Left", "AAAAAwAAHFoAAAA7Aw==");
            commandCollection.Add("Home", "AAAAAwAAHFoAAABCAw==");
            commandCollection.Add("Options", "AAAAAwAAHFoAAAA/Aw==");
            commandCollection.Add("Return", "AAAAAwAAHFoAAABDAw==");
            commandCollection.Add("Num1", "AAAAAwAAHFoAAAAAAw==");
            commandCollection.Add("Num2", "AAAAAwAAHFoAAAABAw==");
            commandCollection.Add("Num3", "AAAAAwAAHFoAAAACAw==");
            commandCollection.Add("Num4", "AAAAAwAAHFoAAAADAw==");
            commandCollection.Add("Num5", "AAAAAwAAHFoAAAAEAw==");
            commandCollection.Add("Num6", "AAAAAwAAHFoAAAAFAw==");
            commandCollection.Add("Num7", "AAAAAwAAHFoAAAAGAw==");
            commandCollection.Add("Num8", "AAAAAwAAHFoAAAAHAw==");
            commandCollection.Add("Num9", "AAAAAwAAHFoAAAAIAw==");
            commandCollection.Add("Num0", "AAAAAwAAHFoAAAAJAw==");
            commandCollection.Add("Power", "AAAAAwAAHFoAAAAVAw==");
            commandCollection.Add("Display", "AAAAAwAAHFoAAABBAw==");
            commandCollection.Add("Audio", "AAAAAwAAHFoAAABkAw==");
            commandCollection.Add("SubTitle", "AAAAAwAAHFoAAABjAw==");
            commandCollection.Add("Favorites", "AAAAAwAAHFoAAABeAw==");
            commandCollection.Add("Yellow", "AAAAAwAAHFoAAABpAw==");
            commandCollection.Add("Blue", "AAAAAwAAHFoAAABmAw==");
            commandCollection.Add("Red", "AAAAAwAAHFoAAABnAw==");
            commandCollection.Add("Green", "AAAAAwAAHFoAAABoAw==");
            commandCollection.Add("Play", "AAAAAwAAHFoAAAAaAw==");
            commandCollection.Add("Stop", "AAAAAwAAHFoAAAAYAw==");
            commandCollection.Add("Pause", "AAAAAwAAHFoAAAAZAw==");
            commandCollection.Add("Rewind", "AAAAAwAAHFoAAAAbAw==");
            commandCollection.Add("Forward", "AAAAAwAAHFoAAAAcAw==");
            commandCollection.Add("Prev", "AAAAAwAAHFoAAABXAw==");
            commandCollection.Add("Next", "AAAAAwAAHFoAAABWAw==");
            commandCollection.Add("Replay", "AAAAAwAAHFoAAAB2Aw==");
            commandCollection.Add("Advance", "AAAAAwAAHFoAAAB1Aw==");
            commandCollection.Add("Angle", "AAAAAwAAHFoAAABlAw==");
            commandCollection.Add("TopMenu", "AAAAAwAAHFoAAAAsAw==");
            commandCollection.Add("PopUpMenu", "AAAAAwAAHFoAAAApAw==");
            commandCollection.Add("Eject", "AAAAAwAAHFoAAAAWAw==");
            commandCollection.Add("Karaoke", "AAAAAwAAHFoAAABKAw==");
            commandCollection.Add("Netflix", "AAAAAwAAHFoAAABLAw==");
            commandCollection.Add("Mode3D", "AAAAAwAAHFoAAABNAw==");
        }

        /// <summary>
        /// Send specified command to the device for control
        /// </summary>
        /// <param name="Command">The short command name - Pause, Play, etc.</param>
        public void SendCommand(string Command)
        {
            StringBuilder command = new StringBuilder();
            command.Append("<?xml version=\"1.0\"?>");
            command.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">");
            command.Append("<s:Body>");
            command.Append("<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">");
            command.Append(String.Format("<IRCCCode>{0}</IRCCCode>", LookUpCode(Command)));
            command.Append("</u:X_SendIRCC>");
            command.Append("</s:Body>");
            command.Append("</s:Envelope>");

            byte[] commandBytes = System.Text.Encoding.UTF8.GetBytes(command.ToString());

            try
            {                
                //build http request
                var httpRequest = new HttpClientRequest();
                httpRequest.Url = new UrlParser(this.controlURL);
                httpRequest.RequestType = RequestType.Post;
                httpRequest.KeepAlive = true;
                httpRequest.ContentSource = ContentSource.ContentBytes;
                httpRequest.ContentBytes = commandBytes;
                
                //setup headers
                httpRequest.Header.AddHeader(new HttpHeader("X-CERS-DEVICE-ID", "CrestronCtrl:" + this.ctrlMacAddress));
                httpRequest.Header.AddHeader(new HttpHeader("SOAPACTION", "\"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\""));
                httpRequest.Header.AddHeader(new HttpHeader("content-type", "text/xml; charset=utf-8"));
                httpRequest.Header.AddHeader(new HttpHeader("Content-Length", commandBytes.Length.ToString()));

                //send the request
                HttpClient httpReqeustClient = new HttpClient();
                HttpRequestResponse response = httpReqeustClient.Dispatch(httpRequest);
               
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine(ex.ToString());
                CrestronConsole.PrintLine("URL: {0}, Command Body: {1}", this.controlURL, command.ToString());
            }

        }

        /// <summary>
        /// Sends a Wake on LAN (WOL) packet to the network to power the device on.  Assumes the device has already registered the crestron controller.
        /// </summary>
        public void PowerOn()
        {
            try
            {
                Byte[] wolPacket = new byte[102];
                for (int i = 0; i <= 5; i++)
                {
                    wolPacket[i] = 0xff;
                }
                string[] macByteValues = null;
                macByteValues = this.macAddress.Split(':'); //mac address of the device

                int start = 6;
                for (int i = 0; i < 16; i++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        wolPacket[start + i * 6 + x] = (byte)Convert.ToInt32(macByteValues[x], 16);
                    }
                }
                using (UDPServer wolClient = new UDPServer())
                {
                    SocketErrorCodes udpResults;

                    udpResults = wolClient.EnableUDPServer(IPAddress.Any, 0, 3);
                    if (udpResults != SocketErrorCodes.SOCKET_OK)
                        CrestronConsole.PrintLine("UDP WOL Init Error {0}", udpResults);
                    udpResults = wolClient.SendData(wolPacket, wolPacket.Length, this.ipAddress, 3, false);
                    if (udpResults != SocketErrorCodes.SOCKET_OK)
                        CrestronConsole.PrintLine("UDP WOL Sending Error {0}", udpResults);
                    udpResults = wolClient.DisableUDPServer();
                    if (udpResults != SocketErrorCodes.SOCKET_OK)
                        CrestronConsole.PrintLine("UDP WOL Cleanup Error {0}", udpResults);
                }
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("Error from PowerOn - WOL Exception: {0}", ex.ToString());
            }
        }
        
        /// <summary>
        /// Triggers registration process on the device.  Device will display PIN onscreen that must be sent via <see cref="PlayerControl.RegisterSubmit(string)"/>
        /// </summary>
        public void RegisterInit()
        {
            try
            {
                CrestronConsole.PrintLine("Calling registration URL: {0}", registrationURL);
                using (HttpClient client = new HttpClient())
                {
                    client.Url = new UrlParser(registrationURL);
                    client.Get(registrationURL);
                }       
            }
            catch (HttpException ex)
            {
                if (ex.Response.Code == 403) { }  //initial request to registration URL returns an http 403, that exception can be ignored
                else
                {
                    CrestronConsole.PrintLine(ex.ToString());
                }
            }            
        }

        /// <summary>
        /// Submits the PIN the device provides as part of registration.  Must be sent before the code expires while device is in registration mode
        /// </summary>
        /// <param name="PinCode">PIN from device</param>
        public void RegisterSubmit(string PinCode)
        {
            try
            {
                CrestronConsole.PrintLine("Sending PIN: {0} to registration URL: {1}", PinCode, registrationURL);
                PinCode = ":" + PinCode;
                string pinEncoded = Convert.ToBase64String(Encoding.Default.GetBytes(PinCode));
                string registrationResponse;

                using (HttpClient client = new HttpClient())
                {
                    var httpRequest = new HttpClientRequest();
                    httpRequest.Url = new UrlParser(registrationURL);
                    httpRequest.Header.AddHeader(new HttpHeader("Authorization", "Basic " + pinEncoded));
                    registrationResponse = ((HttpClientResponse)client.Dispatch(httpRequest)).ContentString;
                }
                CrestronConsole.PrintLine("Registration result: {0}", registrationResponse);
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine(ex.ToString());
            }
        }
    }
}
