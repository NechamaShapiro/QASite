using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QASite.Data
{
    public class QuestionsRepository
    {
        private string _connectionString;

        public QuestionsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Tag GetTag(string name)
        {
            using var context = new QASiteDbContext(_connectionString);
            return context.Tags.FirstOrDefault(t => t.Name == name);
        }
        

        private int AddTag(string name)
        {
            using var context = new QASiteDbContext(_connectionString);
            var tag = new Tag { Name = name };
            context.Tags.Add(tag);
            context.SaveChanges();
            return tag.Id;
        }
        

        //public List<Question> GetQuestionsForTag(string name)
        //{
        //    using var context = new QASiteDbContext(_connectionString);
        //    return context.Questions
        //            .Where(c => c.QuestionsTags.Any(t => t.Tag.Name == name))
        //            .Include(q => q.QuestionsTags)
        //            .ThenInclude(qt => qt.Tag)
        //            .ToList();
        //}

        public void AddQuestion(Question question, List<string> tags, string email)
        {
            using var context = new QASiteDbContext(_connectionString);
            var userName = GetUserNameByEmail(email);
            var q = question;
            q.User.Name = userName;
            context.Questions.Add(q);
            context.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                context.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }

            context.SaveChanges();
        }
        public List<Question> GetQuestions()
        {
            using var context = new QASiteDbContext(_connectionString);
            return context.Questions
                .Include(q => q.QuestionsTags)
                    .ThenInclude(qt => qt.Tag)
                .Include(a => a.Answers)
                .ToList();
        }
        public Question GetQuestionById(int id)
        {
            using var context = new QASiteDbContext(_connectionString);
            return context.Questions
            .Where(q => q.Id == id)
            .Include(q => q.QuestionsTags)
                .ThenInclude(qt => qt.Tag)
            .Include(a => a.Answers)
            .SingleOrDefault();
        }
        public void AddAnswer(int questionId, string text, string userEmail)
        {
            using var context = new QASiteDbContext(_connectionString);
            var userName = GetUserNameByEmail(userEmail);
            var answer = new Answer { Name = userName, DatePosted = DateTime.Now, Text = text, QuestionId = questionId };
            context.Answers.Add(answer);
            var question = context.Questions.Where(q => q.Id == questionId).FirstOrDefault();
            question.Answers.Add(answer);
            context.SaveChanges();
        }
        public string GetUserNameByEmail(string email)
        {
            using var context = new QASiteDbContext(_connectionString);
            return context.Users.Where(u => u.Email == email).Select(u => u.Name).ToString();
        }
    }
}
