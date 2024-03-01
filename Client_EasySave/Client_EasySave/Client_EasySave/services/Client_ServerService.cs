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
    public enum ServerAction
    {
        start = 0,
        stop = 1,
        suspend = 2,
        resume = 3
    }

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

        public void ConnectToServer()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 8888));
            //DisplayMessage("Connecté au serveur.");

            Thread receiveThread = new Thread(Receive);
            receiveThread.Start();

            this.clientSocket = clientSocket;
        }

        public bool IsConnected()
        {
            bool valueReturn = false;
            try
            {
                valueReturn = clientSocket.Connected;
            }
            catch
            {
                // If error : Connection could not be established
                valueReturn = false;
            }
            return valueReturn;
        }

        public void SendAction(int jobId, ServerAction action)
        {
            //ServerMessageObject messageObject = new ServerMessageObject(jobId, action);
            //string message = JsonSerializer.Serialize(messageObject, new JsonSerializerOptions { WriteIndented = true });
            string message = jobId + ";" + (int)action;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(buffer);
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceived = clientSocket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

                    // Trouver l'index du premier séparateur
                    int separatorIndex = message.IndexOf("\r\n");

                    // Vérifier si le séparateur a été trouvé
                    if (separatorIndex >= 0)
                    {
                        // Extraire la partie de la chaîne jusqu'au premier séparateur
                        string jsonMessage = message.Substring(0, separatorIndex);

                        // Utilisez Dispatcher ici pour appeler LoadJson sur le thread UI
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            // Appeler la méthode LoadJson du viewModel
                            viewModel.LoadJson(jsonMessage);
                            mainWindow.ReloadData();
                        });
                    }
                }
            }
            catch (SocketException)
            {
                clientSocket.Close();
                // Utilisez Dispatcher ici si vous souhaitez afficher un message d'erreur
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //DisplayMessage("Le serveur a été déconnecté.");
                });
            }
        }

    }
}
