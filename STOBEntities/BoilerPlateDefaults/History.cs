using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOBEntities.BoilerPlateDefaults
{
    [Serializable]
    public class History
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public string ActionText { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
