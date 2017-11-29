﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class WishListsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: WishLists
        public ActionResult Index()
        {
            var Id = User.Identity.GetUserId();
            var list = (from rec in db.WishList where rec.User.Id == Id select rec.Product.Id).ToList();

            if (list == null)
                return View(list);
            var listWish = (from rec in db.Products where list.Contains(rec.Id) select rec).ToList();
                 
            return View(listWish);
        }
         
        // GET: WishLists/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
         
        public ActionResult Create1(int id)
        {
            if (ModelState.IsValid)
            {
                WishList w = db.WishList.FirstOrDefault(rec => rec.Product.Id == id);
                if (w == null)
                {
                    Product p = db.Products.Find(id);
                    if (p == null)
                    {
                        Console.WriteLine("no product in Db");
                        //need to call api
                    }
                    
                    ApplicationUser u = db.Users.Find(User.Identity.GetUserId());
                    Console.WriteLine("user id" + u.Email);

                    WishList wish = new WishList()
                    {
                        ASIN = p.ASIN,

                        Product = p
                        /*
                        new Product()
                        {
                            Id = p.Id,
                            /*ASIN = p.ASIN,
                            Category = p.Category,
                            DetailPageURL = p.DetailPageURL,
                            Brand = p.Brand,
                            LargeImage = p.LargeImage,
                            OtherInfo = p.OtherInfo
                        } */,
                        User = u
                    };

                    db.WishList.Add(wish);
                    db.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
      
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
         
        // POST: WishLists/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            WishList wishList = db.WishList.FirstOrDefault(rec=>rec.Product.Id==id);
            wishList.Product = null;
            wishList.User = null;
            db.WishList.Remove(wishList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
