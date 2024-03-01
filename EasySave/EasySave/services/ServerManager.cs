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
        resume = 3,
        allstart = 4,
        allsuspend = 5,
        allstop = 6
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
            // Checks if the server has already been started
            if (serverSocket == null || !serverSocket.IsBound)
            {
                // Start the server in a new thread
                Thread startThread = new Thread(() =>
                {
                    try
                    {
                        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
                        serverSocket.Listen(10);

                        // Wait for a client connection
                        clientSocket = serverSocket.Accept();

                        IPEndPoint clientEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
                        string clientInfo = $"Client connecté : {clientEndPoint.Address}:{clientEndPoint.Port}";

                        // Lancez les threads pour la réception et l'envoi de messages
                        Thread receiveThread = new Thread(Receive);
                        receiveThread.Start();

                        Thread sendThread = new Thread(Send);
                        sendThread.Start();
                    }
                    catch (Exception ex)
                    {
                        // Server connection error
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
                // Find out if the server is start or not
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

                    // Find out if the server is start or not
                    string[] parts = message.Split(';');

                    // Check message format
                    if (parts.Length == 2 && int.TryParse(parts[0], out int jobId) && Enum.TryParse(parts[1], out ServerAction action))
                    {
                        MainWindow mainWindow = null;
                        // Utiliser l'ID du travail et l'action du serveur pour exécuter l'action appropriée
                        switch (action)
                        {
                            case ServerAction.start:
                                // START A JOB
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });

                                // Calculate global job index based on current page
                                BackUpManager.listBackUps[jobId].ResetJob();

                                mainWindow.backUpController.backUpManager.ResetStopJob(BackUpManager.listBackUps[jobId]);
                                try
                                {
                                    mainWindow.backUpController.InitiateBackUpJob(BackUpManager.listBackUps[jobId]);
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show(ManageLang.GetString("error_save"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                break;

                            case ServerAction.stop:
                                // STOP A JOB
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });
                                try
                                {
                                    mainWindow.backUpController.backUpManager.StopBackup(BackUpManager.listBackUps[jobId]);
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("Error on the pause :" + ex.Message);
                                }
                                break;

                            case ServerAction.suspend:
                                // SUSPEND A JOB
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });
                                try
                                {
                                    mainWindow.backUpController.backUpManager.PauseBackup(BackUpManager.listBackUps[jobId]);
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("error pause job :  " + ex.Message);
                                }
                                break;

                            case ServerAction.resume:
                                // RESUME A JOB
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });
                                BackUpManager.listBackUps[jobId].ResetJob();

                                mainWindow.backUpController.backUpManager.ResetStopJob(BackUpManager.listBackUps[jobId]);
                                try
                                {
                                    mainWindow.backUpController.InitiateResumeBackUp(BackUpManager.listBackUps[jobId]);
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("error pause job :  " + ex.Message);
                                }
                                break;

                            case ServerAction.allstart:
                                // START ALL JOBS
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });
                                try
                                {
                                    for (int i = 1; i <= BackUpManager.listBackUps.Count; i++)
                                    {
                                        mainWindow.backUpController.InitiateBackUpJob(BackUpManager.listBackUps[i]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("error pause job :  " + ex.Message);
                                }
                                break;

                            case ServerAction.allsuspend:
                                // SUSPEND ALL JOBS
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });
                                try
                                {
                                    for (int i = 1; i <= BackUpManager.listBackUps.Count; i++)
                                    {
                                        mainWindow.backUpController.backUpManager.PauseBackup(BackUpManager.listBackUps[i]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("error pause job :  " + ex.Message);
                                }
                                break;

                            case ServerAction.allstop:
                                // STOP ALL JOBS
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                                });
                                try
                                {
                                    for (int i = 1; i <= BackUpManager.listBackUps.Count; i++)
                                    {
                                        mainWindow.backUpController.backUpManager.StopBackup(BackUpManager.listBackUps[i]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("error pause job :  " + ex.Message);
                                }
                                break;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                // The customer has been disconnected

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
                            // Message object list creation
                            JobObjectFactory messageFactory = new JobObjectFactory();
                            List<model.JobObject> send_message = messageFactory.CreateJobObject();

                            // JSON serialization of the Message object list
                            string json_message = JsonConvert.SerializeObject(send_message);

                            // Add clear separator at end of JSON message
                            string messageWithSeparator = json_message + "\r\n";

                            // Buffer creation from JSON string
                            byte[] buffer = Encoding.UTF8.GetBytes(messageWithSeparator);
                            clientSocket.Send(buffer);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (SocketException)
            {
                // The customer has been disconnected
                clientSocket.Close();
            }
        }


    }
}
