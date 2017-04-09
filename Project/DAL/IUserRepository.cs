using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLP.Models;

namespace TLP.DAL
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        User Save(User user);


    }
}