using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLP.Models;

namespace TLP.DAL
{
    public class EFUserRepository : IUserRepository
    {
        DataAccessLayer dal = new DataAccessLayer();
        public IQueryable<User> Users
        {
            get
            {
                return dal.Users;
            }
        }

        public User Save(User user)
        {
            if (user.uid==0)
            {
                dal.Users.Add(user);

            }
            else
            {
                dal.Entry(user).State = System.Data.Entity.EntityState.Modified;
            }
            dal.SaveChanges();
            return user;
        }
    }
}