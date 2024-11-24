﻿// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace idunno.Bluesky.Graph.Model
{
    internal sealed record GetListResponse
    {
        [JsonConstructor]
        public GetListResponse(ListView list, ICollection<ListItemView> items, string cursor)
        {
            List = list;
            Items = items;
            Cursor = cursor;
        }

        [JsonInclude]
        [JsonRequired]
        public ListView List { get; init; }

        [JsonInclude]
        [JsonRequired]
        public ICollection<ListItemView> Items { get; init; }

        [JsonInclude]
        public string? Cursor { get; init; }
    }
}