using EasySave.model;
using EasySave.services;
using EasySave.utils;
using Microsoft.Extensions.Configuration;
using System.IO;
using EasySave.view;
using EasySave.controller;
namespace EasySave
{
    public class Program
    {

        public static void Main(string[] args)
        {

            
            ConsoleView cv = new ConsoleView();
            cv.InitConfFolder();
            BackUpManager bmManager = new BackUpManager();
            StateManager stateManager = new StateManager();
            LogManager logManager = new LogManager();
            BackUpController controller = new BackUpController(bmManager,logManager, stateManager);        
            cv.backUpController = controller;
            cv.ShowSelectLanguage();
            cv.ShowMainMenu();

            Console.ReadKey();
        
        }
    }
}