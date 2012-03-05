using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using SignalR;
using SignalR.Hubs;

namespace DelivR
{
    public abstract class FileConnection : PersistentConnection
    {
        private static readonly Regex imageFilter = new Regex(@"^(image\/gif|image\/jpeg|image\/png|image\/svg\+xml|image\/tiff)");

        public string DefaultFilePath { get; set; }

        protected void SendFile(string filePath)
        {
            var file = EncodeFile(filePath);
            Send(new
            {
                type = "receive",
                data = new
                {
                    mimeType = file.type,
                    file.content
                }
            });
        }

        protected void SendFile(string id, string filePath)
        {
            var file = EncodeFile(filePath);

            Send(id, new
            {
                type = "receive",
                data = new
                {
                    mimeType = file.type,
                    file.content
                }
            });
        }

        protected void SendRawFile(string mimeType, string data)
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

        protected void SendRawFile(string connectionId, string mimeType, string data)
        {
            Send(connectionId, new
            {
                type = "receive",
                data = new
                {
                    mimeType = mimeType,
                    content = data
                }
            });
        }

        protected override Task OnReceivedAsync(string connectionId, string data)
        {
            dynamic deserialized = _jsonSerializer.Parse(data);

            if (deserialized.type == "saveFile")
            {
                var bytes = Convert.FromBase64String((string)deserialized.content);
                using (var ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    var mimeType = (string)deserialized.mimeType;
                    if (imageFilter.IsMatch(mimeType))
                    {
                        ImageFormat format;
                        switch (mimeType.Split('/')[1].ToLower())
                        {
                            case "jpeg":
                                format = ImageFormat.Jpeg;
                                break;
                            case "gif":
                                format = ImageFormat.Gif;
                                break;
                            case "tiff":
                                format = ImageFormat.Tiff;
                                break;
                            case "svg+xml":
                            case "png":
                            default:
                                format = ImageFormat.Png;
                                break;
                        }
                        var image = Image.FromStream(ms, true);
                        image.Save(Path.Combine(DefaultFilePath, (string)deserialized.name), ImageFormat.Png);
                    }
                    else
                    {
                        using (var fs = File.Create(Path.Combine(DefaultFilePath, (string)deserialized.name)))
                        {
                            ms.WriteTo(fs);
                        }
                    }

                    Send(new {
                        type = "uploaded"
                    });

                    Connection.Broadcast(new
                    {
                        type = "newFile",
                        name = (string)deserialized.name
                    });
                }
            }
            else if (deserialized.type == "getFile")
            {
                SendFile(connectionId, Path.Combine(DefaultFilePath, (string)deserialized.name));
            }
            else
            {
                OnCustomReceivedAsync(connectionId, data);
            }

            return base.OnReceivedAsync(connectionId, data);
        }

        protected virtual void OnCustomReceivedAsync(string connectionId, string data)
        {

        }

        private static dynamic EncodeFile(string filePath)
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
                type = MimeTypeLookup.GetMimeType(file.Extension.Remove(0, 1).ToLower()),
                content = fileString
            };
        }
    }
}