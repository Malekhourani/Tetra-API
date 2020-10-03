using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tetra_Server
{
    public class FileInformation
    {
        private static int fileID;
        public static int FileID
        {
            get
            {
                lock ("")
                {
                    return fileID++;
                }
            }
        }

        public int ClientFileID { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
        public long NumberOfStagesTogetIt { get; set; }
        public int ReceivedStages { get; set; } = 0;
        public bool Received { get; set; } = false;
        public string Name { get; set; }
        public List<int> ReceiversList { get; set; } = new List<int>();

        static FileInformation()
        {
            var apiUri = new Uri("https://localhost:44312/");

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUri;

                var response = client.GetAsync($"api/media").Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;

                    var DeserlizedData = JsonConvert.DeserializeObject(json);

                    if (!int.TryParse(DeserlizedData.ToString(), out fileID)) fileID = default(int);

                }
            }
        }

        public static bool IsJson(string extension) => extension.ToLower() == "json" ? true : false;

        public static Task WriteDataToBinaryFileAsync(string path, string jsonResponse)
        {

            var bw = new BinaryWriter(File.OpenWrite(path));

            return Task.Run(() =>
            {
                bw.Write(jsonResponse);
                bw.Flush();
                bw.Close();
            }
                               );
        }


        public static Task WriteDataToBinaryFileAsync(string path, byte[] data)
        {
            return Task.Run(() => File.WriteAllBytes(path, data));
        }

    }
}
