using System;

namespace BladderChange.Web.Data.Model.Entities
{
    public class BladderChangeInfo
    {
        public BladderChangeInfo()
        {          
        }

        public string MachineNo { get; set; }
        public string Size { get; set; }
        public string BladderNameLeft { get; set; }
        public int BladderLimitLeft { get; set; }
        public int BladderCountLeft { get; set; }
        public int LastChangeLeft { get; set; }
        public string BladderNameRight { get; set; }
        public int BladderLimitRight { get; set; }
        public int BladderCountRight { get; set; }
        public int LastChangeRight { get; set; }
        public bool Status { get; set;}
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }       
        
        public override string ToString()
        {
            return MachineNo;
        }
    }
}
