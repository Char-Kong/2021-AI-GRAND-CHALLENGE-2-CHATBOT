using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using Syn.Bot.Siml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Message
{
    public string Text;
    public Text TextObject;
    public MessageType MessageType;
}

public enum MessageType
{
    User, Bot,
    Warning,
    Error
}

public class ChatManager : MonoBehaviour
{
    //OscovaBot MainBot;
    SimlBot ChatBot;
    TextToSpeech TTS;
    //LipSyncManager LSM;
    //Pronunciation_Kor_En PKE;

    List<Message> Messages = new List<Message>();

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public Color UserColor, BotColor;

    // Start is called before the first frame update
    void Start()
    {
       try
        {
            /* MainBot = new OscovaBot();
             OscovaBot.Logger.LogReceived += (s, o) =>
             {
                 Debug.Log($"OscovaBot: {o.Log}");
             };

             MainBot.Dialogs.Add(new BotDialog());
             MainBot.Trainer.StartTraining();

             MainBot.MainUser.ResponseReceived += (sender, evt) =>
             {
                 AddMessage($"Bot: {evt.Response.Text}", MessageType.Bot);
             };*/
            ChatBot = new SimlBot();
            ChatBot.PackageManager.LoadFromString(File.ReadAllText("Hello_Bot.simlpk"));
           
            SimlBot.Logger.LogReceived += (s, o) =>
            {
                Debug.Log("$SimlBot:{o.Log}");
            };
            TTS = GetComponent<TextToSpeech>();
           // LSM = this.transform.Find("Character1_Reference").gameObject.GetComponent<LipSyncManager>();
           // PKE = GetComponent<Pronunciation_Kor_En>();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToBot();
        }
    }

    public void AddMessage(string messageText, MessageType messageType)
    {
        if(Messages.Count >= 25)
        {
            Destroy(Messages[0].TextObject.gameObject);
            Messages.Remove(Messages[0]);
        }

        var newMessage = new Message { Text = messageText };

        var newText = Instantiate(textObject, chatPanel.transform);

        newMessage.TextObject = newText.GetComponent<Text>();
        newMessage.TextObject.text = messageText;
        newMessage.TextObject.color = messageType == MessageType.User ? UserColor : BotColor;

        Messages.Add(newMessage);
    }
    public void SendMessageToBot()
    {
        var userMessage = chatBox.text;

        if(!string.IsNullOrEmpty(userMessage))
        {
            Debug.Log($"SIMLBot: [USER] {userMessage}");
            AddMessage($"User: {userMessage}", MessageType.User);
            /* var request = MainBot.MainUser.CreateRequest(userMessage);
             var evaluationResult = MainBot.Evaluate(request);
             evaluationResult.Invoke();*/

            var result = ChatBot.Chat(userMessage);
            TTS.Init(result.BotMessage);
            AddMessage($"Bot:{result.BotMessage}",MessageType.Bot);
            //Debug.Log(result.BotMessage);
            //string res_str = PKE.InputString(result.BotMessage);
         
            //LSM.Input = res_str;
            //LSM.PutInputIntoQueue = true;
            


            chatBox.Select();
            chatBox.text = "";
        }
    }
}

/*public class BotDialog : Dialog
{
    [Expression("Hello Bot")]
    public void Hello(Context context, Result result)
    {
        result.SendResponse("Hello User!");
    }
}*/