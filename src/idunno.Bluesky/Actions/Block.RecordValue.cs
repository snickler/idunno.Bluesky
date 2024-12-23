﻿// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

using idunno.AtProto;
using idunno.Bluesky.Record;

namespace idunno.Bluesky.Actions
{
    /// <summary>
    /// Encapsulates the information needed to create a block record.
    /// </summary>
    public sealed record BlockRecordValue : BlueskyRecordValue
    {
        /// <summary>
        /// Creates a new instance of <see cref="BlockRecordValue"/> with <see cref="BlueskyRecordValue.CreatedAt"/> set to the current date and time.
        /// </summary>
        /// <param name="subject">The <see cref="Did"/> to the actor to be blocked.</param>
        public BlockRecordValue(Did subject) : this(subject, DateTimeOffset.UtcNow)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="BlockRecordValue"/>.
        /// </summary>
        /// <param name="subject">The <see cref="Did"/> to the actor to be blocked.</param>
        /// <param name="createdAt">An optional <see cref="DateTimeOffset"/> for the repost creation date, defaults to now.</param>
        [JsonConstructor]
        public BlockRecordValue(Did subject, DateTimeOffset? createdAt) : base(createdAt)
        {
            ArgumentNullException.ThrowIfNull(subject);

            Subject = subject;
        }

        /// <summary>
        /// The record type for a block record.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("$type")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Cannot be static as it won't be serialized.")]
        public string Type => RecordType.Block;

        /// <summary>
        /// Gets the <see cref="Did"/> of the subject to be blocked.
        /// </summary>
        [JsonInclude]
        public Did Subject { get; init; }
    }
}