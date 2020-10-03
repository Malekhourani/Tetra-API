using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Tetra_API.Models;
using Newtonsoft.Json;
using System;

namespace Tetra_API.Controllers
{

    public class UsersController : ApiController
    {
        private Tetra db = new Tetra();

        [Route("~/api/users/{userID:int}/contacts")]
        public async Task<IHttpActionResult> getContacts(int userID)
        {
            var contacts = await Task.Run(() =>
                 db.Contacts
                .Where(u => u.UserID == userID)
                .Select(c => new
                {
                    userID = c.ContactID,
                    nickname = c.User1.NickName,
                    lastSeen = c.User1.LastSeen,
                    bio = c.User1.Bio,
                    email = c.User1.Email,
                    fullName = c.User1.FullName,
                    status = c.User1.Status,
                    profilePicture = c.User1.Medium != null ? c.User1.ProfilePicture : null
                })
            );


            return Json(contacts);
        }

        [Route("~/api/users/{userID:int}/blockedUser")]
        public async Task<IHttpActionResult> getBlockList(int userID)
        {
            var blockedUsers = await Task.Run(() =>
            db.BlockedAccounts
            .Where(u => u.BlockerID == userID)
            .Select(b => new
            {
                userID = b.BlockedID,
                nickname = b.User1.NickName,
                profilePicture = b.User1.Medium != null ? b.User1.ProfilePicture : null
            }
            ));

            return Json(blockedUsers);
        }

        //this will return picture path in the server, the server will use this path to send the file to user
        [Route("~/api/users/{userID:int}/picture")]
        public async Task<IHttpActionResult> getUserPicture(int userID)
        {

            var user = await db.Users
                            .Where(u => u.UserID == userID)
                            .Select((f) => f.Medium.Path)
                            .SingleOrDefaultAsync();

            return Json(user);
        }

        [Route("~/api/users/{userID:int}/status")]
        public async Task<IHttpActionResult> getUserStatus(int userID)
        {

            var userInfo = await db.Users
                             .Where(u => u.UserID == userID)
                             .Select(i => new
                             {
                                 userID = i.UserID,
                                 status = i.Status
                             })
                             .SingleOrDefaultAsync();

            return Json(userInfo);
        }

        [Route("~/api/users/{userID:int}/bio")]
        public async Task<IHttpActionResult> getUserBio(int userID)
        {

            var user = await db.Users
                             .Where(u => u.UserID == userID)
                             .Select(i => new
                             {
                                 userID = i.UserID,
                                 bio = i.Bio
                             })
                             .SingleOrDefaultAsync();

            return Json(user);
        }

        [Route("~/api/users/{userID:int}/code")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> getActivateCode(int userID)
        {

            var codeInfo = await db.NotActivatedAccounts
                                 .Where(u => u.UserID == userID)
                                 .Select(c => new { userID = c.UserID, code = c.ActivateCode })
                                 .SingleOrDefaultAsync();

            if (codeInfo == null) BadRequest();

            return Json(codeInfo);
        }

        [Route("~/api/users/{userID:int}/code/{code}")]
        [HttpPost]
        [HttpGet]
        public async Task<IHttpActionResult> ActivateAccount(int userID, string code)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(code))
                return BadRequest();

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return NotFound();

            await db.ActivateMyAccount(userID, code);

            return Ok();
        }

        [Route("~/api/users/{userID:int}/bio/{bio}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditBio(int userID, string bio)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(bio))
                return BadRequest();

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return NotFound();

            await db.EditUserBio(userID, bio);

            return Ok();
        }

        [Route("~/api/users/{userID:int}/email/{oldEmail}/{newEmail}")]
        [HttpPut]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> EditEmail(int userID, string oldEmail, string newEmail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(newEmail))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(oldEmail))
                return NotFound();

            User user = await Task.Run(() => db.Users.Where((u) => u.Email.ToLower() == oldEmail.ToLower() && u.UserID == userID).FirstOrDefault());

