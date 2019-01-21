using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class TaskClass
    {
        string toDo = "ToDo";
        string inProgress = "InProgress";
        string done = "Done";

        enum _state
        {
            toDo,
            inProgress,
            done
        }

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }
        [BsonElement("Status")]
        public string Status { get; set; }

        public TaskClass(string title, string description, string status)
        {
            if (title == null) throw new Exception("Provide title");
            if (UserId == null) throw new Exception("Provide UserId");
            if (status == null) status = _state.toDo.ToString();
        }

        public void ToDo()
        {
            this.Status = _state.toDo.ToString();
        }

        public void InProgress()
        {
            this.Status = _state.inProgress.ToString();
        }

        public void Done()
        {
            this.Status = _state.done.ToString();
        }
    }
}
