using System;
using System.Collections.Generic;

namespace DevinoTest.Model.SmsMessages.Result
{
    public class ResultMessage
    {
        public string code { get; set; }
        public string messageId { get; set; }
        public object segmentsId { get; set; }
        public List<ReasonMessage> reasons { get; set; }
        public string description { get; set; }
    }
}
