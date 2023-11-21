using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Aether.Identity.Model
{
    public class TokenGenerationRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public Dictionary<string, JsonElement> CustomClaims { get; set; }
    }
}
