using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpotMapProject.Models;
namespace SpotMapProject.Models
{
    public class DetailsViewModel
    {
        public class MyViewModel
        {
            public AspNetSpot SpotData = new AspNetSpot();
            public List<string> SpotComments = new List<string>();
            public List<string> Usernames = new List<string>();
            public List<DateTime?> Date = new List<DateTime?>();
            public string Rating = "";
            public bool editor;
            public List<string> photos = new List<string>();
            public List<int> photos_id = new List<int>();
            public bool favorite;
        }
    }
}