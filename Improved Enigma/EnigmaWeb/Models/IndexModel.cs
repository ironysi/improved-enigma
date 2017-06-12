using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnigmaWeb.Models
{
    public class IndexModel
    {
        public List<UploadedFile> UploadsList { get; set; }


        public IndexModel()
        {
            UploadsList = new List<UploadedFile>();
        }
    }

    public class UploadedFile
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}