using System;
using System.Collections.Generic;
using System.Text;

namespace Clicksign.Client
{
    public class EventName
    {
        public const string Upload = "Upload";
        public const string AddSigner = "add_signer";
        public const string RemoveSigner = "remove_signer";
        public const string Sign = "sign";
        public const string Close = "close";
        public const string AutoClose = "auto_close";
        public const string Deadline = "deadline";
        public const string Cancel = "cancel";
        public const string UpdateDeadline = "update_deadline";
        public const string UpdateAutoClose = "update_auto_close";
    }
}
