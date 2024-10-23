using Dapper;
using IslerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IslerApp.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddJob()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddJob(IslerModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageCssClass = "alert-danger";
                ViewBag.Message = "Eksik veya hatalı işlem yaptın";
                return View("Message");
            }
            using var connection = new SqlConnection(connectionString);
            var ilanlar = "INSERT INTO IsTakip (Name, IsBaslik, Detay, StatusId) VALUES (@Name, @IsBaslik, @Detay, @Status)";

            var data = new
            {
                model.Name,
                model.IsBaslik,
                model.Detay,
                model.Status,
            };

            var rowsAffected = connection.Execute(ilanlar, data);

            ViewBag.MessageCssClass = "alert-success";
            ViewBag.Message = "Eklendi.";
            ViewBag.Return = "IlanEkle";
            return View("Message");
        }
    }
}
