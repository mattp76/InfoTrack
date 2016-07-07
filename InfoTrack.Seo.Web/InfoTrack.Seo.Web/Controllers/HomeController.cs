﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InfoTrack.Seo.Web.Models;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using HtmlAgilityPack;


namespace InfoTrack.Seo.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            SearchModel model = new SearchModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult Search(SearchModel searchModel)
        {
            if (ModelState.IsValid)
            {
                searchModel.Positions = GetPosition(searchModel.Url, searchModel.Term);
                
                return View("Index", searchModel);
            }

            return View("Index", searchModel);

        }



        public static List<int> GetPosition(string url, string searchTerm)
        {
            string raw = "http://www.google.com/search?q={0}";
            string search = string.Format(raw, HttpUtility.UrlEncode(searchTerm));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    string html = reader.ReadToEnd();
                    return FindPosition(html, url);
                }
            }
        }


        private static List<int> FindPosition(string html, string url)
        {

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//h3/a[@href]");
            int counter = -1;
            List<int> positions = new List<int>();

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    counter++;

                    foreach (var attribute in node.Attributes)
                    {
                        if (attribute.Name == "href" && attribute.Value.Contains(url))
                        {
                            positions.Add(counter);

                            return positions;
                            
                        }      
                    }
                }
            }

            return null;
        }



    }
}
