﻿// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the MIT License.

namespace idunno.AtProto.Events
{
    /// <summary>
    /// A class holding information about a session that has just been refreshed.
    /// </summary>
    public sealed class SessionRefreshedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="SessionRefreshedEventArgs"/>
        /// </summary>
        /// <param name="did">The <see cref="Did"/> the session was created for.</param>
        /// <param name="service">The <see cref="Uri"/> of the service the session was created on.</param>
        /// <param name="accessJwt">The access token for the session.</param>
        /// <param name="refreshJwt">The refresh token for the session.</param>
        public SessionRefreshedEventArgs(Did did, Uri service, string accessJwt, string refreshJwt)
        {
            ArgumentNullException.ThrowIfNull(did);
            ArgumentNullException.ThrowIfNull(service);
            ArgumentNullException.ThrowIfNullOrEmpty(accessJwt);
            ArgumentNullException.ThrowIfNullOrEmpty(refreshJwt);

            Did = did;
            Service = service;

            AccessJwt = accessJwt;
            RefreshJwt = refreshJwt;
        }

        /// <summary>
        /// Gets the <see cref="Did"/> the session was created for.
        /// </summary>
        /// <value>The <see cref="Did"/> the session was created for.</value>
        public Did Did { get; }

        /// <summary>
        /// Gets the <see cref="Uri"/> of the service the session was created on.
        /// </summary>
        /// <value>The <see cref="Uri"/> of the service the session was created on.</value>
        public Uri Service { get; }

        /// <summary>
        /// Gets a string representation of the access token for the session.
        /// </summary>
        /// <value>
        /// A string representation of the access token for the session.
        /// </value>
        public string AccessJwt { get; }

        /// <summary>
        /// Gets a string representation of the refresh token for the session.
        /// </summary>
        /// <value>
        /// A string representation of the refresh token for the session.
        /// </value>
        public string RefreshJwt { get; }

    }
}