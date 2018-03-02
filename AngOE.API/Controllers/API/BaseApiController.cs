using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngOE.Common;
using AngOE.Common.CodeSection;

namespace AngOE.API.Controllers.API
{
    public class BaseApiController : ApiController
    {
        internal LoginUserInformationForCodeSection CurrentUser { get; set; }

        /// <summary>
        /// Helper method to encapsulate exception handling for WebAPI
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected void Execute(Action code)
        {
            try
            {
                if (CurrentUser != null)
                {
                    using (var session = LoginUserSection.Start(CurrentUser))
                    {
                        ExecuteManager.Execute(code);
                    }
                }
                else
                {
                    ExecuteManager.Execute(code);
                }
            }
            catch (UnauthorizedException e)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message);
                throw new HttpResponseException(response);
            }
            catch (AngOeException e)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
                throw new HttpResponseException(response);
            }
            catch (Exception)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ErrorMessage.InternalServerError);
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Helper method to encapsulate exception handling for WebAPI
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        protected TResult Execute<TResult>(Func<TResult> code)
        {
            try
            {
                if (CurrentUser != null)
                {
                    using (var session = LoginUserSection.Start(CurrentUser))
                    {
                        return ExecuteManager.Execute(code);
                    }
                }
                else
                {
                    return ExecuteManager.Execute(code);
                }
            }
            catch (UnauthorizedException e)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e.Message);
                throw new HttpResponseException(response);
            }
            catch (AngOeException e)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
                throw new HttpResponseException(response);
            }
            catch (Exception)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ErrorMessage.InternalServerError);
                throw new HttpResponseException(response);
            }
        }
    }
}