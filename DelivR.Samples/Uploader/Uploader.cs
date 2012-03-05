using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DelivR.Samples.Uploader
{
    public class Uploader : FileConnection
    {
        public Uploader()
        {
            DefaultFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}