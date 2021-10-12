using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Dynamic;


namespace ContactApp.Models
{
    public class ContactRepository
    {
        private ContactsDBEntities db = new ContactsDBEntities();

        public List<Contact> GetContacts(string SortName, string SortOrder, string Search, int Skip, int PageSize, out int TotalRecords)
        {
            List<Contact> cList = null;


                cList = db.Contacts
                    .Where(u =>
                            u.FirstName.Contains(Search) ||
                            u.LastName.Contains(Search) ||
                            u.PhoneNumber.Contains(Search) ||
                            u.Email.Contains(Search))
                    .OrderBy(SortName + " " + SortOrder)
                    .ToList();

            TotalRecords = cList.Count;

            if (PageSize > 0)
            {
                cList = cList.Skip(Skip).Take(PageSize).ToList();
            }

            return cList;
        }

        public void SaveContact(Contact Contact)
        {
            if (Contact.Id == 0)
            {
                db.Contacts.Add(Contact);
                db.SaveChanges();
            }
            else
            {
                db.Entry(Contact).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool DeleteContact(int ContactId)
        {
            Contact contact = db.Contacts.Where(c => c.Id == ContactId).FirstOrDefault();
            bool IsDeleted = false;
            try
            {
                db.Contacts.Remove(contact);
                db.SaveChanges();
                IsDeleted = true;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }

        public Contact GetContactById(int ContactId)
        {
            Contact contact = db.Contacts.Where(c => c.Id == ContactId).FirstOrDefault();
            return contact;
        }

    }
}