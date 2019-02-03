using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;

namespace ToDo.Services
{
    public class UserService : IUserService
    {
        private MongoClient mongoClient;
        private IMongoCollection<UserClass> userCollection;

        public UserService()
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
            }
            else
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

        public UserClass CreateTask(string userId, TaskClass task)
        {
            try
            {
                var filter = Builders<UserClass>.Filter.Eq("_id", ObjectId.Parse(userId));
                var update = Builders<UserClass>.Update.Push("taskList", task);

                userCollection.FindOneAndUpdate(filter, update);

                return userCollection.AsQueryable<UserClass>().SingleOrDefault(a => a.Id == ObjectId.Parse(userId));
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public UserClass ChangeTaskState(string userId, TaskClass task)
        {
            try
            {
                userCollection.UpdateOne(
                                Builders<UserClass>.Filter.Where(user => user.Id == ObjectId.Parse(userId) && user.TaskList.Any(taskItem => taskItem.Id == task.Id)),
                                Builders<UserClass>.Update.Set(user => user.TaskList[-1], task)
                            );

                return userCollection.AsQueryable<UserClass>().SingleOrDefault(a => a.Id == ObjectId.Parse(userId));
            }
            catch (Exception err)
            {
                Console.WriteLine("DB error: ");
                Console.WriteLine(err);
                throw err;
            }
        }

        public UserClass DeleteTaskById(string userId, string taskId)
        {
            try
            {
                userCollection.FindOneAndUpdateAsync(
                                user => user.Id == ObjectId.Parse(userId),
                                Builders<UserClass>.Update.PullFilter(user => user.TaskList,
                                    task => task.Id == ObjectId.Parse(taskId)
                                )
                                //Builders<UserClass>.Update.Set(user => user.TaskList[-1], task)
                            );

                return userCollection.AsQueryable<UserClass>().SingleOrDefault(a => a.Id == ObjectId.Parse(userId));
            }
            catch (Exception err)
            {
                Console.WriteLine("DB error: ");
                Console.WriteLine(err);
                throw err;
            }
        }
    }
}
