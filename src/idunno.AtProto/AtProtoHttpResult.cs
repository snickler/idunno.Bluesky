﻿// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the MIT License.

using System.Net;

namespace idunno.AtProto
{
    /// <summary>
    /// Wraps the result from an AT Proto API or Bluesky API call into a single object that holds both the result, if any,
    /// and any error details returned by the API.
    /// </summary>
    /// <typeparam name="TResult">The type the results should be deserialized into.</typeparam>
    public record AtProtoHttpResult<TResult>
    {
        internal AtProtoHttpResult()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="AtProtoHttpResult{TResult}"/>.
        /// </summary>
        /// <param name="statusCode">The underlying HTTP status code returned by the API call.</param>
        /// <param name="result">The resulting object of type <typeparamref name="TResult"/> returned by the API call, if any.</param>
        /// <param name="atErrorDetail">The <see cref="AtErrorDetail"/> returned by the API call, if any.</param>
        public AtProtoHttpResult(HttpStatusCode statusCode, TResult? result, AtErrorDetail? atErrorDetail = null)
        {
            StatusCode = statusCode;
            Result = result;
            AtErrorDetail = atErrorDetail;
        }

        /// <summary>
        /// Gets the <see cref="HttpStatusCode" /> associated with the response to an AT Proto or Bluesky API request.
        /// </summary>
        /// <value>
        /// The <see cref="HttpStatusCode" /> associated with the response to an AT Proto or Bluesky API request.
        /// </value>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// The extended error information, if any, if the request was unsuccessful.
        /// </summary>
        public AtErrorDetail? AtErrorDetail {get; internal set; }

        /// <summary>
        ///The result of an HttpRequest, if the request has was successful.
        /// </summary>
        public TResult? Result { get; internal set; }

        /// <summary>
        /// A flag indicating if the https request returned a status code of OK.
        /// </summary>
        public bool Succeeded
        {
            get
            {
                return StatusCode == HttpStatusCode.OK;
            }
        }

        /// <summary>
        /// True if <paramref name="r"/> is not null, the http status code is OK, and the request had a result, otherwise false.
        /// </summary>
        /// <param name="r">The <see cref="AtProtoHttpResult{TResult}"/> to check.</param>
        public static implicit operator bool(AtProtoHttpResult<TResult> r)
        {
            return
                r is not null &&
                r.StatusCode == HttpStatusCode.OK &&
                r.Result is not null;
        }

        /// <summary>
        /// Converts this instance to a boolean, indicating the success or failure of the request that generated this instance.
        /// </summary>
        /// <returns>
        /// True if this <see cref="AtProtoHttpResult{TResult}" /> is not null, the http status code returned by the API call is OK,
        /// and the request had a result, otherwise false.
        /// </returns>
        public bool ToBoolean()
        {
            return (bool)this;
        }
    }
}