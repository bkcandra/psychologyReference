using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferencingSystem.Application.Main.MVC.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string token { get; set; }

        public string PayerId { get; set; }

        public string shippingAddress { get; set; }

        public string amount { get; set;}
    }

   

}