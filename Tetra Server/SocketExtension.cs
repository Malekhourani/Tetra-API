using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tetra_Server
{
    static class SocketExtensions
    {
        public static async Task SendAnyTypeOfFilesAsync(this Socket socket, string Path, int receiver)
        {
            if (!File.Exists(Path)) throw new Exception("file not found");
            var FileInfo = new FileInfo(Path);

            var FileInByteArray = File.ReadAllBytes(Path);

            var NumberOfStageToSendAFile = FileInByteArray.LongLength / 1024;
            if(NumberOfStageToSendAFile == 0) NumberOfStageToSendAFile = 1;
            var SendedSegment = 0;

            var Info = $"{FileInfo.Name},{FileInfo.Length},{NumberOfStageToSendAFile}";
            var FileInfoInByteArray = Encoding.UTF8.GetBytes(Info);

            try
            {
                await Task.Run(() => socket.Send(FileInfoInByteArray));

                while (SendedSegment != NumberOfStageToSendAFile)
                {
                    int offset = 1024 * SendedSegment;
                    var SegmentSize = (SendedSegment == NumberOfStageToSendAFile - 1) ? FileInByteArray.LongLength - offset : 1024;

                    await Task.Run(() => socket.Send(FileInByteArray, offset, int.Parse(SegmentSize.ToString()), SocketFlags.None));
                    
                    SendedSegment++;
                }

                if (FileInformation.IsJson(FileInfo.Extension.Substring(1)))
                    File.Delete(Path);
            }
            catch
            {
                Server.Notifications.Find(u => u.clientID == receiver).files.Enqueue(Path);
            }


        }
        public static async Task SendAnyTypeOfFilesAsync(this Socket socket, FileInformation information, byte[] file, int receiver)
        {
            information.Size = file.LongLength.ToString();
            information.NumberOfStagesTogetIt = file.LongLength / 1024;

            var Info = $"{information.FullName},{information.Size},{information.NumberOfStagesTogetIt}";
            var FileInfoInByteArray = Encoding.UTF8.GetBytes(Info);

            try
            {
                await Task.Run(() => socket.Send(FileInfoInByteArray));

                while (information.ReceivedStages != information.NumberOfStagesTogetIt)
                {
                    int offset = 1024 * information.ReceivedStages;

                    var SegmentSize = (information.ReceivedStages == information.NumberOfStagesTogetIt - 1) ? file.LongLength - offset : 1024;

                    await Task.Run(() => socket.Send(file, offset, int.Parse(SegmentSize.ToString()), SocketFlags.None));

                    information.ReceivedStages++;
                }

            }
            catch
            {
                string path = $@"C://data/{information.ClientFileID}-{information.Name}.{information.Extension}";
                await FileInformation.WriteDataToBinaryFileAsync(path, file);

                Server.Notifications.Find(u => u.clientID == receiver).files.Enqueue(path);
            }
        }

        public static async Task SendAnyTypeOfFilesAsync(this Socket socket, FileInformation information, byte[] file)
        {
            information.Size = file.LongLength.ToString();
            information.NumberOfStagesTogetIt = file.LongLength / 1024;

            var Info = $"{information.FullName},{information.Size},{information.NumberOfStagesTogetIt}";
            var FileInfoInByteArray = Encoding.UTF8.GetBytes(Info);

            try
            {
                await Task.Run(() => socket.Send(FileInfoInByteArray));

                while (information.ReceivedStages != information.NumberOfStagesTogetIt)
                {
                    int offset = 1024 * information.ReceivedStages;

                    var SegmentSize = (information.ReceivedStages == information.NumberOfStagesTogetIt - 1) ? file.LongLength - offset : 1024;

                    await Task.Run(() => socket.Send(file, offset, int.Parse(SegmentSize.ToString()), SocketFlags.None));

                    information.ReceivedStages++;
                }

            }catch {
                //i don't know what should i do here xDDDDDDD
                //because we will use this method just for our web site 
                //so meh :p
            }
        }

        public static async Task SendStatusMessage(this Socket socket, string requestName, RequestState requestState, string stateMessage = "meh")
        {
            var responseState = new
            {
                request = requestName,
                state = requestState.ToString(),
                message = stateMessage
            };

            var jsonObject = JsonConvert.SerializeObject(responseState);

            var data = Encoding.UTF8.GetBytes(jsonObject);

            await socket.SendAnyTypeOfFilesAsync(
            information: new FileInformation()
            {
                FullName = "stateMessage",
                Extension = "json"
            },
            file: data);

        }
    }
}
