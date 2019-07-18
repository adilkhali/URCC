using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedRemote.Core.Models.V1
{
    public class ErrorResponse
    {

        public int Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
