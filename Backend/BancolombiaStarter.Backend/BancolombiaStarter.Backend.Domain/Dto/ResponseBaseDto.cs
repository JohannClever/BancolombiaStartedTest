using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Domain.Dto
{
    public class ResponseBaseDto
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
