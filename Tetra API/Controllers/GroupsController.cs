using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tetra_API.Models;
using System;

namespace Tetra_API.Controllers
{
    public class GroupsController : ApiController
    {
        private Tetra db = new Tetra();
        enum GroupType
        {
            CONVERSATION,
            GROUP
        }

        [Route("~/api/groups/{groupID:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGroup(int groupID)
        {
            var group = await
                                db.Groups
                                .Where(g => g.GroupID == groupID)
                                 .Select((g) => new
                                 {
                                     groupID = g.GroupID,
                                     groupName = g.GroupName,
                                     groupType = g.GroupType,
                                     owner = g.Owner,
                                     groupPicture = g.GroupPicture,
                                     groupMessage = g.GroupMessages != null ? 
                                                    g.GroupMessages
                                                    .Select(m => new
                                                    {
                                                        groupID = m != null ? m.GroupID : 0,
                                                        senderID = m != null ? m.SenderID : 0,
                                                        messageID = m != null ? m.MessageID : 0,
                                                        message = m.Message != null ? new
                                                        {
                                                            messageID = m.Message.MessageID,
                                                            content = m.Message.Content,
                                                            sentTime = m.Message.SentTime,
                                                            attachment = m.Message.Medium != null ? m.Message.Medium.MediaID : 0,
                                                            replyMessageID = m.Message.Message1 != null ? m.Message.ReplyMessageID : null,
                                                        } : null
                                                    }).Take(1) : null,
                                     participants = g.Participants
                                                    .Where(p => p.GroupID == g.GroupID)
                                                    .Select(u => new
                                                    {
                                                        groupID = u != null ? u.GroupID : 0,
                                                        userID = u != null ? u.UserID : 0,
                                                        adminID = u.AdminID != null ? u.AdminID : 0,
                                                        user = new
                                                        {
                                                            userID = u.UserID,
                                                            nickname = u.User.NickName,
                                                            fullname = u.User.FullName,
                                                            email = u.User.Email,
                                                            bio = u.User.Bio,
                                                            lastSeen = u.User.LastSeen,
                                                            status = u.User.Status,
                                                            profilePicture = u.User.ProfilePicture
                                                        }
                                                    })
                                 }).SingleOrDefaultAsync();
            if (group == null)
                return NotFound();

            return Json(group, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [Route("~/api/groups/user/{userID:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> getUserGroups(int userID)
        {

            var groups = await Task.Run(() =>
            {
                return db.Participants
                .Where(p => p.User.UserID == userID)
                .Select(g => new
                {
                    groupID = g.GroupID,

                    groupType = g.Group.GroupType,

                    groupName = g.Group.GroupName,

                    owner = g.Group.Owner,

                    groupPicture = g.Group.GroupPicture,

                    participants = g.Group.Participants
                                   .Select(p => new
                                   {
                                       userID = p.UserID,
                                       nickname = p.User.NickName,
                                       email = p.User.Email,
                                       status = p.User.Status,
                                       bio = p.User.Bio,
                                       profilePicture = p.User.ProfilePicture
                                   })
                                   .Where(p => p.userID != userID),

                    groupMessage = g.Group.GroupMessages
                                      .Where(gr => gr.GroupID == g.GroupID)
                                      .Select(m => new
                                      {
                                          groupID = m.GroupID,
                                          senderID = m.SenderID,
                                          messageID = m != null ? m.MessageID : 0,
                                          message = m.Message != null ? new
                                          {
                                              messageID = m.Message.MessageID,
                                              content = m.Message.Content,
                                              sentTime = m.Message.SentTime,
                                              attachment = m.Message.Medium != null ? m.Message.Medium.MediaID : 0,
                                              replyMessageID = m.Message.Message1 != null ? m.Message.ReplyMessageID : null,
                                          } : null
                                      })
                                      .Take(1)
                });
            });


            return Json(groups, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }


        [Route("~/api/groups/{groupID:int}/name/{newName}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditGroupName(int groupID, string newName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest();
            }

            Group group = await Task.Run(() =>
            {
                return db.Groups.Where((g) => g.GroupID == groupID).FirstOrDefault();
            });

            if (group == null) return NotFound();

            await db.EditGroupName(groupID, newName);

            return Ok();
        }

        [Route("~/api/groups/{groupID:int}/picture/{mediaID:int}/path/{path}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditGroupPicture(int groupID, int mediaID, string path)
        {
            var group = db.Groups.FirstOrDefaultAsync((g) => g.GroupID == groupID);
            if (group == null) return NotFound();

            if (!File.Exists(path)) return BadRequest();
            else
            {
                await db.EditGroupPicture(groupID, mediaID, path, path.Substring(path.Length - 1, path.LastIndexOf('/')));
                return Ok();
            }
        }

        [Route("~/api/groups/{groupID:int}/description/{description}")]
        [HttpPut]
        [HttpGet]
        public async Task<IHttpActionResult> EditGroupDescription(int groupID, string description)
        {
            var group = db.Groups.FirstOrDefaultAsync((g) => g.GroupID == groupID);
            if (group == null) return NotFound();

            await db.EditGroupDescription(groupID, description);
            return Ok();

        }

        [Route("~/api/groups/{userID:int}/with/{friendID:int}")]
        [ResponseType(typeof(Group))]
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> CreateConversation(int userID, int friendID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await db.Users.FirstOrDefaultAsync((u) => u.UserID == userID);
            if (user == null) NotFound();

            var friend = await db.Users.FirstOrDefaultAsync(u => u.UserID == friendID);
            if (friend == null) NotFound();

            await Task.Run(() => db.CreateAConversation(userID, friendID));

            var conversationFullInfo = await Task.Run(() =>
            (
            db.Participants
            .Where(u => u.UserID == userID)
            .Where(g => g.Group.GroupType == "CONVERSATION")

            .Intersect(
                db.Participants
               .Where(u => u.UserID == friendID)
               .Where(g => g.Group.GroupType == "CONVERSATION"))
            ).FirstOrDefaultAsync()
            );

            var conversationID = conversationFullInfo.GroupID;

            return Json<int?>(conversationID);
        }

        // POST: api/Groups
        [Route("~/api/groups/{groupName}/owner/{ownerID:int}")]
        [ResponseType(typeof(Group))]
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> CreateGroup(string groupName, int ownerID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(groupName))
            {
                return BadRequest();
            }

            var friend = await db.Users.FirstOrDefaultAsync(u => u.UserID == ownerID);
            if (friend == null) NotFound();

            var createTime = DateTime.Now;

            await db.CreateAGroup(ownerID, groupName);

            var group = await db.Groups.Where(info => info.Owner == ownerID &&
                                                        info.GroupName == groupName &&
                                                        info.CreateTime >= createTime).FirstOrDefaultAsync();

            return Json<int>(group.GroupID);
        }

        // DELETE: api/Groups/5
        [Route("~/api/groups/{groupID:int}/requester/{requesterID:int}")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> DeleteGroup(int groupID, int requesterID)
        {

            var requester = await db.Users.FirstOrDefaultAsync(u => u.UserID == requesterID);
            if (requester == null) return NotFound();

            var group = await db.Groups.FirstOrDefaultAsync(g => g.GroupID == groupID);
            if (group == null) return NotFound();

            if (group.GroupType == GroupType.GROUP.ToString())
                await db.DeleteAGroup(groupID, requesterID);
            else
                await db.DeleteAConversation(groupID, requesterID);

            return Ok();
        }

        [Route("~/api/groups/{groupID:int}/user/{requesterID:int}/add/{userID:int}")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> AddAUserToAGroup(int userID, int groupID, int requesterID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await db.Groups.FirstOrDefaultAsync(g => g.GroupID == groupID);
            if (group == null) return NotFound();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            if (user == null) return NotFound();

            await db.AddAUser(groupID, requesterID, userID);

            return Json<int>(groupID);
        }

        [Route("~/api/groups/{groupID:int}/admin/{userID:int}/owner/{ownerID:int}")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> AddAnAdmin(int userID, int ownerID, int groupID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = await db.Groups.FirstOrDefaultAsync(g => g.GroupID == groupID);
            if (group == null) return NotFound();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            if (user == null) return NotFound();

            await db.AddAnAdmin(groupID, ownerID, userID);

            return Ok();
        }

        [Route("~/api/groups/{groupID:int}/admin/{userID:int}/owner/{ownerID:int}")]
        [HttpDelete]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveAnAdmin(int groupID, int ownerID, int userID)
        {

            await db.RemoveAnAdmin(groupID, ownerID, userID);

            return Ok();
        }

        [Route("~/api/groups/{groupID:int}/remove/{userID:int}/owner/{ownerID:int}")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> RemoveAMembre(int groupID, int ownerID, int userID)
        {

            var group = await db.Groups.FirstOrDefaultAsync(g => g.GroupID == groupID);
            if (group == null) return NotFound();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            if (user == null) return NotFound();

            var owner = await db.Users.FirstOrDefaultAsync(u => u.UserID == ownerID);
            if (owner == null) return NotFound();

            await db.RemoveAUser(groupID, ownerID, userID);

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

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.GroupID == id) > 0;
        }

    }
}