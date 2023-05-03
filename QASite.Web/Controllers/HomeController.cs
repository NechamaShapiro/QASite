using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QASite.Data;
using QASite.Web.Models;
using System.Diagnostics;

namespace QASite.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new QuestionsRepository(_connectionString);
            return View(repo.GetQuestions());
        }

        //public IActionResult ViewForTag(string tagName)
        //{
        //    var repo = new QuestionsRepository(_connectionString);
        //    return View(repo.GetQuestionsForTag(tagName));
        //}

        [Authorize]
        public IActionResult AskQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AskQuestion(Question question, List<string> tags)
        {
            question.DatePosted = DateTime.Now;
            var repo = new QuestionsRepository(_connectionString);
            var email = User.Identity.Name;
            repo.AddQuestion(question, tags, email);
            return Redirect("/");
        }
        public IActionResult ViewQuestion(int id)
        {
            var repo = new QuestionsRepository(_connectionString);
            return View(repo.GetQuestionById(id));
        }
        [HttpPost]
        public IActionResult AddAnswer(int questionId, string text, string userEmail)
        {
            var repo = new QuestionsRepository(_connectionString);
            repo.AddAnswer(questionId, text, userEmail);
            return RedirectToAction("index");
        }
    }
}