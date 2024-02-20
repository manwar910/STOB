using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOBEntities.Models
{
    public class DashBoardDTO
    {
        public Int32 TotalDraft { get; set; }
        public Int32 ToTalSubmitted { get; set; }
        public Int32 ToTalReviewSent { get; set; }
        public Int32 ToTalModified { get; set; }

        public Int32 ToTalApproved { get; set; }
        public int TotalCounts { get; set; }

        public Int32 ToTalRejected { get; set; }
    }
}
