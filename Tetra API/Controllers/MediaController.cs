using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tetra_API.Models;

namespace Tetra_API.Controllers
{
    public class MediaController : ApiController
    {
        private Tetra db = new Tetra();

        [Route("~/api/media")]
        public async Task<IHttpActionResult> getMaxMediaID()
        {
            var MaxMediaID = await Task.Run(() => db.Media.LastOrDefault().MediaID);

            if (MaxMediaID != 0)
                return Json<int>(MaxMediaID);

            return Json<int>(1);
        }

        [Route("~/api/media/{mediaID:int}")]
        public async Task<IHttpActionResult> getMediaInfo(int mediaID)
        {
            var mediaInfo = await Task.Run(() => db.Media.Where(m => m.MediaID == mediaID).SingleOrDefault());

            var media = new Medium()
            {
                MediaID = (mediaInfo == null) ? 0 : mediaID,
                MediaName = mediaInfo.MediaName,
                Path = mediaInfo.Path
            };

            if (mediaInfo == null) return BadRequest();

            return Json(media);
        }

        [Route("~/api/media/{mediaID:int}/name/{name}/path/{path}")]
        [HttpPost]
        public void SaveMedia(int mediaID, string name, string path)
        {
            Task.Run(() => db.SaveMedia(mediaID, path, name));
        }

        //[Route("~/api/media/{mediaID:int}/group/{groupID:int}")]
        //[HttpPost]
        //public async void ConnectMediaToGroup(int groupID, int mediaID)
        //{
        //    db.Groups.Where(g => g.GroupID == groupID).SingleOrDefault().GroupPicture = mediaID;
        //    await db.SaveChangesAsync();
        //}

        //[Route("~/api/media/{mediaID:int}/user/{userID:int}")]
        //[HttpPost]
        //public async void ConnectMediaToUser(int userID, int mediaID)
        //{
        //    db.Users.Where(u => u.UserID == userID).SingleOrDefault().ProfilePicture = mediaID;
        //    await db.SaveChangesAsync();
        //}

        //[Route("~/api/media/{mediaID:int}/message/{messageID:int}")]
        //[HttpPost]
        //public async void ConnectMediaToMessage(int messageID, int mediaID)
        //{
        //    db.Messages.Where(m => m.MessageID == messageID).SingleOrDefault().Attachment = mediaID;
        //    await db.SaveChangesAsync();
        //}

        [Route("~/api/media/{mediaID:int}/path")]
        public async Task<IHttpActionResult> getMediaPath(int mediaID)
        {
            var mediaInfo = await Task.Run(() => db.Media.Where(m => m.MediaID == mediaID).SingleOrDefault());

            if (mediaInfo == null) return BadRequest();

            return Json<string>(mediaInfo.Path);
        }

    }
}
