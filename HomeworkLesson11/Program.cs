using System;
using System.Collections.Generic;
using System.Threading;

namespace HomeworkLesson11
{
    class Program
    {
        static void Main(string[] args)
        {
            TimerCallback tm = new TimerCallback(clientOperations.balanceChecker);
            Timer timer = new Timer(tm, null, 0, 1000);

            List<int> addResults = new List<int>();
            Thread insertThread = new Thread(() => 
            { 
                addResults.Add(clientOperations.Insert(13));
                addResults.Add(clientOperations.Insert(15));  
            });
            insertThread.Start();
            insertThread.Join();

            List<bool> updateResults = new List<bool>();
            Thread updateThread = new Thread(() => updateResults.Add(clientOperations.Update(1, 29)));
            updateThread.Start();
            updateThread.Join();

            List<bool> deleteResults = new List<bool>();
            Thread deleteThread = new Thread(() => deleteResults.Add(clientOperations.Delete(1)));
            deleteThread.Start();
            deleteThread.Join();

            List<Client> selectByIdResults = new List<Client>();
            Thread selectByIdThread = new Thread(() => selectByIdResults.Add(clientOperations.SelectByID(2)));
            selectByIdThread.Start();
            selectByIdThread.Join();

            List<List<Client>> selectAllResults = new List<List<Client>>();
            Thread selectAllThread = new Thread(() => clientOperations.SelectAll());
            selectAllThread.Start();
            selectAllThread.Join();
        }
    }
    
}
