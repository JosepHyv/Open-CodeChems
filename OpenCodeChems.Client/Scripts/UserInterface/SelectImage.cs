using Godot;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.UserInterface;
using System;
using System.Threading.Tasks;

namespace OpenCodeChems.Client.UserInterface
{
    public class SelectImage : Control
    {
        public string username = EditProfile.username;
        Network serverClient;
        private int imageProfile = 0;

        public override void _Ready()
        {
            serverClient = GetNode<Network>("/root/Network") as Network;
            serverClient.Connect("CorrectEditImageProfile", this, nameof(CorrectEditImageProfile));
            serverClient.Connect("EditImageProfileFail", this, nameof(IncorrectEditImageProfile));
        }

        public void _on_ChemsGamerTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_CHEMS_GAMER;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_ChemsChristmasTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_CHEMS_CHRISTMAS;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_DravenTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_DRAVEN;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_KittyMinecraftTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_KITTY_MINECRAFT;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_LinkTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_LINK;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_MechTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_MECH;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_MechChivaTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_MECH_CHIVA;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_ColdTeamTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_COLD_TEAM;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_WindowsTextureButton_pressed()
        {
            imageProfile = Constants.IMAGE_PROFILE_WINDOWS;
            serverClient.EditImageProfile(username, imageProfile);
        }
        public void _on_CancelTextureButton_pressed()
        {
            GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
        }
        public void CorrectEditImageProfile()
        {
            GetParent().GetNode<AcceptDialog>("SelectImage/SelectImageAcceptDialog").WindowTitle = ("NOTIFICATION");
            GetParent().GetNode<AcceptDialog>("SelectImage/SelectImageAcceptDialog").DialogText = ("CORRECT_IMAGE_PROFILE_UPDATE");
            GetParent().GetNode<AcceptDialog>("SelectImage/SelectImageAcceptDialog").Visible = true;
        }
        public void IncorrectEditImageProfile()
        {
            GetParent().GetNode<AcceptDialog>("SelectImage/SelectImageAcceptDialog").WindowTitle = ("ERROR");
            GetParent().GetNode<AcceptDialog>("SelectImage/SelectImageAcceptDialog").DialogText = ("ERROR_IMAGE_PROFILE_UPDATE");
            GetParent().GetNode<AcceptDialog>("SelectImage/SelectImageAcceptDialog").Visible = true;
        }
    }
}