using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tetra_API.Models;

namespace Tetra_Server
{
    enum RequestState
    {
        Succeeded,
        Failure,
    }

    class Request
    {
        static Uri apiUri = new Uri("https://localhost:44312/");
        public static async void ActivateAccount(Socket clientSocket, string requestContent, FileInformation information)
        {
            var acma = new
            {
                fileID = default(int),
                userID = default(int),
                code = string.Empty
            };

            var data = JsonConvert.DeserializeAnonymousType(requestContent, acma);

            information.ClientFileID = data.fileID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PostAsync($"api/users/{data.userID}/code/{data.code}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                    await clientSocket.SendStatusMessage("ACMA", RequestState.Succeeded);

                await clientSocket.SendStatusMessage("ACMA", RequestState.Failure);
            }
        }
        public static async void AddFriend(Socket clientSocket, string requestContent, FileInformation information)
        {
            var adfc = new
            {
                fileID = default(int),
                userID = default(int),
                friendID = default(int)
            };

            var adfcObject = JsonConvert.DeserializeAnonymousType(requestContent, adfc);
            information.ClientFileID = adfcObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.PostAsync($"api/users/{adfcObject.userID}/contacts/add/{adfcObject.friendID}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    information.ReceiversList.Append(adfcObject.friendID);

                    var adfcResponse = new
                    {
                        fileID = FileInformation.FileID,
                        userID = adfcObject.friendID,
                        friendID = adfcObject.userID
                    };

                    var jsonResponse = JsonConvert.SerializeObject(adfcResponse);

                    var path = @"C:\\data\" + adfcResponse.fileID + "-adfc.json";

                    await FileInformation.WriteDataToBinaryFileAsync(
                                           path: path,
                                           jsonResponse: jsonResponse
                                           );


                    Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    //await friendSocket.SendAnyTypeOfFilesAsync(path, friendID);

                    await clientSocket.SendStatusMessage("ADFC", RequestState.Succeeded);

                }
                await clientSocket.SendStatusMessage("ADFC", RequestState.Failure);
            }
        }
        public static async void AddAdmin(Socket clientSocket, string requestContent, FileInformation information)
        {
            var adga = new
            {
                fileID = default(int),
                requesterID = default(int),
                newAdminID = default(int),
                groupID = default(int),
                subscribers = new List<int>()
            };

            var adgaObject = JsonConvert.DeserializeAnonymousType(requestContent, adga);

            information.ReceiversList = adgaObject.subscribers;
            information.ClientFileID = adgaObject.fileID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PostAsync($"api/groups/{adgaObject.groupID}/admin/{adgaObject.newAdminID}/owner/{adgaObject.requesterID}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {

                    var adgaResponse = new
                    {
                        fileID = FileInformation.FileID,
                        requesterID = adgaObject.requesterID,
                        newAdminID = adgaObject.newAdminID,
                        groupID = adgaObject.groupID,
                    };

                    var jsonResponse = JsonConvert.SerializeObject(adgaResponse);

                    var path = @"C:\\data\" + adgaResponse.fileID + "-adga.json";

                    await FileInformation.WriteDataToBinaryFileAsync(
                                           path: path,
                                           jsonResponse: jsonResponse
                                           );

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("ADGA", RequestState.Succeeded);

                }
                await clientSocket.SendStatusMessage("ADGA", RequestState.Failure);
            }
        }
        public static async void RemoveAdmin(Socket clientSocket, string requestContent, FileInformation information)
        {
            var rega = new
            {
                fileID = default(int),
                groupID = default(int),
                requesterID = default(int),
                userID = default(int),
                subscribers = new List<int>()
            };

            var regaObject = JsonConvert.DeserializeAnonymousType(requestContent, rega);

            information.ReceiversList = regaObject.subscribers;
            information.ClientFileID = regaObject.fileID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.DeleteAsync($"api/groups/{regaObject.groupID}/admin/{regaObject.userID}/owner/{regaObject.requesterID}");

                if (response.IsSuccessStatusCode)
                {
                    var regaResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = regaObject.groupID,
                        ownerID = regaObject.requesterID,
                        userID = rega.userID
                    };

                    var jsonResponse = JsonConvert.SerializeObject(regaResponse);

                    var path = @"C:\\data\" + regaResponse.fileID + "-rega.json";

                    await FileInformation.WriteDataToBinaryFileAsync(
                                           path: path,
                                           jsonResponse: jsonResponse
                                           );

                    await Server.SendToAllOnlineSubscribers(regaObject.subscribers, path);

                    await clientSocket.SendStatusMessage("REGA", RequestState.Succeeded);
                }
                await clientSocket.SendStatusMessage("REGA", RequestState.Failure);

            }
        }
        public static async void AddGroupMember(Socket clientSocket, string requestContent, FileInformation information)
        {
            var adug = new
            {
                fileID = default(int),
                requesterID = default(int),
                userID = default(int),
                groupID = default(int),
                subscribers = new List<int>()
            };

            var adugObject = JsonConvert.DeserializeAnonymousType(requestContent, adug);

            information.ReceiversList = adugObject.subscribers;
            information.ClientFileID = adugObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.PostAsync($"api/groups/{adugObject.groupID}/user/{adugObject.requesterID}/add/{adugObject.userID}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var adugResponse = new
                    {
                        fileID = FileInformation.FileID,
                        requesterID = adugObject.requesterID,
                        userID = adugObject.userID,
                        groupID = adugObject.groupID
                    };

                    string path = @"C:\\data\" + adugResponse.fileID + "-adug.json";

                    var jsonResponse = JsonConvert.SerializeObject(adugResponse);

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(adugObject.subscribers, path);

                    await clientSocket.SendStatusMessage("ADUG", RequestState.Succeeded);
                }
                await clientSocket.SendStatusMessage("ADUG", RequestState.Failure);
            }
        }
        public static async void RemoveGroupMember(Socket clientSocket, string requestContent, FileInformation information)
        {
            var reau = new
            {
                fileID = default(int),
                requesterID = default(int),
                groupID = default(int),
                friendID = default(int),
                subscribers = new List<int>()
            };

            var reauObject = JsonConvert.DeserializeAnonymousType(requestContent, reau);


            information.ClientFileID = reauObject.fileID;
            information.ReceiversList.AddRange(reauObject.subscribers);


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.DeleteAsync($"api/groups/{reauObject.groupID}/remove/{reauObject.friendID}/owner/{reauObject.requesterID}");

                if (response.IsSuccessStatusCode)
                {

                    var reauResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = reauObject.groupID,
                        userID = reauObject.requesterID,
                        memberID = reauObject.friendID
                    };

                    string path = @"C:\\data\" + reauResponse.fileID + "-reau.json";

                    var jsonResponse = JsonConvert.SerializeObject(reauResponse);

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("REAU", RequestState.Succeeded);
                }
                await clientSocket.SendStatusMessage("REAU", RequestState.Failure);
            }
        }
        public static async void BlockUser(Socket clientSocket, string requestContent, FileInformation information)
        {
            var blau = new
            {
                fileID = default(int),
                blockerID = default(int),
                blockedID = default(int)
            };

            var blauObject = JsonConvert.DeserializeAnonymousType(requestContent, blau);

            information.ClientFileID = blauObject.fileID;
            information.ReceiversList.Add(blauObject.blockedID);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/users/{blauObject.blockerID}/block/{blauObject.blockedID}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {

                    var blauResponse = new
                    {
                        fileID = FileInformation.FileID,
                        blockerID = blauObject.blockerID,
                        blockedID = blauObject.blockedID
                    };

                    string path = @"C:\\data\" + blauResponse.fileID + "-blau.json";

                    var jsonResponse = JsonConvert.SerializeObject(blauResponse);

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);
                    await clientSocket.SendStatusMessage("BLAU", RequestState.Succeeded);
                }
                await clientSocket.SendStatusMessage("BLAU", RequestState.Failure);
            }
        }
        public static async void UnBlockUser(Socket clientSocket, string requestContent, FileInformation information)
        {
            var rebl = new
            {
                fileID = default(int),
                blockerID = default(int),
                blockedID = default(int)
            };

            var reblObject = JsonConvert.DeserializeAnonymousType(requestContent, rebl);

            information.ReceiversList.Add(reblObject.blockedID);
            information.ClientFileID = reblObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.DeleteAsync($"api/users/{reblObject.blockerID}/unblock/{reblObject.blockedID}");

                if (response.IsSuccessStatusCode)
                {

                    var reblResponse = new
                    {
                        fileID = FileInformation.FileID,
                        blockerID = reblObject.blockerID,
                        blockedID = reblObject.blockedID
                    };

                    string path = @"C:\\data\" + reblResponse.fileID + "-rebl.json";

                    var jsonResponse = JsonConvert.SerializeObject(reblResponse);

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);
                }

                await clientSocket.SendStatusMessage("REBL", RequestState.Succeeded);
            }
            await clientSocket.SendStatusMessage("REBL", RequestState.Failure);
        }
        public static async void CreateAnAccount(Session clientSession, string requestContent)
        {
            var user = JsonConvert.DeserializeObject<User>(requestContent);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PostAsync($"api/users", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<User>(content);
                    clientSession.UserID = userInfo.UserID;

                    string path = @"C:\\data\" + FileInformation.FileID + "-crau.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, content);

                    await clientSession.ClientSocket.SendAnyTypeOfFilesAsync(path, userInfo.UserID);

                    await clientSession.ClientSocket.SendStatusMessage("CRAU", RequestState.Succeeded);

                }
                await clientSession.ClientSocket.SendStatusMessage("CRAU", RequestState.Failure);
            }
        }
        public static async void CreateAConversation(Socket clientSocket, string requestContent, FileInformation information)
        {
            var crac = new
            {
                fileID = default(int),
                requesterID = default(int),
                friendID = default(int)
            };

            var cracObject = JsonConvert.DeserializeAnonymousType(requestContent, crac);

            information.ClientFileID = cracObject.fileID;
            information.ReceiversList.AddRange(new[] { cracObject.friendID, cracObject.requesterID });


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PostAsync($"api/groups/{cracObject.requesterID}/with/{cracObject.friendID}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();

                    var conversationID = JsonConvert.DeserializeObject<int>(content);

                    var cracResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = conversationID,
                        groupType = "CONVERSATION",
                        createTime = DateTime.Now,
                        participants = new List<int>()
                        {
                            cracObject.requesterID,
                            cracObject.friendID
                        }
                    };

                    var jsonResponse = JsonConvert.SerializeObject(cracResponse);

                    string path = @"C:\\data\" + cracResponse.fileID + "-crac.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("CRAC", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("CRAC", RequestState.Failure);

            }
        }
        public static async void CreateAGroup(Socket clientSocket, string requestContent, FileInformation information)
        {
            var crag = new
            {
                fileID = default(int),
                groupName = string.Empty,
                requesterID = default(int),
                subscribers = new List<int>()
            };

            var cragObject = JsonConvert.DeserializeAnonymousType(requestContent, crag);

            information.ClientFileID = cragObject.fileID;
            information.ReceiversList = cragObject.subscribers;
            information.ReceiversList.Add(cragObject.requesterID);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PostAsync($"api/groups/{cragObject.groupName}/owner/{cragObject.requesterID}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var groupID = JsonConvert.DeserializeObject<int>(content);

                    var cragResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = groupID,
                        groupType = "GROUP",
                        createTime = DateTime.Now,
                        participants = new List<int>()
                    };

                    cragResponse.participants.AddRange(information.ReceiversList);

                    var jsonResponse = JsonConvert.SerializeObject(cragResponse);

                    string path = @"C:\\data\" + cragResponse.fileID + "-crag.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("CRAG", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("CRAG", RequestState.Failure);

            }
        }
        public static async void DeleteMessage(Socket clientSocket, string requestContent, FileInformation information)
        {
            var deam = new
            {
                fileID = default(int),
                groupID = default(int),
                requesterID = default(int),
                messageID = default(int),
                subscribers = new List<int>()
            };

            var deamObject = JsonConvert.DeserializeAnonymousType(requestContent, deam);

            information.ClientFileID = deamObject.fileID;
            information.ReceiversList = deamObject.subscribers;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.DeleteAsync($"api/messages/{deamObject.messageID}/delete-by/{deamObject.requesterID}");

                if (response.IsSuccessStatusCode)
                {
                    var deamResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = deamObject.groupID,
                        senderID = deamObject.requesterID,
                        messageID = deamObject.messageID
                    };

                    var jsonResponse = JsonConvert.SerializeObject(deamResponse);

                    string path = @"C:\\data\" + deamResponse.fileID + "-deam.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("DEAM", RequestState.Succeeded);
                }
                await clientSocket.SendStatusMessage("DEAM", RequestState.Failure);

            }
        }
        public static async void DeleteGroup(Socket clientSocket, string requestContent, FileInformation information)
        {
            var deag = new
            {
                fileID = default(int),
                groupID = default(int),
                requesterID = default(int),
                subscribers = new List<int>()
            };

            var deagObject = JsonConvert.DeserializeAnonymousType(requestContent, deag);

            information.ClientFileID = deagObject.requesterID;
            information.ReceiversList = deagObject.subscribers;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.DeleteAsync($"api/groups/{deagObject.groupID}/requester/{deagObject.requesterID}");

                if (response.IsSuccessStatusCode)
                {
                    var deagResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = deagObject.groupID,
                        requesterID = deagObject.requesterID
                    };

                    var jsonResponse = JsonConvert.SerializeObject(deagResponse);

                    string path = @"C:\\data\" + deagResponse.fileID + "-deag.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("DEAG", RequestState.Succeeded);
                }
                await clientSocket.SendStatusMessage("DEAG", RequestState.Failure);

            }
        }
        public static async void DeleteFriend(Socket clientSocket, string requestContent, FileInformation information)
        {
            var defc = new
            {
                fileID = default(int),
                requesterID = default(int),
                friendID = default(int)
            };

            var defcObject = JsonConvert.DeserializeAnonymousType(requestContent, defc);

            information.ClientFileID = defcObject.fileID;
            information.ReceiversList.Add(defcObject.friendID);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.DeleteAsync($"api/users/{defcObject.requesterID}/contacts/remove/{defcObject.friendID}");

                if (response.IsSuccessStatusCode)
                {

                    var defcResponse = new
                    {
                        fileID = FileInformation.FileID,
                        userID = defcObject.friendID,
                        friendID = defcObject.requesterID
                    };

                    var path = @"C:\\data\" + defcResponse.fileID + "-defc.json";

                    var jsonResponse = JsonConvert.SerializeObject(defcResponse);

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);


                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);
                    await clientSocket.SendStatusMessage("DEFC", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("DEFC", RequestState.Failure);

            }
        }
        public static async void DestoryMyAccount(Socket clientSocket, string requestContent, FileInformation information)
        {
            var deau = new
            {
                fileID = default(int),
                requesterID = default(int),
                subscribers = new List<int>()
            };

            var deauObject = JsonConvert.DeserializeAnonymousType(requestContent, deau);

            information.ClientFileID = deauObject.fileID;
            information.ReceiversList = deauObject.subscribers;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.DeleteAsync($"api/users/{deauObject.requesterID}");

                if (response.IsSuccessStatusCode)
                {
                    var deauResponse = new
                    {
                        fileID = FileInformation.FileID,
                        userID = deauObject.requesterID
                    };

                    var jsonResponse = JsonConvert.SerializeObject(deauResponse);

                    var path = @"C:\\data\" + deauResponse.fileID + "-deau.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("DEAU", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("DEAU", RequestState.Failure);
            }
        }
        public static async void EditGroupName(Socket clientSocket, string requestContent, FileInformation information)
        {
            var edgn = new
            {
                fileID = default(int),
                groupID = default(int),
                requesterID = default(int),
                newName = string.Empty,
                subscribers = new List<int>()
            };

            var edgnObject = JsonConvert.DeserializeAnonymousType(requestContent, edgn);

            information.ReceiversList = edgnObject.subscribers;
            information.ClientFileID = edgnObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/groups/{edgnObject.groupID}/name/{edgnObject.newName}", new StringContent(requestContent));


                if (response.IsSuccessStatusCode)
                {
                    var edgnResponse = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = edgnObject.groupID,
                        requesterID = edgnObject.requesterID,
                        newName = edgnObject.newName
                    };

                    var jsonResponse = JsonConvert.SerializeObject(edgnResponse);

                    var path = @"C:\\data\" + edgnResponse.fileID + "-edgn.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(edgnObject.subscribers, path);

                    await clientSocket.SendStatusMessage("EDGN", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("EDGN", RequestState.Failure);
            }
        }
        public static async void EditGroupPicture(Session clientSession, string requestContent, FileInformation information)
        {
            var edgp = new
            {
                fileID = default(int),
                requesterID = default(int),
                groupID = default(int),
                fileName = string.Empty,
                mediaID = default(int),
                subscribers = new List<int>()
            };

            var edgpObject = JsonConvert.DeserializeAnonymousType(requestContent, edgp);

            information.ReceiversList = edgpObject.subscribers;
            information.ClientFileID = edgpObject.fileID;

            FileInformation attachmentInformation;


            var exist = clientSession.files.TryGetValue(edgpObject.mediaID, out attachmentInformation);

            if (!exist)
            {
                Medium mediaInfo = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = apiUri;
                    var apiResponse = await client.GetAsync($"api/media/{edgpObject.mediaID}");

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        mediaInfo = JsonConvert.DeserializeObject<Medium>(await apiResponse.Content.ReadAsStringAsync());
                        attachmentInformation.ClientFileID = mediaInfo.MediaID;
                        attachmentInformation.Extension = mediaInfo.MediaName.Split('.')[1];
                    }
                }
            }


            attachmentInformation.ReceiversList = edgpObject.subscribers;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;



                if (attachmentInformation.Received)
                {
                    var filePath = @"C:\\data\" + attachmentInformation.ClientFileID + "." + attachmentInformation.Extension;

                    var response = await client.PutAsync($"api/groups/{edgpObject.groupID}/media/{attachmentInformation.ClientFileID}/path/{filePath}", new StringContent(requestContent));

                    if (response.IsSuccessStatusCode)
                    {
                        var edgpResponse = new
                        {
                            fileID = FileInformation.FileID,
                            requesterID = edgpObject.requesterID,
                            groupID = edgpObject.groupID,
                            fileName = edgpObject.fileName,
                            attachmentFileID = edgpObject.mediaID
                        };

                        var jsonResponse = JsonConvert.SerializeObject(edgpResponse);

                        var path = @"C:\\data\" + edgpResponse.fileID + "-edgp.json";

                        await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                        await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);
                        await Server.SendToAllOnlineSubscribers(attachmentInformation.ReceiversList, filePath);

                        await clientSession.ClientSocket.SendStatusMessage("EDGP", RequestState.Succeeded);
                    }
                    else await clientSession.ClientSocket.SendStatusMessage("EDGP", RequestState.Failure);
                }
            }
        }
        public static async void EditBio(Socket clientSocket, string requestContent, FileInformation information)
        {
            var edub = new
            {
                fileID = default(int),
                requesterID = default(int),
                bio = string.Empty,
                subscribers = new List<int>()
            };

            var edubObject = JsonConvert.DeserializeAnonymousType(requestContent, edub);

            information.ReceiversList = edubObject.subscribers;
            information.ClientFileID = edubObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/users/{edubObject.requesterID}/bio/{edubObject.bio}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var edubResponse = new
                    {
                        fileID = FileInformation.FileID,
                        userID = edubObject.requesterID,
                        bio = edubObject.bio
                    };

                    var jsonResponse = JsonConvert.SerializeObject(edubResponse);

                    string path = @"C:\\data\" + edubResponse.fileID + "-edub.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(edubObject.subscribers, path);

                    await clientSocket.SendStatusMessage("EDUB", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("EDUB", RequestState.Failure);
            }
        }
        public static async void EditEmail(Socket clientSocket, string requestContent, FileInformation information)
        {
            var edue = new
            {
                fileID = default(int),
                requesterID = default(int),
                oldEmail = string.Empty,
                newEmail = string.Empty,
                subscribers = new List<int>()
            };

            var edueObject = JsonConvert.DeserializeAnonymousType(requestContent, edue);

            information.ReceiversList = edueObject.subscribers;
            information.ClientFileID = edueObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/users/{edueObject.requesterID}/email/{edueObject.oldEmail}/{edueObject.newEmail}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var edueResponse = new
                    {
                        fileID = FileInformation.FileID,
                        userID = edueObject.requesterID,
                        oldEmail = edueObject.oldEmail,
                        newEmail = edueObject.newEmail
                    };

                    var jsonResponse = JsonConvert.SerializeObject(edueResponse);

                    string path = @"C:\\data\" + edueResponse.fileID + "-edue.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(edueObject.subscribers, path);

                    await clientSocket.SendStatusMessage("EDUE", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("EDUE", RequestState.Failure);
            }
        }
        public static async void EditGroupDescription(Socket clientSocket, string requestContent, FileInformation information)
        {
            var edgd = new
            {
                fileID = default(int),
                groupID = default(int),
                requesterID = default(int),
                description = string.Empty,
                subscribers = new List<int>()
            };

            var edgdObject = JsonConvert.DeserializeAnonymousType(requestContent, edgd);

            information.ReceiversList = edgdObject.subscribers;
            information.ClientFileID = edgdObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/groups/{edgdObject.groupID}/description/{edgdObject.description}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var edgdObjectToSerialize = new
                    {
                        fileID = FileInformation.FileID,
                        groupID = edgdObject.groupID,
                        description = edgdObject.description
                    };

                    var jsonResponse = JsonConvert.SerializeObject(edgdObjectToSerialize);

                    string path = @"C:\\data\" + edgdObject.fileID + "-edgd.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);

                    await clientSocket.SendStatusMessage("EDGD", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("EDGD", RequestState.Failure);
            }
        }
        public static async void EditNickname(Socket clientSocket, string requestContent, FileInformation information)
        {
            var edun = new
            {
                fileID = default(int),
                requesterID = default(int),
                newName = string.Empty,
                subscribers = new List<int>()
            };

            var edunObject = JsonConvert.DeserializeAnonymousType(requestContent, edun);

            information.ReceiversList = edunObject.subscribers;
            information.ClientFileID = edunObject.fileID;


            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/users/{edunObject.requesterID}/nickname/{edunObject.newName}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var edunResponse = new
                    {
                        fileID = FileInformation.FileID,
                        requesterID = edunObject.requesterID,
                        newName = edunObject.newName
                    };

                    var jsonResponse = JsonConvert.SerializeObject(edunResponse);

                    string path = @"C:\\data\" + edunResponse.fileID + "-edun.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(edunObject.subscribers, path);

                    await clientSocket.SendStatusMessage("EDUN", RequestState.Succeeded);
                }
                else await clientSocket.SendStatusMessage("EDUN", RequestState.Failure);
            }
        }
        public static async void EditPassword(Socket clientSocket, string requestContent, FileInformation information)
        {
            var edup = new
            {
                fileID = default(int),
                requesterID = default(int),
                oldPassword = string.Empty,
                newPassword = string.Empty
            };

            var edupObject = JsonConvert.DeserializeAnonymousType(requestContent, edup);

            information.ClientFileID = edupObject.fileID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/users/{edupObject.requesterID}/password/{edupObject.oldPassword}/{edupObject.newPassword}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                    await clientSocket.SendStatusMessage("EDUP", RequestState.Succeeded);
                else await clientSocket.SendStatusMessage("EDUP", RequestState.Failure);

            }
        }
        public static async void EditStatus(Session clientSession, string requestContent, FileInformation information)
        {
            var edus = new
            {
                fileID = default(int),
                requesterID = default(int),
                status = string.Empty,
                subscribers = new List<int>()
            };

            var edusObject = JsonConvert.DeserializeAnonymousType(requestContent, edus);

            information.ReceiversList = edusObject.subscribers;
            information.ClientFileID = edusObject.fileID;


            clientSession.UserID = edusObject.requesterID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.PutAsync($"api/users/{edusObject.requesterID}/status/{edusObject.status}", new StringContent(requestContent));

                if (response.IsSuccessStatusCode)
                {
                    var edusResponse = new
                    {
                        fileID = FileInformation.FileID,
                        userID = edusObject.requesterID,
                        status = edusObject.status
                    };

                    var jsonResponse = JsonConvert.SerializeObject(edusResponse);

                    string path = @"C:\\data\" + edusResponse.fileID + "-edus.json";

                    await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                    await Server.SendToAllOnlineSubscribers(edusObject.subscribers, path);

                    await clientSession.ClientSocket.SendStatusMessage("EDUS", RequestState.Succeeded);


                    if (edusObject.status.ToUpper() == clientStatus.Offline.ToString().ToUpper())
                    {
                        Server.Sessions.RemoveAt(Server.Sessions.FindIndex((session) => session.UserID == edusObject.requesterID));

                        Server.Notifications.Add(
                            new NotificationQueue()
                            {
                                clientID = clientSession.UserID,
                                files = new Queue<string>()
                            }
                            );

                    }
                    else
                    {
                        //clientSession.UserID = edusObject.requesterID;
                        //Server.NotificationsStack.Add(clientSession.UserID, new Stack<string>());

                        var notificationsInfo = Server.Notifications.Find(u => u.clientID == edusObject.requesterID);

                        NotificationQueue.SendAllNotifications(clientSession.ClientSocket, notificationsInfo.files, notificationsInfo.clientID);

                    }

                }
                else await clientSession.ClientSocket.SendStatusMessage("EDUS", RequestState.Failure);

            }
        }
        public static async void EditPicture(Session clientSession, string requestContent, FileInformation information)
        {
            var edpp = new
            {
                fileID = default(int),
                requesterID = default(int),
                mediaID = default(int),
                subscribers = new List<int>()
            };

            var edppObject = JsonConvert.DeserializeAnonymousType(requestContent, edpp);

            information.ClientFileID = edppObject.fileID;
            information.ReceiversList = edppObject.subscribers;

            FileInformation attachmentInformation;


            var exist = clientSession.files.TryGetValue(edppObject.mediaID, out attachmentInformation);

            if (!exist)
            {
                Medium mediaInfo = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = apiUri;
                    var apiResponse = await client.GetAsync($"api/media/{edppObject.mediaID}");

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        mediaInfo = JsonConvert.DeserializeObject<Medium>(await apiResponse.Content.ReadAsStringAsync());
                        attachmentInformation.ClientFileID = mediaInfo.MediaID;
                        attachmentInformation.Extension = mediaInfo.MediaName.Split('.')[1];
                    }
                }
            }

            attachmentInformation.ReceiversList = edppObject.subscribers;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;



                if (attachmentInformation.Received)
                {
                    var filePath = @"C:\\data\" + attachmentInformation.ClientFileID + "." + attachmentInformation.Extension;

                    var response = await client.PutAsync($"api/users/{edppObject.requesterID}/media/{attachmentInformation.ClientFileID}/path/{filePath}", new StringContent(requestContent));

                    if (response.IsSuccessStatusCode)
                    {
                        var edppResponse = new
                        {
                            fileID = FileInformation.FileID,
                            userID = edppObject.requesterID,
                            mediaID = edpp.mediaID
                        };

                        var jsonResponse = JsonConvert.SerializeObject(edppResponse);

                        var path = @"C:\\data\" + edppResponse.fileID + "-edpp.json";

                        await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                        await Server.SendToAllOnlineSubscribers(information.ReceiversList, path);
                        await Server.SendToAllOnlineSubscribers(attachmentInformation.ReceiversList, filePath);

                        await clientSession.ClientSocket.SendStatusMessage("EDPP", RequestState.Succeeded);
                    }
                    else await clientSession.ClientSocket.SendStatusMessage("EDPP", RequestState.Failure);
                }
            }



        }
        public static async void GetMedia(Session clientSession, string requestContent, FileInformation information)
        {
            var gema = new
            {
                fileID = default(int),
                mediaID = default(int)
            };

            var gemaObject = JsonConvert.DeserializeAnonymousType(requestContent, gema);

            information.ClientFileID = gemaObject.fileID;
            information.ReceiversList.Add(clientSession.UserID);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.GetAsync($"api/media/{gemaObject.mediaID}/path");

                if (response.IsSuccessStatusCode)
                {
                    var path = await response.Content.ReadAsStringAsync();

                    await clientSession.ClientSocket.SendAnyTypeOfFilesAsync(path, clientSession.UserID);
                }
            }

        }
        public static async void SendMessage(Socket clientSocket, string requestContent, FileInformation information)
        {
            var seme = new
            {
                fileID = default(int),
                groupID = default(int),
                senderID = default(int),
                content = string.Empty,
                mediaID = default(int),
                messageToReplayID = default(int),
                subscribers = new List<int>()
            };

            var semeObject = JsonConvert.DeserializeAnonymousType(requestContent, seme);

            information.ReceiversList.AddRange(semeObject.subscribers);
            information.ReceiversList.Add(semeObject.senderID);
            information.ClientFileID = semeObject.fileID;

            FileInformation attachmentInformation;

            var exist = Server.Sessions.Find(s => s.UserID == semeObject.senderID).files.TryGetValue(semeObject.mediaID, out attachmentInformation);

            if (!exist)
            {
                Medium mediaInfo = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = apiUri;
                    var apiResponse = await client.GetAsync($"api/media/{semeObject.mediaID}");

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        mediaInfo = JsonConvert.DeserializeObject<Medium>(await apiResponse.Content.ReadAsStringAsync());

                        attachmentInformation = new FileInformationBuilder()
                                                    .ClientFileID(mediaInfo.MediaID)
                                                    .Fullname(mediaInfo.MediaName)
                                                    .Build();
                    }
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                attachmentInformation.ReceiversList = semeObject.subscribers;


                var filePath = @"C:\\data\" + attachmentInformation.FullName;


                if (attachmentInformation.Received)
                {
                    var response = await client.PostAsync($"api/messages/{semeObject.content}/user/{semeObject.senderID}/group/{semeObject.groupID}/media/{filePath}/message/replay/{semeObject.messageToReplayID}", new StringContent(requestContent));



                    if (response.IsSuccessStatusCode)
                    {


                        var ResponseContent = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

                        var semeResponse = new
                        {
                            fileID = FileInformation.FileID,
                            messageID = ResponseContent,
                            groupID = semeObject.groupID,
                            senderID = semeObject.senderID,
                            content = semeObject.content,
                            mediaID = semeObject.mediaID,
                            messageToReplayID = semeObject.messageToReplayID,
                        };


                        string path = @"C:\\data\" + semeResponse.fileID + "-seme.json";

                        var jsonResponse = JsonConvert.SerializeObject(semeResponse);

                        await FileInformation.WriteDataToBinaryFileAsync(path, jsonResponse);

                        await Server.SendToAllOnlineSubscribers(semeObject.subscribers, path);

                        await clientSocket.SendStatusMessage("SEME", RequestState.Succeeded);
                    }
                    else await clientSocket.SendStatusMessage("SEME", RequestState.Failure);
                }

            }
        }
        public static async void GetContacts(Socket clientSocket, string requestContent, FileInformation information)
        {
            var gecl = new
            {
                fileID = default(int),
                userID = default(int)
            };

            var geclObject = JsonConvert.DeserializeAnonymousType(requestContent, gecl);

            information.ClientFileID = geclObject.fileID;
            information.ReceiversList.Add(geclObject.userID);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.GetAsync($"api/users/{geclObject.userID}/contacts");

                if (response.IsSuccessStatusCode)
                {

                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(geclObject.fileID)
                                       .Fullname("GECL.json")
                                       .AddReceiver(geclObject.userID)
                                       .Build();

                    //this will send response info and attachment id
                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geclObject.userID);
                }
                else await clientSocket.SendStatusMessage("GECL", RequestState.Failure);
            }
        }
        public static async void GetBlockedAccounts(Socket clientSocket, string requestContent, FileInformation information)
        {
            var gebl = new
            {
                fileID = default(int),
                userID = default(int)
            };

            var geblObject = JsonConvert.DeserializeAnonymousType(requestContent, gebl);

            information.ClientFileID = geblObject.fileID;
            information.ReceiversList.Add(geblObject.userID);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.GetAsync($"api/users/{geblObject.userID}/blockedUser");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(geblObject.fileID)
                                       .Fullname("GEBL.json")
                                       .AddReceiver(geblObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geblObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEBL", RequestState.Failure);
            }
        }
        public static async void GetUserPicture(Socket clientSocket, string requestContent, FileInformation information)
        {
            var geup = new
            {
                fileID = default(int),
                userID = default(int)
            };

            information.ClientFileID = geup.fileID;

            var geupObject = JsonConvert.DeserializeAnonymousType(requestContent, geup);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"api/users/{geupObject.userID}/picture");

                if (response.IsSuccessStatusCode)
                {
                    var photoPath = await response.Content.ReadAsStringAsync();

                    await clientSocket.SendAnyTypeOfFilesAsync(photoPath, geupObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEUP", RequestState.Failure);
            }
        }
        public static async void GetUserStatus(Socket clientSocket, string requestContent, FileInformation information)
        {
            var geus = new
            {
                fileID = default(int),
                userID = default(int)
            };

            var geusObject = JsonConvert.DeserializeAnonymousType(requestContent, geus);

            information.ClientFileID = geusObject.fileID;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"api/users/{geusObject.userID}/status");

                if (response.IsSuccessStatusCode)
                {

                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GEUS.json")
                                       .AddReceiver(geusObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geusObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEUS", RequestState.Failure);
            }
        }
        public static async void GetUserBio(Socket clientSocket, string requestContent, FileInformation information)
        {
            var geub = new
            {
                fileID = default(int),
                userID = default(int)
            };

            var geubObject = JsonConvert.DeserializeAnonymousType(requestContent, geub);

            information.ClientFileID = geubObject.fileID;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"api/users/{geubObject.userID}/bio");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GEUB.json")
                                       .AddReceiver(geubObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geubObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEUB", RequestState.Failure);
            }
        }
        public static async void GetActivateCode(Socket clientSocket, string requestContent, FileInformation information)
        {
            var geac = new
            {
                fileID = default(int),
                userID = default(int)
            };

            var geacObject = JsonConvert.DeserializeAnonymousType(requestContent, geac);

            information.ClientFileID = geacObject.fileID;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"api/users/{geacObject.userID}/code");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                   .ClientFileID(FileInformation.FileID)
                   .Fullname("GEAC.json")
                   .AddReceiver(geacObject.userID)
                   .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geacObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEAC", RequestState.Failure);
            }
        }
        public static async void GetUserGroupList(Socket clientSocket, string requestContent, FileInformation information)
        {
            var gegl = new
            {
                fileID = default(int),
                userID = default(int)
            };

            var geglObject = JsonConvert.DeserializeAnonymousType(requestContent, gegl);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.GetAsync($"api/groups/user/{geglObject.userID}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GEGL.json")
                                       .AddReceiver(geglObject.userID)
                                       .Build();

                    //this will send response info and attachment id
                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geglObject.userID);

                }
                else await clientSocket.SendStatusMessage("GEGL", RequestState.Failure);
            }
        }
        public static async void GetGroupInfo(Socket clientSocket, string requestContent, FileInformation information)
        {
            var gegi = new
            {
                fileID = default(int),
                groupID = default(int),
                userID = default(int)
            };

            var gegiObject = JsonConvert.DeserializeAnonymousType(requestContent, gegi);

            information.ClientFileID = gegiObject.fileID;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"api/groups/{gegiObject.groupID}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GEGI.json")
                                       .AddReceiver(gegiObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, content, gegiObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEGI", RequestState.Failure);
            }
        }
        public static async void GetMessage(Socket clientSocket, string requestContent)
        {
            var geme = new
            {
                fileID = default(int),
                userID = default(int),
                messageID = default(int)
            };

            var gemeObject = JsonConvert.DeserializeAnonymousType(requestContent, geme);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.GetAsync($"~/api/messages/{gemeObject.messageID}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GEME.json")
                                       .AddReceiver(gemeObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, gemeObject.userID);
                }
                else await clientSocket.SendStatusMessage("GEME", RequestState.Failure);
            }
        }
        public static async void LoadMoreMessages(Socket clientSocket, string requestContent, FileInformation information)
        {
            var lomm = new
            {
                fileID = default(int),
                groupID = default(int),
                userID = default(int),
                lastMessageSendTime = default(DateTime)
            };

            var lommObject = JsonConvert.DeserializeAnonymousType(requestContent, lomm);

            information.ClientFileID = lommObject.fileID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.GetAsync($"api/messages/group/{lommObject.groupID}/date/{lommObject.lastMessageSendTime}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("LOMM.json")
                                       .AddReceiver(lommObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, lommObject.userID);
                }
                else await clientSocket.SendStatusMessage("LOMM", RequestState.Failure);
            }
        }
        public static async void GetNewMessages(Socket clientSocket, string requestContent, FileInformation information)
        {
            var genm = new
            {
                fileID = default(int),
                userID = default(int),
                lastSeen = default(DateTime)
            };

            var genmObject = JsonConvert.DeserializeAnonymousType(requestContent, genm);

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;
                var response = await client.GetAsync($"api/messages/user/{genmObject.userID}/date/{genmObject.lastSeen}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GENM.json")
                                       .AddReceiver(genmObject.userID)
                                       .Build();

                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, genmObject.userID);
                }
                else await clientSocket.SendStatusMessage("GENM", RequestState.Failure);
            }
        }
        public static async void GetFileID(Session clientSession)
        {
            var fileID = JsonConvert.SerializeObject(FileInformation.FileID);

            var path = $@"C:\data\{FileInformation.FileID}-gefi.json";

            await FileInformation.WriteDataToBinaryFileAsync(path, fileID);

            await clientSession.ClientSocket.SendAnyTypeOfFilesAsync(path, clientSession.UserID);
        }
        public static async void GetSearchingList(Socket clientSocket, string requestContent, FileInformation information)
        {
            var gesl = new
            {
                fileID = default(int),
                requesterID = default(int),
                fullname = string.Empty
            };

            var geslObject = JsonConvert.DeserializeAnonymousType(requestContent, gesl);

            information.ClientFileID = geslObject.fileID;

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = await client.GetAsync($"api/users/search/{geslObject.fullname}");

                if (response.IsSuccessStatusCode)
                {

                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GESL.json")
                                       .AddReceiver(geslObject.requesterID)
                                       .Build();

                    //this will send response info and attachment id
                    await clientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, geslObject.requesterID);

                }
                else await clientSocket.SendStatusMessage("GESL", RequestState.Failure);
            }
        }
        public static async void SaveMedia(int MediaID, string Name, string Path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                await client.PostAsync($"api/media/{MediaID}/name/{Name}/path/{Path}", null);
            }
        }
        public static async void GetUserInfo(Session clientSession, string requestContent, FileInformation information)
        {
            var geui = new
            {
                fileID = default(int),
                email = string.Empty,
                password = string.Empty
            };

            var geuiObject = JsonConvert.DeserializeAnonymousType(requestContent, geui);

            information.ClientFileID = geui.fileID;

            using (var client = new HttpClient())
            {
                //sending a request to api to get data
                var response = await client.GetAsync($"api/users/email/{geuiObject.email}/password/{geuiObject.password}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsByteArrayAsync();

                    var fileInfo = new FileInformationBuilder()
                                       .ClientFileID(FileInformation.FileID)
                                       .Fullname("GEUI.json")
                                       .AddReceiver(clientSession.UserID)
                                       .Build();

                    await clientSession.ClientSocket.SendAnyTypeOfFilesAsync(fileInfo, apiResponse, clientSession.UserID);
                }
                else
                {
                    await clientSession.ClientSocket.SendStatusMessage("GEUI", RequestState.Failure);
                }
            }
        }
    }
}
