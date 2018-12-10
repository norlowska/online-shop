using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace OnlineShop.Models
{
    public class Product : IEntit
    {
        
        public Product()
        {
            image = new Dictionary<String, String>();
            files = new List<object>();
        }

        [NotMapped]
        public Dictionary<String, String> image { get; set; }

        [Required, MaxLength(120)]
        public String name { get; set; }
        [AllowHtml]
        public String description { get; set; }
        [Required, Range(0.0, (Double)decimal.MaxValue)]
        public decimal price { get; set; }

        public string jsonDictionary { get; set; }
        public string jsonList { get; set; }

        public List<object> files { get; set; }
        public int count { get; set; }

        public virtual Category cat_pro { get; set; }
        //public ICollection<Comment> comments { get; set; }

        public void toJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            jsonDictionary = serializer.Serialize((object)image);
            jsonList = serializer.Serialize((object)files);


        }

        public void toDictionary()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            if (jsonDictionary != null)
            {
                image = serializer.Deserialize<Dictionary<String, String>>(jsonDictionary);
            }

            if(jsonList != null)
            {
                files = serializer.Deserialize<List<object>>(jsonList);
            }

        }
        
    }
}