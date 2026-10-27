using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class MDFileUpload
    {
       
            [System.ComponentModel.DataAnnotations.Key]
            public int FileID { get; set; }
            public string FileName { get; set; }

            public string FilePath { get; set; }

            public string RBSAdminPassword { get; set; }
            public bool IsUpdated { get; set; }
            public DateTime LastUpdateDate { get; set; }
            public string LastUpdateTime { get; set; }
            public bool IsCurrentMenuFile { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional


    }
}
