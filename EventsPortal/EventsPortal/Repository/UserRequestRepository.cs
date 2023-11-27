using EventsPortal.Data;
using EventsPortal.Models;
using EventsPortal.Models.DBObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsPortal.Repository
{
    public class UserRequestRepository
    {
        private ApplicationDbContext dbContext;

        public UserRequestRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public UserRequestRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UserRequestModel> GetAllUserRequests()
        {
            List<UserRequestModel> userRequestList = new List<UserRequestModel>();

            foreach (UserRequest dbUserRequest in dbContext.UserRequests)
            {
                userRequestList.Add(MapDbObjectToModel(dbUserRequest));
            }
            return userRequestList;
        }

        public UserRequestModel GetUserRequestByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.UserRequests.FirstOrDefault(x => x.RequestId == ID));
        }

        public void InsertUserRequest(UserRequestModel userRequestModel)
        {
            userRequestModel.RequestID = Guid.NewGuid();
            userRequestModel.RequestDateTime = DateTime.Now;
            dbContext.UserRequests.Add(MapModelToDbObject(userRequestModel));
            dbContext.SaveChanges();
        }

        public void UpdateUserRequest(UserRequestModel userRequestModel)
        {
            UserRequest existingUserRequest = dbContext.UserRequests.FirstOrDefault(x => x.RequestId == userRequestModel.RequestID);
            if (existingUserRequest != null)
            {
                existingUserRequest.RequestType = userRequestModel.RequestType;
                existingUserRequest.RequestDateTime = userRequestModel.RequestDateTime;
                existingUserRequest.AdditionalInfo = userRequestModel.AdditionalInfo;
                existingUserRequest.UserName = userRequestModel.UserName;
                existingUserRequest.UserEmail = userRequestModel.UserEmail;
                dbContext.SaveChanges();
            }
        }

        public void DeleteUserRequest(Guid id)
        {
            UserRequest existingUserRequest = dbContext.UserRequests.FirstOrDefault(x => x.RequestId == id);
            if (existingUserRequest != null)
            {
                dbContext.UserRequests.Remove(existingUserRequest);
                dbContext.SaveChanges();
            }
        }

        private UserRequestModel MapDbObjectToModel(UserRequest dbUserRequest)
        {
            UserRequestModel userRequestModel = new UserRequestModel();
            if (dbUserRequest != null)
            {
                userRequestModel.RequestID = dbUserRequest.RequestId;
                userRequestModel.RequestType = dbUserRequest.RequestType;
                userRequestModel.RequestDateTime = dbUserRequest.RequestDateTime;
                userRequestModel.AdditionalInfo = dbUserRequest.AdditionalInfo;
                userRequestModel.UserName = dbUserRequest.UserName;
                userRequestModel.UserEmail = dbUserRequest.UserEmail;
            }
            return userRequestModel;
        }

        private UserRequest MapModelToDbObject(UserRequestModel userRequestModel)
        {
            UserRequest userRequest = new UserRequest();

            if (userRequestModel != null)
            {
                userRequest.RequestId = userRequestModel.RequestID;
                userRequest.RequestType = userRequestModel.RequestType;
                userRequest.RequestDateTime = userRequestModel.RequestDateTime;
                userRequest.AdditionalInfo = userRequestModel.AdditionalInfo;
                userRequest.UserName = userRequestModel.UserName;
                userRequest.UserEmail = userRequestModel.UserEmail;
            }

            return userRequest;
        }
    }
}
