using ReferencingSystem.Application.Main.MVC.Models;
using ReferencingSystem.Core.BF;
using ReferencingSystem.Model;
using ReferencingSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReferencingSystem.Application.Main.MVC.API
{
    public class RSController : ApiController
    {
        private RsContext db = new RsContext();
        private BusinessFunctionComponents bfc = new BusinessFunctionComponents();

        public dynamic GetReference(string Id, string transId)
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(transId))
                return Json(new { Status = 1, Reference = db.Reference.Where(x => (x.Id.ToString() == Id && x.TransId == transId)).FirstOrDefault() });
            else
                return Json(new { Status = 0, Message = "Bad Request, invalid id or transId" });
        }

        public dynamic GetReferenceShareRecords(string Id, string transId)
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(transId))
            {
                var list = db.ReferenceShareRecord.Where(x => (x.ReferenceId.ToString() == Id)).ToList();
                var university = db.University.ToList();
                var records = new List<JsonReferenceShareRecord>();
                foreach (var item in list)
                {
                    var record = new JsonReferenceShareRecord()
                    {
                        Id = item.id,
                        UniversityId = item.UniversityId,
                        UniversityName = university.Where(x => x.Id == item.UniversityId).FirstOrDefault().Name,
                        LinkToken = item.LinkToken,
                        CreatedDate = SystemConstants.FromUnixTimeToLocal(item.CreatedUTC).ToString("dd MMM yyyy")
                    };
                    records.Add(record);
                }
                return Json(new { Status = 1, Count = list.Count, ShareList = records });
            }
            else
                return Json(new { Status = 0, Message = "Bad Request, invalid id or transId" });
        }

        
    }



}