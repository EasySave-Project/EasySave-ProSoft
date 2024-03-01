using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySave.controller;
using EasySave.model;
using EasySave.view;
using Newtonsoft.Json;

namespace EasySave.services
{
    public enum ServerAction
    {
        start = 0,
        stop = 1,
        suspend = 2,
        resume = 3
    }

    public class ServerManager
    {
        private Socket serverSocket;
        private Socket clientSocket;

        private bool isPaused = false;
        private readonly object pauseLock = new object();

        private static ServerManager _instance;

        public static ServerManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServerManager();
            }
            return _instance;
        }

        public void SetServerSocket(Socket serverSocket)
        {
            this.serverSocket = serverSocket;
        }

        public void StartServer()
        {
            // Vérifie si le serveur n'est pas déjà démarré
            if (serverSocket == null || !serverSocket.IsBound)
            {
                // Démarrer le serveur dans un nouveau thread
                Thread startThread = new Thread(() =>
                {
                    try
                    {
                        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
                        serverSocket.Listen(10);

                        // Attendez la connexion d'un client
                        clientSocket = serverSocket.Accept();

                        IPEndPoint clientEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
                        string clientInfo = $"Client connecté : {clientEndPoint.Address}:{clientEndPoint.Port}";
                        //displayMessage?.Invoke(clientInfo);

                        // Lancez les threads pour la réception et l'envoi de messages
                        Thread receiveThread = new Thread(Receive);
                        receiveThread.Start();

                        Thread sendThread = new Thread(Send);
                        sendThread.Start();
                    }
                    catch (Exception ex)
                    {
                        //displayMessage?.Invoke($"Erreur lors de la connexion au serveur : {ex.Message}");
                    }
                });
                startThread.Start();
            }
        }

        public bool IsServerStarted()
        {
            bool returnServerStarted = false;
            try
            {
                // Savoir si le serveur est start ou non
                returnServerStarted = serverSocket != null && serverSocket.IsBound;
            }
            catch
            {
                // If error : Connection could not be established
                returnServerStarted = false;
            }
            return returnServerStarted;
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

                    // Diviser le message en parties en utilisant le point-virgule comme délimiteur
                    string[] parts = message.Split(';');

                    // Vérifier si le message a le bon format
                    if (parts.Length == 2 && int.TryParse(parts[0], out int jobId) && Enum.TryParse(parts[1], out ServerAction action))
                    {
                        // Utiliser l'ID du travail et l'action du serveur pour exécuter l'action appropriée
                        switch (action)
                        {
                            case ServerAction.start:
                                MainWindow mainWindow = null;
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });

                                // Exécuter du job dans un thread séparé
                                Thread jobThread = new Thread(() =>
                                {
                                    try
                                    {
                                        // Récupérer le MainWindow à partir du thread de l'interface utilisateur
                                        MainWindow mainWindow = System.Windows.Application.Current.Dispatcher.Invoke(() => (MainWindow)System.Windows.Application.Current.MainWindow);
                                        BackUpController backUpController = null;

                                        // Utiliser Dispatcher.Invoke pour accéder aux éléments de l'interface utilisateur
                                        mainWindow.Dispatcher.Invoke(() =>
                                        {
                                            backUpController = mainWindow.backUpController;
                                        });

                                        if (backUpController != null)
                                        {
                                            // Exécuter la logique du job à partir du thread de l'interface utilisateur
                                            backUpController.InitiateBackUpJob(BackUpManager.listBackUps[jobId]);
                                        }
                                    }
                                    catch
                                    {
                                        System.Windows.MessageBox.Show(ManageLang.GetString("error_save"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                });

                                // Faire du thread un thread d'arrière-plan
                                jobThread.IsBackground = true;

                                // Démarrer le thread
                                jobThread.Start();
                                break;
                            case ServerAction.stop:
                                //threadManager.StopJobThread(jobId);
                                break;
                            case ServerAction.suspend:
                                //threadManager.SuspendJobThread(jobId);
                                break;
                            case ServerAction.resume:
                                //threadManager.ResumeJobThread(jobId);
                                break;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                //displayMessage?.Invoke("Le client a été déconnecté.");
            }
        }

        private void Send()
        {
            try
            {
                while (clientSocket.Connected)
                {
                    lock (pauseLock)
                    {
                        if (!isPaused)
                        {
                            // Création de la liste d'objets Message
                            JobObjectFactory messageFactory = new JobObjectFactory();
                            List<model.JobObject> send_message = messageFactory.CreateJobObject();

                            // Sérialisation de la liste d'objets Message en JSON
                            string json_message = JsonConvert.SerializeObject(send_message);

                            // Création du buffer à partir de la chaîne JSON
                            byte[] buffer = Encoding.UTF8.GetBytes(json_message);
                            clientSocket.Send(buffer);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (SocketException)
            {
                // Le client a été déconnecté
                clientSocket.Close();
                //displayMessage?.Invoke("Le client a été déconnecté.");
            }
        }


    }
}
