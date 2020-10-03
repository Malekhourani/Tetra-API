using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Tetra_API.Models;

namespace Tetra_API.Controllers
{
    public class MessagesController : ApiController
    {
        private Tetra db = new Tetra();

        // GET: api/Messages/5
        [ResponseType(typeof(Message))]
        [HttpGet]
        [Route("~/api/messages/{messageID:int}")]
        public async Task<IHttpActionResult> GetMessage(int messageID)
        {
            var messageInfo = await Task.Run(() => db.GroupMessages
                                               .Where(m => m.MessageID == messageID));

            if (messageInfo == null)
            {
                return NotFound();
            }

            var message = await messageInfo
                                .Select(m => new
                                {
                                    groupID = m.GroupID,
                                    senderID = m.SenderID,
                                    messageID = m.MessageID,
                                    message = new
                                    {
                                        messageID = (m.Message != null ? m.MessageID : 0),
                                        content = m.Message.Content,
                                        sentTime = m.Message.SentTime,
                                        attachment = (m.Message.Attachment != null ? m.Message.Attachment : null)
                                    }
                                }).SingleOrDefaultAsync();

            return Json(message);
        }


        // GET: api/messages/meh/user/1/group/1004/ /message/88
        [Route("~/api/messages/{content}/user/{senderID:int}/group/{groupID:int}/media/{attachmentPath}/message/replay/{messageID:int}")]
        [HttpPost]
        [HttpGet]
        public async Task<IHttpActionResult> SendAMessage(int groupID, int senderID, string content, string attachmentPath = null, int? messageToReplayID = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest();

            string attachmentName = string.Empty;
            if (!string.IsNullOrWhiteSpace(attachmentPath))
                attachmentName = attachmentPath.Substring(attachmentPath.LastIndexOf('/'), attachmentPath.Length - 1);

            var sentTime = DateTime.Now;
            await db.SendMessage(groupID, senderID, content, attachmentPath, attachmentName, messageToReplayID, sentTime);

            var messageID = await db.GroupMessages.Where(g => g.GroupID == groupID).Where(u => u.SenderID == senderID).Where(m => m.Message.SentTime == sentTime).FirstOrDefaultAsync();

            return Json(messageID);
        }


        [Route("~/api/messages/{messageID:int}/delete-by/{requesterID:int}")]
        [HttpDelete]
        [HttpGet]
        public async Task<IHttpActionResult> DeleteMessage(int requesterID, int messageID)
        {

            var sender = await db.Users.FirstOrDefaultAsync((u) => u.UserID == requesterID);
            if (sender == null) return NotFound();

            var message = await db.Messages.FirstOrDefaultAsync((m) => m.MessageID == messageID);
            if (message == null) return NotFound();

            await db.DeleteMessage(requesterID, messageID);

            return Ok();
        }



        [Route("~/api/messages/user/{userID:int}/date/{signOutDate:DateTime}")]
        public async Task<IHttpActionResult> getMyNewMessages(int userID, DateTime signOutDate)
        {
            var userGroups = await Task.Run(() => db.Participants.Where(u => u.UserID == userID).Select(g => g.GroupID));

            var NewMessages = db.GroupMessages
                              .Where(g => userGroups.Contains(g.GroupID))
                              .Where(m => m.Message.SentTime >= signOutDate)
                              .Select(gm => new
                              {
                                  groupID = gm.GroupID,
                                  messageID = gm.MessageID,
                                  senderID = gm.SenderID,
                                  message = new
                                  {
                                      messageID = gm.Message.MessageID,
                                      content = gm.Message.Content,
                                      attachment = gm.Message.Attachment,
                                      sentTime = gm.Message.SentTime,
                                      replyMessaegeID = gm.Message.ReplyMessageID
                                  }
                              });

            return Json(NewMessages);
        }


        [Route("~/api/messages/group/{groupID:int}/date/{lastMessageSendTime:DateTime}")]
        [HttpGet]
        public async Task<IHttpActionResult> loadMoreMessages(int groupID, DateTime lastMessageSendTime)
        {
            var Messages = await Task.Run(() => db.GroupMessages
                                                .Where(g => g.GroupID == groupID)
                                                .Where(ms => ms.Message.SentTime <= lastMessageSendTime)
                                                .Select(m => new
                                                {
                                                    groupID = m.Group != null ? m.GroupID : 0,
                                                    senderID = m.User != null ? m.SenderID : 0,
                                                    messageID = m.Message != null ? m.MessageID : 0,
                                                    message = m.Message != null ? new
                                                    {
                                                        messageID = m.MessageID,
                                                        content = m.Message.Content,
                                                        sentTime = m.Message.SentTime,
                                                        replyMessageID = m.Message.ReplyMessageID != null ? m.Message.ReplyMessageID : null,
                                                        attachment = m.Message.Medium != null ? m.Message.Attachment : 0
                                                    } : null
                                                })
                                                .Take(25));
            return Json(Messages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.MessageID == id) > 0;
        }

    }
}