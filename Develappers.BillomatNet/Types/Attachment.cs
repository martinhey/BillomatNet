﻿namespace Develappers.BillomatNet.Types
{
    public class Attachment
    {
        public string Filename { get; set; }
        public string Mimetype { get; set; }
        public byte[] Base64File { get; set; }
    }
}