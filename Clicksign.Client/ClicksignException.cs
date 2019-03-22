using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clicksign.Client
{

    public class ClicksignException : Exception
    {
        public ClicksignException()
        {

        }

        public ClicksignException(string message, int statusCode, IList<String> errors = null) : base(message)
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        public int StatusCode { get; set; }

        public IList<string> Errors { get; set; }
    }
}
