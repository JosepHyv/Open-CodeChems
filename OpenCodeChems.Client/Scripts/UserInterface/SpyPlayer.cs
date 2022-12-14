using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.Resources;


namespace OpenCodeChems.Client.UserInterface
{

  public class SpyPlayer : Control
  {
  
	private LineEdit ChatLineEdit; 
	private RichTextLabel turnIndicator;
	private TextEdit ChatBlock;
	Network serverClient;
  private readonly Image civilYellow = new Image();
  private readonly Image agentTypeRed = new Image();
  private readonly Image agentTypeBlue = new Image();
  private readonly Image assassinBlack = new Image();
  private readonly string PATH_CIVIL_COLOR = "Scenes/Resources/Icons/square-64.png";
  private readonly ImageTexture textureCivil = new ImageTexture();
  private readonly string PATH_ASSASSIN_COLOR = "Scenes/Resources/Icons/ssquareBlack.png";
  private readonly ImageTexture textureAssassin = new ImageTexture();
	private string PATH_RED_COLOR = "Scenes/Resources/Icons/squareRed.png";
	private ImageTexture textureRed = new ImageTexture();
	private string PATH_BLUE_COLOR = "Scenes/Resources/Icons/squareBlue.png";
	private ImageTexture textureBlue = new ImageTexture();
	private int SelectedIndex = Constants.NULL_INDEX;
	private ConfirmationDialog turnDialog;

	private AcceptDialog notification;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChatLineEdit = GetParent().GetNode<LineEdit>("SpyPlayer/ChatLineEdit");
		ChatBlock = GetParent().GetNode<TextEdit>("SpyPlayer/ChatTextEdit");
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("UpdateChatLog", this, nameof(AadToChat));
		serverClient.Connect("ChangeClue", this, nameof(ClueChange));
		serverClient.Connect("UpdateCard",this, nameof(ChangeColor));
		serverClient.Connect("AnswerValue", this, nameof(ValidatedAnswer));
		turnDialog = GetParent().GetNode<ConfirmationDialog>("SpyPlayer/TurnConfirmationDialog");
		turnDialog.GetCancel().Connect("pressed", this, nameof(TurnCancelDialog));
		serverClient.Connect("FinishGame", this, nameof(FinishMessage));
		notification = GetParent().GetNode<AcceptDialog>("SpyPlayer/AnswareAcceptDialog");
		turnIndicator = GetParent().GetNode<RichTextLabel>("SpyPlayer/BackGroundNinePatchRect/TurnRichTextLabel");
		
		List<string> listElements = serverClient.boardWords;
		
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		for(int c = 0 ; c<listElements.Count && c < 25; c++)
		{
			itemNode.SetItemText(c, listElements[c]);					
		}

		serverClient.Connect("CleanRoom", this, nameof(ChangeToMainMenu));
		serverClient.Connect("WriteTurnIndicator", this, nameof(UpdateTurnIndicator));
			

	}

	public void FinishMessage(bool status)
	{
		if(status)
		{
			GetTree().ChangeScene("res://Scenes/VictoryScreen.tscn");
		}
		else
		{
			GetTree().ChangeScene("res://Scenes/DefeatScreen.tscn");
		}
	}
	public void ChangeToMainMenu()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}

	public void _on_ChatTextureButton_pressed()
	{
		string message = ChatLineEdit.Text;
		if(!String.IsNullOrWhiteSpace(message))
		{
			ChatLineEdit.Clear();
			serverClient.ChatInGame(message);
		}
	}
	public void AadToChat(string message)
	{
		ChatBlock.InsertTextAtCursor($"{message}\n");
	}
	public void _on_CodeNamesItemList_item_selected(int index)
	{
		
		SelectedIndex = index;
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		var  selectedCard = GetParent().GetNode<RichTextLabel>("SpyPlayer/BackGroundNinePatchRect/SelectedCardRichTextLabel");
		selectedCard.Clear();
		selectedCard.AddText(itemNode.GetItemText(SelectedIndex));
			
	}


	public void ClueChange(string clue)
	{
		var clueContainer = GetParent().GetNode<RichTextLabel>("SpyPlayer/BackGroundNinePatchRect/KeyNumberRichTextLabel");
		clueContainer.Clear();
		clueContainer.AddText(clue);
	}

	public void _on_SendWordTextureButton_pressed()
	{
		if(SelectedIndex != Constants.NULL_INDEX) 
		{
			serverClient.VerifySelectedCard(SelectedIndex);
		}
	}

    [Obsolete]
    public void ChangeColor(int color, int index)
	{
		if(color == 0)
		{
			ChangeBlue(index);
		}
		else if(color == 1)
		{
			ChangeRed(index);
		}
		else if(color == 2)
		{
			changeYellow(index);
		}
		else if(color == 3)
		{
			ChangeBlack(index);
		}
		
	}

	public void ValidatedAnswer(bool guessAnswer)
	{
		if(guessAnswer)
		{
			turnDialog.DialogText = "RIGHT_ANSWER";
			turnDialog.Visible = true;
		}
		else
		{
			notification.DialogText = "WRONG_ANSWER";
			notification.Visible = true;
			serverClient.skipTurn();
		}
	}
	public void TurnAcceptDialog()
	{
		serverClient.keepTurn();
	}
	public void TurnCancelDialog()
	{
		serverClient.skipTurn();
		turnDialog.Visible = false;
	}
	public void ChangeBlack(int index)
	{	
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		assassinBlack.Load(PATH_ASSASSIN_COLOR);
		textureAssassin.CreateFromImage(assassinBlack);
		itemNode.SetItemIcon(index, textureAssassin);
	}
	public void ChangeBlue(int index)
	{
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		agentTypeBlue.Load(PATH_BLUE_COLOR);
		textureBlue.CreateFromImage(agentTypeBlue);
		itemNode.SetItemIcon(index, textureBlue);
	}
	public void ChangeRed(int index)
	{
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		agentTypeRed.Load(PATH_RED_COLOR);
		textureRed.CreateFromImage(agentTypeRed);
		itemNode.SetItemIcon(index, textureRed);
	}
	public void changeYellow(int index)
	{
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		civilYellow.Load(PATH_CIVIL_COLOR);
		textureCivil.CreateFromImage(civilYellow);
		itemNode.SetItemIcon(index, textureCivil);
	}


	public void _on_LeaveGameTextureButton_pressed()
	{
		serverClient.LeftRoom();
		ChangeToMainMenu();
	}
	private void UpdateTurnIndicator(string turnName)
	{
		turnIndicator.Text = turnName;
	}
 }
}