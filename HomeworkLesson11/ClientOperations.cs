using System;
using System.Collections.Generic;
using System.Threading;

namespace HomeworkLesson11
{
    public class Client
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
    }
    public static class clientOperations
    {
        private static List<Client> clientList = new List<Client>();
        private static List<Client> shadowclientList = new List<Client>();
        private static int idCounter = 0;
        private static readonly Object obj = new Object();

        public static int Insert(decimal balance)
        {
            lock (obj)
            {
                idCounter++;
                clientList.Add(new Client {Id = idCounter, Balance = balance});
                shadowclientList.Add(new Client {Id = idCounter, Balance = balance});
            }
                Console.WriteLine($"Added 1 client, ID: {idCounter}, Balance {balance}");
                return idCounter;
        }
        public static bool Update(int id, decimal balance)
        {
            Monitor.Enter(obj);
            foreach (var item in clientList)
            {
                if (item.Id == id)
                {
                    item.Balance = balance;
                    return true;
                }
            }
            Monitor.Exit(obj);
            return false;
        }
        public static bool Delete(int id)
        {
            Monitor.Enter(obj);
            foreach (var item in clientList)
            {
                if (item.Id == id)
                {
                    clientList.Remove(item);
                    return true;
                }
            }
            foreach (var item in shadowclientList)
            {
                if (item.Id == id)
                {
                    shadowclientList.Remove(item);
                    return true;
                }
            }
            Monitor.Exit(obj);
            return false;
        }
        public static Client SelectByID(int id)
        {
            Monitor.Enter(obj);
            foreach (var item in clientList)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            Monitor.Exit(obj);
            return null;
        }
        public static List<Client> SelectAll()
        {
            return clientList;
        }
        public static void balanceChecker(object obj)
        {
            foreach (var item1 in clientList)
            {
                foreach (var item2 in shadowclientList)
                {
                    if (item1.Id == item2.Id)
                    {
                        if (item1.Balance < item2.Balance)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine
                            (
                                $"Client's ID: {item1.Id}," +
                                $"balance before change: {item2.Balance} and after change {item1.Balance}," +
                                $"total change in balance: -{item2.Balance - item1.Balance}"
                            );
                            Console.ResetColor();
                            item2.Balance = item1.Balance;
                        }
                        if (item1.Balance > item2.Balance)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine
                            (
                                $"Client's ID: {item1.Id}," +
                                $"balance before change: {item2.Balance} and after change {item1.Balance}," +
                                $"total change in balance: +{item1.Balance - item2.Balance}"
                            );
                            Console.ResetColor();
                            item2.Balance = item1.Balance;
                        }
                    }
                }
            }
        }
    }
}