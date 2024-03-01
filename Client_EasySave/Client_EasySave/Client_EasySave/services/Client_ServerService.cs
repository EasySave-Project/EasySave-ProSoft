using Client_EasySave.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Client_EasySave.ViewModel;
using Client_EasySave;

namespace EasySaveClient.Services
{
    // Enumeration to define possible actions that can be sent to the server
    public enum ServerAction
    {
        start = 0,
        stop = 1,
        suspend = 2,
        resume = 3,
        allstart = 4,
        allsuspend = 5,
        allstop = 6
    }

    // Class to represent a message object sent to the server
    public class ServerMessageObject
    {
        public int jobId { get; set; }
        public ServerAction serverAction { get; set; }

        public ServerMessageObject(int jobId, ServerAction serverAction)
        {
            this.jobId = jobId;
            this.serverAction = serverAction;
        }
    }

    // Manages the communication with the server
    public class ServerManager
    {
        private Socket clientSocket;
        private JobViewModels viewModel;
        private MainWindow mainWindow;

        public ServerManager(MainWindow mainWindow)
        {
            viewModel = JobViewModels.GetInstance();
            this.mainWindow = mainWindow;
        }

        // Connects to the server
        public void ConnectToServer()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 8888));

            Thread receiveThread = new Thread(Receive);
            receiveThread.Start();

            this.clientSocket = clientSocket;
        }

        // Checks if the client is connected to the server
        public bool IsConnected()
        {
            bool valueReturn = false;
            try
            {
                valueReturn = clientSocket.Connected;
            }
            catch
            {
                valueReturn = false;
            }
            return valueReturn;
        }

        // Sends an action to the server
        public void SendAction(int jobId, ServerAction action)
        {
            string message = jobId + ";" + (int)action;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(buffer);
        }

        // Receives messages from the server
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceived = clientSocket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                    int separatorIndex = message.IndexOf("\r\n");

                    if (separatorIndex >= 0)
                    {
                        try
                        {
                            string jsonMessage = message.Substring(0, separatorIndex);

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                viewModel.LoadJson(jsonMessage);
                                mainWindow.ReloadData();
                            });
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error : " + e.Message);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                clientSocket.Close();
            }
        }
    }
}