            if (user == null)
                return NotFound();

            await db.EditUserEmail(oldEmail, newEmail);

            return Ok();
        }

        [Route("~/api/users/{userID:int}/nickname/{newName}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditNickname(int userID, string newName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest();

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);

            if (user == null)
                return NotFound();

            await db.EditUserNickName(userID, newName);

            return Ok();
        }


        [Route("~/api/users/{userID:int}/media/{mediaID:int}/path/{path}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditProfilePicture(int userID, int mediaID, string path)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(path))
                return BadRequest();

            if (!(await db.Users.Select(u => u.UserID == userID).FirstOrDefaultAsync()))
                return NotFound();

            User user = await Task.Run(() => db.Users.Where((u) => u.UserID == userID).FirstOrDefault());

            if (user == null)
                return BadRequest();

            await db.SetAnImage(userID,
                                mediaID,
                                null,
                                path);

            return Ok();
        }

        [Route("~/api/users/{userID:int}/status/{status}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditStatus(int userID, string status)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return NotFound();

            if (user.Status == status.ToUpper()) return BadRequest();
            else
            {
                if (user.Status == "ONLINE")
                {

                    await db.GoOffline(userID);

                }
                else await db.GoOnline(userID);
            }

            return Ok();
        }

        [Route("~/api/users/{userID:int}/password/{oldPassword}/{newPassword}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditPassword(int userID, string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return NotFound();

            await db.EditUserPassword(userID, oldPassword, newPassword);

            return Ok();
        }

        [Route("~/api/users")]
        [ResponseType(typeof(int))]
        [HttpPost]
        [HttpGet]
        public async Task<IHttpActionResult> CreateUser(User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(user.NickName) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password)) return BadRequest();

            try
            {
                await db.CreateAUser(user);
            }
            catch
            {
                return BadRequest();
            }

            int? id = await db.Users
                              .Where((u) => u.NickName == user.NickName)
                              .Select((u) => u.UserID)
                              .SingleOrDefaultAsync();

            return Json<int?>(id);
        }

        [ResponseType(typeof(int))]
        [Route("~/api/users/{userID:int}/contacts/add/{friendID:int}")]
        [HttpPost]
        [HttpGet]
        public async Task<IHttpActionResult> AddFriend(int userID, int friendID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return BadRequest();

            var friend = await db.Users.FirstOrDefaultAsync((u) => u.UserID == friendID);
            if (friend == null) return BadRequest();

            await db.AddAFriend(userID, friendID);

            return Ok();
        }

        [Route("~/api/users/{userID:int}")]
        [HttpDelete]
        [HttpGet]
        public async Task<IHttpActionResult> DeleteUser(int userID)
        {
            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return NotFound();

            await db.DeleteUser(userID);

            return Ok();
        }


        [Route("~/api/users/{userID:int}/contacts/remove/{friendID:int}")]
        [HttpDelete]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveFromContacts(int userID, int friendID)
        {
            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) return NotFound();

            var friend = await db.Users.FirstOrDefaultAsync((u) => u.UserID == friendID);
            if (friend == null) return NotFound();

            await db.DeleteFromContacts(userID, friendID);


            return Ok();
        }

        [Route("~/api/users/{blockerID:int}/block/{blockedID:int}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> BlockUser(int blockerID, int blockedID)
        {
            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == blockerID);
            if (user == null) return NotFound();

            var friend = await db.Users.FirstOrDefaultAsync((u) => u.UserID == blockedID);
            if (friend == null) return NotFound();

            await db.BlockAUser(blockerID, blockedID);

            return Ok();
        }

        [Route("~/api/users/{blockerID:int}/unblock/{blockedID:int}")]
        [HttpDelete]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveBlock(int blockerID, int blockedID)
        {
            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == blockerID);
            if (user == null) return NotFound();

            var friend = await db.Users.FirstOrDefaultAsync((u) => u.UserID == blockedID);
            if (friend == null) return NotFound();

            await db.RemoveBlock(blockerID, blockedID);

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }

        [Route("~/api/users/email/{email}/password/{password}")]
        [HttpGet]
        public async Task<IHttpActionResult> getUserInfo(string email, string password)
        {

            var userInfo = await db.Users
                                 .Where(u => u.Email.ToLower() == email.ToLower() && u.Password == password)
                                 .Select(u => new
                                 {
                                     userID = u.UserID,
                                     nickname = u.NickName,
                                     fullname = u.FullName,
                                     profilePicture = u.Medium != null ? u.ProfilePicture : null,
                                     medium = u.Medium != null ? new
                                     {
                                         mediaID = u.Medium.MediaID,
                                         mediaName = u.Medium.MediaName,
                                         path = u.Medium.Path
                                     } : null,
                                     contacts = u.Contacts
                                                 .Select(us => new
                                                 {
                                                     contactID = us.ContactID,
                                                     user1 = new
                                                     {
                                                         userID = us.User1 != null ? us.User1.UserID : 0,
                                                         nickname = us.User1.NickName,
                                                         email = us.User1.Email,
                                                         bio = us.User1.Bio,
                                                         fullname = us.User1.FullName,
                                                         lastSeen = us.User1.LastSeen,
                                                         status = us.User1.Status,
                                                         profilePicture = us.User1.Medium != null ? us.User1.ProfilePicture : null
                                                     }
                                                 }),

                                     groups = u.Participants
                                               .Where(u1 => u1.UserID == u.UserID)
                                               .Select(g => new
                                               {
                                                   groupID = g.Group.GroupID,
                                                   groupName = g.Group.GroupName,
                                                   groupType = g.Group.GroupType,
                                                   createTime = g.Group.CreateTime,
                                                   description = g.Group.Description,
                                                   groupPicture = g.Group.GroupPicture != null ? g.Group.GroupPicture : null,
                                                   owner = g.Group.Owner != null ? g.Group.Owner : null,
                                                   user = g.User != null ? new
                                                   {
                                                       userID = g.Group.Owner,
                                                       nickname = g.User.NickName,
                                                       fullName = g.User.FullName,
                                                       lastSeen = g.User.LastSeen,
                                                       status = g.User.Status,
                                                       email = g.User.Email,
                                                       bio = g.User.Bio,
                                                       profilePicture = g.User.Medium != null ? g.User.ProfilePicture : null
                                                   } : null,
                                                   groupMessages = g.Group.GroupMessages
                                                                  .Select(m => new
                                                                  {
                                                                      groupID = m.GroupID,
                                                                      senderID = m.User != null ? m.SenderID : null,
                                                                      messageID = m.Message != null ? m.MessageID : null,
                                                                      message = m.Message != null ? new
                                                                      {
                                                                          messageID = m.MessageID,
                                                                          content = m.Message.Content,
                                                                          sentTime = m.Message.SentTime,
                                                                          replyMessageID = m.Message.Message1 != null ? m.Message.ReplyMessageID : null,
                                                                          attachment = m.Message.Medium != null ? m.Message.Attachment : null
                                                                      } : null
                                                                  }).Take(25)
                                               })
                                 }).SingleOrDefaultAsync();



            if (userInfo == null) return BadRequest();

            return Json(userInfo, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [Route("~/api/users/search/{name}")]
        public async Task<IHttpActionResult> SearchingForUser(string name)
        {
            var users = await Task.Run(() =>
                              db.Users
                              .Where(u => u.FullName == name)
                              .Select(u => new
                              {
                                  userID = u.UserID,
                                  nickname = u.NickName,
                                  fullname = u.FullName,
                                  pictureProfile = u.Medium != null ? u.ProfilePicture : null
                              }));

            if (users == null) BadRequest();

            return Json(users);
        }

    }
}