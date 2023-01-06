using System;
using System.Collections;
using System.Collections.Generic;

namespace RadinProjectNotes
{
    [Serializable]
    public class UserDatabase : IEnumerable<User>
    {
        public long dateSaved;  //in ticks since unix era (DateTime.UtcNow.Ticks)
        public List<User> userData;

        public UserDatabase()
        {
            this.userData = new List<User>();
            this.dateSaved = 0;
        }

        public int NumUsers
        {
            get { return userData.Count; }
        }

        public User AddUser(string username, string password, Permissions permissions)
        {
            User newUser = new User(username, password, permissions);
            this.userData.Add(newUser);
            return newUser;
        }

        public bool DeleteUser(Guid id)
        {
            foreach (User user in userData)
            {
                if (user.ID == id)
                {
                    userData.Remove(user);
                    return true;
                }
            }

            return false;
        }

        public User SetUserPermissions(Guid id, Permissions permissions)
        {
            foreach (User user in userData)
            {
                if (user.ID == id)
                {
                    user.permissions = permissions;
                    return user;
                }
            }

            throw new UserNotFound(id);
        }

        public User MatchUsernameAndPassword(string username, string password)
        {
            //password may be hashed already or not, so check for both
            foreach(User user in this.userData)
            {
                if (user.username.ToLower() == username.ToLower())
                {
                    //if password was reset, send back the user here
                    if (user.password == Security.ResetPassword)
                    {
                        return user;
                    }

                    //check passwords as well
                    string hashedPassword = Security.HashSHA1(password + user.ID.ToString());
                    if ((hashedPassword == user.password) || (password == user.password))
                    {
                        return user;
                    }
                }
            }

            throw new InvalidUsernamePassword();
        }

        public User FindUserById(Guid id)
        {
            foreach (User user in this.userData)
            {
                if (user.ID == id)
                {
                    return user;
                }
            }

            throw new UserNotFound(id);
        }

        public bool ChangePassword(string username, string newPassword)
        {
            foreach (User user in this.userData)
            {
                if (user.username.ToLower() == username.ToLower())
                {
                    string hashedPassword = User.HashPassword(newPassword, user.ID);
                    user.password = hashedPassword;
                    return true;
                }
            }

            return false;
        }

        public bool UsernameExists(string username)
        {
            foreach (User user in userData)
            {
                if (user.username.ToLower() == username.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<User> GetEnumerator()
        {
            return ((IEnumerable<User>)userData).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)userData).GetEnumerator();
        }

        public class UserNotFound : Exception
        {
            public UserNotFound(Guid id) : base(String.Format("User with id {0} not found", id.ToString())) { }
        }

        public class InvalidUsernamePassword : Exception
        {
            public InvalidUsernamePassword() : base("Invalid username and password provided.") { }
        }
    }
}
