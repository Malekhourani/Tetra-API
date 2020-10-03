using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tetra_API.Models;


namespace Tetra_Server
{
    public class Server
    {
        Socket ServerSocket;

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static List<Session> sessions;

        public static List<Session> Sessions
        {
            get => sessions;
            private set => sessions = value;
        }

        static List<NotificationQueue> notifications = new List<NotificationQueue>();

        public static List<NotificationQueue> Notifications { get => notifications; private set => notifications = value; }

        public Server()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Sessions = new List<Session>();

            NotificationQueue.ReadFromFile();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler((obj, e) => Shutdown());

            this.Start();
        }

        private void Shutdown()
        {
            NotificationQueue.MakeBackUp();
        }
        private void Start()
        {
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 8809));
            ServerSocket.Listen(150);
            Accept();
        }
        private void Accept()
        {
            allDone.Reset();
            ServerSocket.BeginAccept(AcceptCallBack, ServerSocket);
            allDone.WaitOne();
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            var Session = new Session();
            Session.ClientSocket = (ar.AsyncState as Socket).EndAccept(ar);


            sessions.Add(Session);


            Session.ClientSocket.BeginReceive(Session.FileInfo, 0, Session.FileInfo.Length, SocketFlags.None, GetFile, new object[] { Session, default(int), false, new FileInformation() });

            Accept();
        }

        private async void GetFile(IAsyncResult ar)
        {
            var CurrentSessionInfo = ar.AsyncState as object[];

            var CurrentSession = CurrentSessionInfo[0] as Session;
            var ReceivedStages = int.Parse(CurrentSessionInfo[1].ToString());
            var Exist = bool.Parse(CurrentSessionInfo[2].ToString());
            FileInformation information = (CurrentSessionInfo[3] as FileInformation);

            /*int.Parse(CurrentSessionInfo[3].ToString());*/

            int NumberOfBytes = 0;

            try
            {
                SocketError errorCode;
                NumberOfBytes = CurrentSession.ClientSocket.EndReceive(ar, out errorCode);

                if (errorCode != SocketError.Success)
                    NumberOfBytes = 0;
            }
            catch
            {
                sessions.RemoveAt(sessions.FindIndex(s => s.UserID == CurrentSession.UserID));
            }

            if (!Exist)
            {
                var receivedInfo = Encoding.UTF8.GetString(CurrentSession.FileInfo);
                var FileInfo = receivedInfo.Split(',');

                information = new FileInformationBuilder()
                                  .ClientFileID(int.Parse(FileInfo[3]))
                                  .Fullname(FileInfo[0])
                                  .Size(FileInfo[1])
                                  .NumberOfStageToGetFile(int.Parse(FileInfo[2]))
                                  .Build();

                //userID from fileInfo
                CurrentSession.UserID = int.Parse(FileInfo[4]);


                CurrentSession.files.Add(information.ClientFileID, information);


                CurrentSession.ClientSocket.BeginReceive(buffer: CurrentSession.Buffer,
                                                         offset: 0,
                                                         size: CurrentSession.Buffer.Length,
                                                         socketFlags: SocketFlags.None,
                                                         callback: GetFile,
                                                         state: new object[]
                                                                {
                                                                  CurrentSession,
                                                                  ReceivedStages,
                                                                  true,
                                                                  information
                                                                }
                                                         );
            }



            //if (information == null)
            //    CurrentSession.files.TryGetValue(CFileID, out information);


            if (NumberOfBytes > 0)
            {

                if (!Directory.Exists(@"C:\\data"))
                    Directory.CreateDirectory(@"C:\\data");

                var BinaryFile = new BinaryWriter(File.Open(@"C:\\data\" + information.ClientFileID + "." + information.Extension, FileMode.Append));

                BinaryFile.Write(CurrentSession.Buffer);
                BinaryFile.Flush();
                BinaryFile.Close();

                CurrentSession.ClientSocket.BeginReceive(buffer: CurrentSession.Buffer,
                                                         offset: 0,
                                                         size: CurrentSession.Buffer.Length,
                                                         socketFlags: SocketFlags.None,
                                                         callback: GetFile,
                                                         state: new object[]
                                                                { 
                                                                  CurrentSession,
                                                                  ReceivedStages++,
                                                                  true,
                                                                  information
                                                                }
                                                         );
            }

            information.ReceivedStages++;

            if (information.NumberOfStagesTogetIt < information.ReceivedStages || NumberOfBytes == 0)
            {
                information.Received = true;

                string path = @"C:\\data\" + information.ClientFileID + information.Extension;

                if (FileInformation.IsJson(information.Extension.ToLower()))
                {
                    var requestContent = File.ReadAllText(path);
                    var apiUri = new Uri("https://localhost:44312/");

                    await ExecuteRequestAsync(apiUri, CurrentSession, information.Name, requestContent, information);
                }
                
            }
        }
        private async Task ExecuteRequestAsync(Uri apiUri, Session CurrentSession, string request, string requestContent, FileInformation information)
        {
            switch (request)
            {
                //activate user account
                case "ACMA":
                    {
                        Request.ActivateAccount(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //add new friend 
                case "ADFC":
                    {
                        Request.AddFriend(CurrentSession.ClientSocket, requestContent, information);

                        break;
                    }

                //add new user to admins list
                case "ADGA":
                    {
                        Request.AddAdmin(CurrentSession.ClientSocket, requestContent, information);

                        break;
                    }

                //remove user from admins list
                case "REGA":
                    {

                        Request.RemoveAdmin(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //add new user to group
                case "ADUG":
                    {

                        Request.AddGroupMember(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //remove user from members list
                case "REAU":
                    {
                        Request.RemoveGroupMember(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //block user 
                case "BLAU":
                    {
                        Request.BlockUser(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //remove block
                case "REBL":
                    {
                        Request.UnBlockUser(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //creat a new account
                case "CRAU":
                    {
                        Request.CreateAnAccount(CurrentSession, requestContent);
                        break;
                    }

                //create a new conversation
                case "CRAC":
                    {
                        Request.CreateAConversation(CurrentSession.ClientSocket, requestContent, information);

                        break;
                    }

                //create a new group
                case "CRAG":
                    {
                        Request.CreateAGroup(CurrentSession.ClientSocket, requestContent, information);

                        break;
                    }

                //delete a message from group messages list
                case "DEAM":
                    {
                        Request.DeleteMessage(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //delete a group
                case "DEAG":
                    {
                        Request.DeleteGroup(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //remove user from contact list
                case "DEFC":
                    {
                        Request.DeleteFriend(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //delete my account
                case "DEAU":
                    {
                        Request.DestoryMyAccount(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //edit group name
                case "EDGN":
                    {
                        Request.EditGroupName(CurrentSession.ClientSocket, requestContent, information);

                        break;
                    }

                //edit group picture
                case "EDGP":
                    {
                        Request.EditGroupPicture(CurrentSession, requestContent, information);
                        break;
                    }

                //edit user bio
                case "EDUB":
                    {
                        Request.EditBio(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //edit user email
                case "EDUE":
                    {
                        Request.EditEmail(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //edit group description
                case "EDGD":
                    {
                        Request.EditGroupDescription(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //edit user name
                case "EDUN":
                    {
                        Request.EditNickname(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //edit user password 
                case "EDUP":
                    {
                        Request.EditPassword(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //edit user Profile Picture
                case "EDPP":
                    {
                        Request.EditPicture(CurrentSession, requestContent, information);
                        break;
                    }

                //edit user status
                //it must be the first request came to the server 
                //it change status of request sender
                case "EDUS":
                    {
                        Request.EditStatus(CurrentSession, requestContent, information);
                        break;
                    }

                //send a message
                case "SEME":
                    {
                        Request.SendMessage(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get contacts list
                case "GECL":
                    {
                        Request.GetContacts(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get blocks list
                case "GEBL":
                    {
                        Request.GetBlockedAccounts(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get user picture
                case "GEUP":
                    {
                        Request.GetUserPicture(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get user status
                case "GEUS":
                    {
                        Request.GetUserStatus(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get user bio
                case "GEUB":
                    {
                        Request.GetUserBio(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get activate code 
                case "GEAC":
                    {
                        Request.GetActivateCode(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get my groups
                case "GEMG":
                    {
                        Request.GetUserGroupList(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get my new messages
                case "GENM":
                    {
                        Request.GetNewMessages(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get file ID
                case "GEFI":
                    {
                        Request.GetFileID(CurrentSession);
                        break;
                    }

                //get group information
                case "GEGI":
                    {
                        Request.GetGroupInfo(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get message by id
                case "GEME":
                    {
                        Request.GetMessage(CurrentSession.ClientSocket, requestContent);
                        break;
                    }

                //load more messages
                case "LOMM":
                    {
                        Request.LoadMoreMessages(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get searching list
                case "GESL":
                    {
                        Request.GetSearchingList(CurrentSession.ClientSocket, requestContent, information);
                        break;
                    }

                //get user info
                case "GEUI":
                    {
                        Request.GetUserInfo(CurrentSession, requestContent, information);
                        break;
                    }

                //get media
                case "GEMA":
                    {
                        Request.GetMedia(CurrentSession, requestContent, information);
                        break;
                    }
            }
        }

        public static async Task SendToAllOnlineSubscribers(IEnumerable<int> subscribers, string FilePath)
        {
            Parallel.ForEach(subscribers, subscriber =>
            {
                sessions.Find(s => s.UserID == subscriber).ClientSocket.SendAnyTypeOfFilesAsync(FilePath, subscriber);
            });
        }

        public static async Task<Socket> GetUserSocket(int userID) => await Task.Run(() => sessions.Find(s => s.UserID == userID).ClientSocket);

        public static async Task<Session> GetUserSession(int userID) => await Task.Run(() => sessions.Find(s => s.UserID == userID));

    }

    enum clientStatus
    {
        Online,
        Offline
    }

    class MyClass
    {
        static void Main(string[] args)
        {
            Server server = new Server();
        }
    }
}
