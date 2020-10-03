using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tetra_Server
{
    public class NotificationQueue
    {
        public int clientID { get; set; }
        public Queue<string> files { get; set; }

        public void AddFilePath(string path)
        {
            files.Enqueue(path);
        }

        public string GetFirstFilePath()
        {
            return files.Dequeue();
        }

        public static void SendAllNotifications(Socket userSocket, IEnumerable<string> notificationsQueue, int clientID)
        {
            foreach (var item in notificationsQueue)
            {
                userSocket.SendAnyTypeOfFilesAsync(item, clientID);
            }
        }

        public static void ReadFromFile()
        {
            if (File.Exists("notificationStack.txt"))
            {
                using (var txtFile = new StreamReader("notificationStack.txt"))
                {
                    string notificationInfo;

                    while ((notificationInfo = txtFile.ReadLine()) != null)
                    {
                        var notification = notificationInfo.Split(',');

                        Queue<string> userNotificationsQueue;
                        int userID = int.Parse(notification[0]);

                        userNotificationsQueue = Server.Notifications.Find(n => n.clientID == userID).files;

                        if (userNotificationsQueue == null) userNotificationsQueue = new Queue<string>();

                        userNotificationsQueue.Enqueue(notification[1]);
                    }
                }
            }
        }

        public static void MakeBackUp()
        {
            if (File.Exists("notificationStack.txt"))
                File.Create("notificationStack.txt");

            using (var txtFile = new StreamWriter("notificationStack.txt"))
            {
                foreach (var instance in Server.Notifications)
                {
                    foreach (var path in instance.files)
                    {
                        txtFile.WriteLine($"{instance.clientID},{path}");
                    }
                }
            }
        }
    }
}
