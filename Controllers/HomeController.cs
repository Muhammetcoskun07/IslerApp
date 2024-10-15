using Dapper;
using IslerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace IslerApp.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "Server=45.84.189.34\\MSSQLSERVER2019;Initial Catalog=muham128_IsTakip;User Id=muham128_IsTakipdbuser;Password=522848Aa.;TrustServerCertificate=True";

        public IActionResult Index()
        {
            using var connection = new SqlConnection(connectionString);
            var jobs = connection.Query<IslerModel>("SELECT Name, SUM(CASE WHEN IsTakip.StatusId = 1 THEN 1 ELSE 0 END) as Active,SUM(CASE WHEN IsTakip.StatusId = 2 THEN 1 ELSE 0 END) as Tamamlandi, COUNT(IsTakip.Id) as Total FROM status LEFT JOIN IsTakip ON status.Id = IsTakip.StatusId GROUP BY Name").ToList();

            return View(jobs);
        }
        public IActionResult HepsiniGoster()
        {
            using var connection = new SqlConnection(connectionString);
            var jobs = connection.Query<IslerModel>("SELECT IsTakip.Id,Name,IsBaslik,StatusId, CreatedDate, Status, SUM(CASE WHEN IsTakip.StatusId = 1 THEN 1 ELSE 0 END) as Active,SUM(CASE WHEN IsTakip.StatusId = 2 THEN 1 ELSE 0 END) as Tamamlandi, COUNT(IsTakip.Id) as Total FROM IsTakip LEFT JOIN status ON status.Id = IsTakip.StatusId GROUP BY Name,IsBaslik,StatusId, IsTakip.Id, CreatedDate, Status ORDER BY Status").ToList();

            return View(jobs);
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT IsTakip.*, Status.Status FROM IsTakip LEFT JOIN Status ON Status.Id = IsTakip.StatusId WHERE IsTakip.Id = @Id";
                var post = connection.QuerySingleOrDefault<IslerModel>(sql, new { Id = id });

                return View(post);
            }
        }
        public IActionResult Duzenle(int? id)
        {
            using var connection = new SqlConnection(connectionString);
            var edit = connection.QuerySingleOrDefault<IslerModel>("SELECT * FROM IsTakip LEFT JOIN status ON status.Id = IsTakip.StatusId WHERE IsTakip.Id = @id", new { id = id });

            return View(edit);
        }
        [HttpPost]
        public IActionResult Duzenle(IslerModel model)
        {
            using var connection = new SqlConnection(connectionString);

            var sql = "UPDATE IsTakip SET IsBaslik=@IsBaslik, Detay=@Detay, Name=@Name, CreatedDate = @CreatedDate WHERE IsTakip.Id = @Id";

            var parameters = new
            {
                model.IsBaslik,
                model.Detay,
                CreatedDate = DateTime.Now,
                model.Name,
                model.Id,
            };
            var affectedRows = connection.Execute(sql, parameters);

            ViewBag.Message = "Güncellendi.";
            ViewBag.MessageCssClass = "alert-success";
            return View("Message");
        }

        public IActionResult Sil(int id)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = "DELETE FROM IsTakip WHERE Id = @Id";

            var rowsAffected = connection.Execute(sql, new { Id = id });

            return RedirectToAction("Index");
        }
        public IActionResult ChangeStatus(int id,IslerModel model)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = "UPDATE IsTakip SET StatusId = @StatusId WHERE IsTakip.Id = @Id";
            var parameters = new
            {
                model.StatusId,
                Id = id
            };
            var affectedRows = connection.Execute(sql, parameters);
            ViewBag.Message = "Güncellendi.";
            ViewBag.MessageCssClass = "alert-success";
            return View("Message");
        }

    }
}
