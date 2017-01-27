using System;
using System.Web.Http;

namespace Votify.Rocks.Service.WebApi
{
    [RoutePrefix("")]
    public class EmailController : ApiController
    {
        private readonly ISendMailService _sendMailService;

        public EmailController(ISendMailService sendMailService)
        {
            if (sendMailService == null) throw new ArgumentNullException(nameof(sendMailService));
            _sendMailService = sendMailService;
        }

        /// <summary>
        /// Share vote session details via email
        /// </summary>
        /// <param name="sessionKey">The key of the session you want share</param>
        /// <param name="email">Email address to share to</param>
        [Route("{sessionKey}/Share")]
        [HttpPost]
        public void ShareSessionViaEmail(string sessionKey, string email)
        {
            _sendMailService.ShareSessionViaEmail(email, sessionKey);
        }
    }
}
