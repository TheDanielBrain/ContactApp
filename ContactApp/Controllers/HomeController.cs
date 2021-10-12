using ContactApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactApp.Controllers
{
    public class HomeController : Controller
    {
        private ContactRepository cDB = new ContactRepository();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetContacts(string SortOrder = "asc", string SortName = "LastName", int PageSize = 10, int PageNumber = 1, string Search = "")
        {
            List<Contact> cList = null;
            JsonResult jContact = null;
            int TotalRecords = 0;
            dynamic jList = new JArray() as dynamic;

            try
            {
                int Skip = (PageNumber * PageSize) - PageSize;
                cList = cDB.GetContacts(SortName, SortOrder, Search, Skip, PageSize, out TotalRecords);
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            jContact = Json(new { total = TotalRecords, rows = cList }, JsonRequestBehavior.AllowGet);
            jContact.MaxJsonLength = int.MaxValue;
            return jContact;
        }

        public JsonResult Save(Contact contact)
        {
            JsonResult jResponse = null;
            bool Status = false;

            try
            {
                cDB.SaveContact(contact);
                Status = true;
            }
            catch (Exception ex)
            {
                Status = false;
            }

            jResponse = Json(new { Status = Status }, JsonRequestBehavior.AllowGet);
            jResponse.MaxJsonLength = int.MaxValue;
            return jResponse;
        }

        [HttpPost]
        public JsonResult GetContactById(int ContactId)
        {
            bool Status = false;
            Contact contact = null;
            JsonResult jContact = null;

            try
            {
                contact = cDB.GetContactById(ContactId);
                Status = true;
            }
            catch (Exception ex)
            {
                Status = false;
            }

            jContact = Json(new { contact = contact, status = Status }, JsonRequestBehavior.AllowGet);
            jContact.MaxJsonLength = int.MaxValue;
            return jContact;
        }

        [HttpGet]
        public ActionResult Delete(int ItemId)
        {
            string Success = "";
            try
            {
                Contact usr = cDB.GetContactById(ItemId);
                cDB.DeleteContact(ItemId);
                Success = "Success";
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
                return View("Error", new HandleErrorInfo(ex, "Home", "Delete"));
            }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }
    }
}