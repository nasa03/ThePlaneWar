// Copyright © 2018 Procedural Worlds Pty Limited.  All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCommon1
{
    public class PWNews// : RemoteContent
    {
        public string Title { get; private set; }
        public string Url { get; private set; }
        public string Body { get; private set; }

        public PWNews(string sourceUrl)// : base(sourceUrl)
        {
            Title = "Awesome news!";
            Url = "http://procedural-worlds.com";
            Body = "This is the body of the news. It will contain some text and possibly an image.";
        }
    }
}
