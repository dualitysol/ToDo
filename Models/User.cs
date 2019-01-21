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
        public string[] TaskList { get; set; }
        public string Token { get; set; }
    }

    public class UserModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<UserClass> userCollection;

        public UserModel()
        {
            mongoClient = new MongoClient(ConfigurationManager.AppSetting["mongoDBHost"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSetting["mongoDBName"]);
            userCollection = db.GetCollection<UserClass>("users");
        }

        public List<UserClass> findAll()
        {
            return userCollection.AsQueryable<UserClass>().ToList();
        }

        public UserClass find(string id)
        {
            var userId = new ObjectId(id);
            return userCollection.AsQueryable<UserClass>().SingleOrDefault(a => a.Id == userId);
        }

        public UserClass findByEmail(string email) => userCollection.AsQueryable<UserClass>().SingleOrDefault(a => a.Email == email);

        public void create(UserClass user) 
        {
            var sameEmailUser = this.findByEmail(user.Email);
            if (sameEmailUser != null)
            {
                throw new Exception("User with current email already exists!");
            } else
            {
                userCollection.InsertOne(user);
            };
        }

        public void update(UserClass user)
        {
            userCollection.UpdateOne(
                Builders<UserClass>.Filter.Eq("_id", user.Id),
                Builders<UserClass>.Update
                    .Set("username", user.UserName)
                    .Set("email", user.Email)
                    .Set("password", user.Password)
            );
        }

        public void delete(UserClass user)
        {
            userCollection.DeleteOne(Builders<UserClass>.Filter.Eq("_id", user.Id));
        }
    }
}
