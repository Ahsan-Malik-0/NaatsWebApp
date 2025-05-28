using NaatsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace NaatsWebApp.Controllers
{
    public class AlbumController : Controller
    {
        DBHelper db = new DBHelper();
        // GET: Album
        [HttpGet]
        public ActionResult CreateAlbum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAlbum(Album obj)
        {

            if (ModelState.IsValid)
            {
                string filename = obj.imgfile.FileName;
                string extension = Path.GetExtension(filename);
                var allowsExtension = new[] { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };
                if (allowsExtension.Contains(extension))
                {
                    obj.nid = Session["id"].ToString();
                    string filena = obj.nid + "_" + obj.ano + extension;
                    string serverPath = Path.Combine(Server.MapPath("~/Assets/Images"), filena);
                    obj.imgfile.SaveAs(serverPath);
                    obj.imgPath = "/Assets/Images/" + filena;
                    string query = $"INSERT INTO Album (ano, nid, title, year, imgPath) " +
                            $"VALUES ('{obj.ano}', '{obj.nid}', '{obj.title}', '{obj.year}', '{obj.imgPath}')";

                    try
                    {
                        db.OpenConnection();
                        db.InsertUpdateDelete(query);
                        db.CloseConnection();

                        TempData["Success"] = "Album added successfully!";
                        return RedirectToAction("Dashboard", "NK");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View(); // Show view with error
                    }
                    finally
                    {
                        db.CloseConnection();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid file type. Please upload a valid image file.";
                    return View();
                }
            }

            else
            {
                ViewBag.ErrorMessage = "Fill all fields";
                return View();
            }

        }

        [HttpGet]
        public ActionResult AllAlbums(string id)
        {
            string query = $"Select imgPath, ano, title, year from Album where nid='{id}'";
            List<Album> alist = new List<Album>();
            db.OpenConnection();
            SqlDataReader sdr = db.getData(query);
            while (sdr.Read())
            {
                Album obj = new Album
                {
                    imgPath = sdr["imgPath"].ToString(),
                    ano = sdr["ano"].ToString(),
                    title = sdr["title"].ToString(),
                    year = sdr["year"].ToString()
                };
                alist.Add(obj);
            }
            return View(alist);
        }
    }
}