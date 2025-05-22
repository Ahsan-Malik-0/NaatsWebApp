using Antlr.Runtime.Misc;
using NaatsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace NaatsWebApp.Controllers
{
    public class NKController : Controller
    {
        // GET: NT
        DBHelper db = new DBHelper();

        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(NaatKhwaan n, string formAction)
        {
            if (formAction == "Login")
            {
                return RedirectToAction("Login");
            }
            else if (formAction == "Create")
            {
                try
                {
                    db.OpenConnection();

                    string query = $"INSERT INTO naatKhwaan (name, city, gender, email, pass) VALUES ('{n.fullname}', '{n.city}', '{n.gender}', '{n.email}', '{n.pwd}')";
                    db.InsertUpdateDelete(query);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(); // Show view with error
                }
                finally
                {
                    db.CloseConnection();
                }

                return RedirectToAction("Login");
            }

            // Default case to avoid compiler error
            return View();
        }


        //public ActionResult Login()

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass, string action)
        {
            if (action == "Signin")
            {
                return RedirectToAction("Signin");
            }

            if (action == "Login")
            {
                try
                {
                    db.OpenConnection();
                    string query = "SELECT * FROM naatKhwaan WHERE email = @Email AND pass = @Pass";
                    SqlCommand cmd = new SqlCommand(query, db.conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Pass", pass);

                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        Session["id"] = sdr["id"].ToString();
                        Session["name"] = sdr["name"].ToString();
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ViewBag.Result = "Invalid Email or Password";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
                finally
                {
                    db.CloseConnection();
                }
            }

            return View();
        }

        public ActionResult Dashboard()
        {
            // Redirect if session is null
            if (Session["id"] == null && Session["name"] == null)
            {
                return RedirectToAction("Login");
            }

            string query = "SELECT * FROM naatKhwaan";
            List<NaatKhwaan> naatKhwaans = new List<NaatKhwaan>();

            try
            {
                db.OpenConnection();
                SqlDataReader sdr = db.getData(query);

                while (sdr.Read())
                {
                    NaatKhwaan n = new NaatKhwaan
                    {
                        id = (int)sdr["id"],
                        fullname = sdr["name"].ToString(),
                        city = sdr["city"].ToString(),
                        gender = Convert.ToChar(sdr["gender"]),
                        email = sdr["email"].ToString(),
                        pwd = sdr["pass"].ToString()
                    };

                    naatKhwaans.Add(n);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }

            return View(naatKhwaans); // ✅ Send list to view
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            db.OpenConnection();
            string query = $"SELECT * FROM naatKhwaan WHERE id = {id}";
            SqlDataReader sdr = db.getData(query);
            sdr.Read();
            NaatKhwaan n = new NaatKhwaan 
            {
                fullname = sdr["name"].ToString(),
                city = sdr["city"].ToString(),
                gender = Convert.ToChar(sdr["gender"]),
                email = sdr["email"].ToString(),
                pwd = sdr["pass"].ToString()
            };
            return View(n);
        }

        [HttpPost]
        public ActionResult Edit(NaatKhwaan n)
        {
            try
            {
                db.OpenConnection();
                string query = $"UPDATE naatKhwaan SET name = '{n.fullname}', city = '{n.city}', gender = '{n.gender}', email = '{n.email}', pass = '{n.pwd}' WHERE id = {n.id}";
                db.InsertUpdateDelete(query);

                TempData["Success"] = "Record updated successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(n);
            }
            finally
            {
                db.CloseConnection();
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                db.OpenConnection();
                string query = "DELETE FROM naatKhwaan WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, db.conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            finally
            {
                db.CloseConnection();
            }

            return RedirectToAction("Dashboard");
        } 

        public ActionResult Logout()
        {
            Session.Clear();         // Clear all session variables
            Session.Abandon();       // End the current session
            return RedirectToAction("Login"); // Redirect to login page
        }

    }
}