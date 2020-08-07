using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Services.SMS.MH
{
    public class MhContent
    {
        public string op;

        public string uname;

        public string pass;

        public string message;

        public string from;

        public string to;

        public MhContent(string op, string uname, string pass, string message, string from, string to)
        {
            this.op = op;
            this.uname = uname;
            this.pass = pass;
            this.message = message;
            this.from = from;
            this.to = to;
        }
    }
}
