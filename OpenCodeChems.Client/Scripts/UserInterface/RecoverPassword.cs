using Godot;
using System;
using System.Text;

public class RecoverPassword : Control
{
    
    public override void _Ready()
    {
        var to = new {email = "miguelzinedinne@gmail.com"};
        GetNode("NinePatchRect/HTTPRequest").Connect("request_completed", this, "OnRequestCompleted");
    }

    public void _on_RecoverTextureButton_pressed()
    {
        HTTPRequest httpRequest = GetNode<HTTPRequest>("NinePatchRect/HTTPRequest");
        httpRequest.Request("http://www.mocky.io/v2/5185415ba171ea3a00704eed");
    }

    public void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        JSONParseResult json = JSON.Parse(Encoding.UTF8.GetString(body));
    }

    public void MakePostRequest(string url, object data_to_send, bool use_ssl)
    {
        string query = JSON.Print(data_to_send);
        HTTPRequest httpRequest = GetNode<HTTPRequest>("HTTPRequest");
        string[] headers = new string[] { "Content-Type: application/json" };
        httpRequest.Request(url, headers, use_ssl, HTTPClient.Method.Post, query);
    }
//  public override void _Process(float delta)
//  {
//      
//  }
}
