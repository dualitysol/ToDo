using Microsoft.IdentityModel.Protocols;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo;

namespace ToDo.Models
{
    public class UserClass
    {
        [BsonId]
        public ObjectId Id { get; set; }
        //[BsonElement("Title")]
        //public string Id { get; set; }
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("taskList")]
        public List<TaskClass> TaskList { get; set; }
        public string Token { get; set; }

        public UserClass ()
        {
            if (this.TaskList == null) this.TaskList = new List<TaskClass>();
        }
    }

    public class TaskClass
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Status")]
        public string Status { get; private set; }

        public TaskClass(string id, string Title, string Description, string Status)
        {

            Console.WriteLine("From TaskClass:");
            if (Title == null) throw new Exception("Provide title");
            // if (UserId == null) throw new Exception("Provide UserId");
            if (Status == null) Status = "To Do";
            if (id == null) this.Id = ObjectId.GenerateNewId();
            if (id is string) this.Id = ObjectId.Parse(id);
            this.Title = Title;
            this.Description = Description;
            this.Status = Status;
            Console.WriteLine("ID: " + this.Id);
            Console.WriteLine("Title: " + this.Title);
            Console.WriteLine("Description: " + this.Description);
            Console.WriteLine("Status: " + this.Status);
        }

        public void ToDo()
        {
            this.Status = "To Do";
        }

        public void InProgress()
        {
            this.Status = "In Progress";
        }

        public void Done()
        {
            this.Status = "Done";
        }
    }
}
