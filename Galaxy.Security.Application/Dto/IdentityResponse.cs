﻿namespace Galaxy.Security.Application.Dto
{
    public class IdentityResponse
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();
    }
}
