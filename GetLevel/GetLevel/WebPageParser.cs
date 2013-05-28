using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

using HtmlAgilityPack;
using Microsoft.Xna.Framework.Content;


namespace GetLevel {
    class WebPageParser {
        HtmlWeb browser;
        WebClient client = new WebClient();
        public HtmlDocument document;
        public string url;


        public static string fallBackPage = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>The Webpage Dungeon</title><style type=\"text/css\">body {background-color:#EEEEEE; color:#222222; }</style></head><body> <p></p><p></p><p></p><p></p><p></p><p></p><p><p></p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p><p></p> <a href=\"http://reddit.com/\">Reddit</a> <a href=\"http://google.com/\">Google</a><a href=\"http://youtube.com/\">Youtube</a></body></html>";

        public static WebPageParser FromUrl(string url) {
            if (url == "") { return WebPageParser.FromString(fallBackPage);  }
            else {
                WebPageParser foo = new WebPageParser();
                if (url.IndexOf("http://") < 0 && url.IndexOf("https://") < 0 || (url.IndexOf(".") < 0) || (url.IndexOf("///") > 0)) {
                    url = "http://" + url;
                }
                if (foo.browser == null) { foo.browser = new HtmlWeb(); }
                if (foo.client == null) { foo.client = new WebClient(); }
                try {
                    try { foo.document = foo.browser.Load(url); }
                    catch (UriFormatException e) { WebPageParser.FromString(fallBackPage); }
                foo.url = url;}
                catch (Exception e) { foo = WebPageParser.FromString(fallBackPage); }
                
                return foo;
            }
        }
        public static WebPageParser FromString(string pagecontent) {
            WebPageParser foo = new WebPageParser();
            if (foo.browser == null) { foo.browser = new HtmlWeb(); }
            if (foo.client == null) { foo.client = new WebClient(); }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pagecontent);
            foo.document = doc;
            return foo;
        }
        public WebPageParser() { }

        public List<String> GetAllLinks() {
            List<String> list = new List<String>();
            foreach (var node in this.document.DocumentNode.DescendantsAndSelf("a")) {
                list.Add(node.GetAttributeValue("href", ""));
            }
            return list;
        }
        public List<HtmlNode> GetAllImages() {
            List<HtmlNode> list = new List<HtmlNode>();
            foreach (var node in this.document.DocumentNode.DescendantsAndSelf("img")) {
                list.Add(node);
            }
            return list;
        }
        public List<int> GetAllLists() {
            List<int> list = new List<int>();
            foreach (var node in this.document.DocumentNode.Descendants("ul")) {
                int total = 0;
                foreach (var node2 in node.ChildNodes) {
                    if (node2.Name == "li") { total++; }
                }
                if (total > 0) { list.Add(total); }
            }
            foreach (var node in this.document.DocumentNode.Descendants("ol")) {
                int total = 0;
                foreach (var node2 in node.ChildNodes) {
                    if (node2.Name == "li") { total++; }
                }
                if (total > 0) { list.Add(total); }
            }
            foreach (var node in this.document.DocumentNode.Descendants("dl")) {
                int total = 0;
                foreach (var node2 in node.ChildNodes) {
                    if (node2.Name == "dt") { total++; }
                }
                if (total > 0) { list.Add(total); }
            }
            return list;
        }
        public int GetTotalWebsiteElements() {
            return this.document.DocumentNode.DescendantsAndSelf().Count();
        }
        public string GetWebpageTitle() {
            return this.document.DocumentNode.Descendants("title").ElementAt(0).InnerText;
        }
        public List<String> GetWebpageColors() { 
            List<String> Colors = new List<String>();
            foreach (var node in document.DocumentNode.Descendants("style")) {
                String css = node.InnerText;
                Regex cssReg = new Regex("#(?:[0-9a-fA-F]{3}){1,2}");
                int index = 0;
                do {
                    Match match = cssReg.Match(css, index);
                    index = match.Index;
                    if (match.Success) {
                        Colors.Add(match.Value);
                        index++;
                    }
                    else { index = -1; }
                } while (index != -1 && index < css.Length);
            }
            return Colors;
        }
        public List<String> GetWebpageColors(string pagecontent) {
            List<String> Colors = new List<String>();
            String css = pagecontent;
            Regex cssReg = new Regex("#(?:[0-9a-fA-F]{3}){1,2}");
            int index = 0;
            do {
                Match match = cssReg.Match(css, index);
                index = match.Index;
                if (match.Success) {
                    Colors.Add(match.Value);
                    index++;
                }
                else { index = -1; }
            } while (index != -1 && index < css.Length);
            return Colors;
        }

        public List<String> GetAllWebpageColors() {
            List<String> AllWebpageColors = new List<String>();
            AllWebpageColors.AddRange(GetWebpageColors());
            foreach (string css in GetWebpageCssLocations()) {
                AllWebpageColors.AddRange(GetWebpageColors(GetCss(css)));
            }
            return AllWebpageColors;
        }

        public List<string> GetWebpageCssLocations() {
            List<string> cssLocations = new List<string>();
            foreach (var node in this.document.DocumentNode.Descendants("link")) {
                string cssPath = "";
                if (node.GetAttributeValue("rel", "").ToLower() == "stylesheet") {
                    string href = node.GetAttributeValue("href", "");
                    cssPath = (href.IndexOf("http://") == -1 && href.IndexOf("https://") == -1 ? GetRootUrl() : "") + href;
                    cssLocations.Add(cssPath);
                }
            }
            return cssLocations;
        }

        public string GetRootUrl() {
            String rooturl = this.url;
            if (rooturl[rooturl.Count() - 1] == '/') { rooturl = rooturl.Substring(0, rooturl.Count() - 1); }
            return rooturl;
        }

        public string GetCss(string cssLocation) {
            string downloadString = client.DownloadString(cssLocation);
            return downloadString;
        }

        public override string ToString() {
            String output = "";
            foreach (var node in this.document.DocumentNode.DescendantsAndSelf()) {

                output += node.Name;
                output += " ";
                output += node.InnerText;
                output += "\n";
            }
            return output;
        }
    }
}
