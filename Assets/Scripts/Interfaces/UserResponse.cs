using System.Runtime.CompilerServices;

[System.Serializable]
public class UserResponse
{
    public string id { get; set; }
    public string username { get; set; }
    public string avatar { get; set; }
    public string regEmail { get; set; }
    public object regDate { get; set; }
    public string status { get; set; }
    public object birthDate { get; set; }
}
