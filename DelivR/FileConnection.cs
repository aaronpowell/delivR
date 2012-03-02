using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using SignalR;
using SignalR.Hubs;

namespace DelivR
{
    public abstract class FileConnection : PersistentConnection
    {
        protected void SendImage(string filePath)
        {
            var file = EncodeImage(filePath);
            Send(new {
                type = "receive",
                data = new {
                    mimeType = "image/" + file.type,
                    file.content
                }
            });
        }

        protected void SendImage(string id, string filePath)
        {
            var file = EncodeImage(filePath);

            Send(id, new
            {
                type = "receive",
                data = new
                {
                    mimeType = "image/" + file.type,
                    file.content
                }
            });
        }

        protected void SendFile(string mimeType, string data)
        {
            Send(new
            {
                type = "receive",
                data = new
                {
                    mimeType = mimeType,
                    content = data
                }
            });
        }

        private static dynamic EncodeImage(string filePath)
        {
            var fileString = string.Empty;
            var file = new FileInfo(filePath);

            using (var fs = file.OpenRead())
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fileString = Convert.ToBase64String(bytes);
            }

            return new
            {
                type = file.Extension.Remove(0, 1).ToLower(),
                content = fileString
            };
        }
    }
}