using System.Text.Json.Serialization;

namespace Thor.Abstractions.Dtos;

public class CasdoorUserDto
{
    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("sub")]
    public string Sub { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("data")]
    public CasdoorUserDataDto? Data { get; set; }
}

public class CasdoorUserDataDto
{
    public string owner { get; set; }
    public string name { get; set; }
    public string id { get; set; }
    public string externalId { get; set; }
    public string type { get; set; }
    public string displayName { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string avatar { get; set; }
    public string avatarType { get; set; }
    
    public string email { get; set; }
    public bool emailVerified { get; set; }
    public string phone { get; set; }
    
    public string location { get; set; }
    
    public string affiliation { get; set; }
    
    public string title { get; set; }
    public string idCardType { get; set; }
    public string idCard { get; set; }
    
    public string homepage { get; set; }
    
    public string bio { get; set; }
    
    public string tag { get; set; }
    
    public string language { get; set; }
    
    public string gender { get; set; }
    
    public string birthday { get; set; }
    
    public string education { get; set; }
    
    public int score { get; set; }
}

public class AccountItems
{
    public string name { get; set; }
    public bool visible { get; set; }
    public string viewRule { get; set; }
    public string modifyRule { get; set; }
    public string regex { get; set; }
}