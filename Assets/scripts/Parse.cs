using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parse{

    public enum MessageType
    {
        Symbol, Message, GameState, UnKnown
    }

	public static MessageType GetType(string parseString)
    {
        string[] strings = parseString.Split( ':');

        switch (strings[0])

        {
            case "s":
                return MessageType.Symbol;
            case "m":
                return MessageType.Message;
            case "g":
                return MessageType.GameState;
            default:
                return MessageType.UnKnown;
        }
    }

    public static string GetMessage(string parseString)
    {
        try
        {
            string[] strings = parseString.Split(':');
            return strings[1];
        }
        catch (System.Exception){
            return "";
        }
    }

}
