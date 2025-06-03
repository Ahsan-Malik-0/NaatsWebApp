using NaatsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NaatsWebApp.Controllers
{
    public class NaatController : Controller
    {
        DBHelper db = new DBHelper();
        // GET: Naat
        public ActionResult CreateNaat()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNaat(Naats obj, string albumNo)
        {
            if (ModelState.IsValid)
            {
                string filename = obj.audiofile.FileName;
                string extension = System.IO.Path.GetExtension(filename);
                var allowedExtensions = new[] { ".mp3" };
                if (allowedExtensions.Contains(extension))
                {
                    obj.nid = Session["id"].ToString();
                    obj.ano = int.Parse(albumNo);
                    obj.title = filename.Split('.')[0];
                    string serverPath = System.IO.Path.Combine(Server.MapPath("~/Assets/Audio"), filename);
                    obj.audiofile.SaveAs(serverPath);
                    obj.audiopath = "/Assets/Audio/" + filename;
                    string query = $"INSERT INTO Naats (nid, ano, nno, title, audioPath) " +
                                   $"VALUES ('{obj.nid}', '{obj.ano}', '{obj.title}', '{obj.audiopath}')";
                    try
                    {
                        db.OpenConnection();
                        db.InsertUpdateDelete(query);
                        db.CloseConnection();
                        TempData["Success"] = "Naat added successfully!";
                        return RedirectToAction("Dashboard", "NK");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View(); // Show view with error
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid file type. Please upload an audio file.";
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AllNaats(string nid, int ano)
        {
            List<Naats> naats = new List<Naats>();
            db.OpenConnection();
            string query = $"SELECT * FROM Naats WHERE nid = ${nid} AND ano = ${ano}";
            SqlDataReader sdr = db.getData(query);

            while (sdr.Read())
            {
                Naats n = new Naats();
                n.title = sdr["title"].ToString();
                n.audiopath = sdr["audioPath"].ToString();
                naats.Add(n);
            }
            sdr.Close();
            db.CloseConnection();
            return View(naats);
        }
    }


}