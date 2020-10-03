using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetra_API.Models;

namespace Tetra_Server
{
    public class FileInformationBuilder
    {
        FileInformation file;
        public FileInformationBuilder()
        {
            file = new FileInformation();
        }
        public FileInformationBuilder ClientFileID(int fileID)
        {
            file.ClientFileID = fileID;
            return this;
        }
        public FileInformationBuilder Fullname(string fullname)
        {
            file.FullName = fullname;
            var info = file.FullName.Split('.');
            file.Name = info[0];
            file.Extension = info[1];
            return this;
        }

        public FileInformationBuilder Size(string size)
        {
            file.Size = size;
            return this;
        }

        public FileInformationBuilder AddReceivers(List<int> subscribers)
        {
            file.ReceiversList = subscribers;
            return this;
        }

        public FileInformationBuilder AddReceiver(int subscriber)
        {
            file.ReceiversList.Append(subscriber);
            return this;
        }

        public FileInformationBuilder NumberOfStageToGetFile(int n)
        {
            file.NumberOfStagesTogetIt = n;
            return this;
        }

        public FileInformation Build() => file;
    }
}
