using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System;
using System.Globalization;

public class SaveSystem : MonoBehaviour
{
    string _userFilePath;
    string _gameSettingsFilePath;
    string _listAttemptsFilePath;

    string _userJsonString;
    string _gameSettingsJsonString;
    string _listAttemptsJsonString;

    private void Awake() 
    {
        /* _userFilePath = Application.dataPath + "/User.json";
        _gameSettingsFilePath = Application.dataPath + "/GameSettings.json";
        _listAttemptsFilePath = Application.dataPath + "/ListAttempts.json"; */
    }

    #region Json Getters

    //Metodo para obtener el JSON de usuario
    public User GetUser()
    {
        _userFilePath = Application.dataPath + "/User.json";

        //si el archivo no existe, crea uno nuevo con valores por defecto
        if (!File.Exists(_userFilePath))
        {
            File.Create(_userFilePath).Dispose();
            _userJsonString = JsonUtility.ToJson(GetDefaultUser());
            File.WriteAllText(_userFilePath, _userJsonString);
        }

        _userJsonString = File.ReadAllText(_userFilePath);
        User _user = JsonUtility.FromJson<User>(_userJsonString);
        
        return _user;
    }

    public ListAttempts GetListAttempts()
    {
        _listAttemptsFilePath = Application.dataPath + "/ListAttempts.json";

        //si el archivo no existe, crea uno nuevo
        if (!File.Exists(_listAttemptsFilePath))
        {
            File.Create(_listAttemptsFilePath).Dispose();
            _listAttemptsJsonString = JsonUtility.ToJson(GetDefaultListAttempts());
            File.WriteAllText(_listAttemptsFilePath, _listAttemptsJsonString);
        }

        _listAttemptsJsonString = File.ReadAllText(_listAttemptsFilePath);
        ListAttempts listAttempts = JsonUtility.FromJson<ListAttempts>(_listAttemptsJsonString);
        
        return listAttempts;
    }

    //Metodo para obtener el JSON de configuraciones de juego
    public GameSettings GetGameSettings()
    {
        // Seteo el path del archivo
        _gameSettingsFilePath = Application.dataPath + "/GameSettings.json";

        //Chequeo si el archivo exite, si existe lo crea, sino sigue de largo
        if (!File.Exists(_gameSettingsFilePath))
        {
            //Creo el arhivo e invoco el método Dispose() para que me lo deje listo para modificar.
            File.Create(_gameSettingsFilePath).Dispose();

            //Como no puedo inicializar un JSON con variables vacias, le pido al método GetDefaultSettings() que me inicialice la clase/estructura con datos por default.
            _gameSettingsJsonString = JsonUtility.ToJson(GetDefaultSettings());

            //Escribo el archivo
            File.WriteAllText(_gameSettingsFilePath, _gameSettingsJsonString);
        }
        
        //Obtengo los datos del JSON y lo paso
        _gameSettingsJsonString = File.ReadAllText(_gameSettingsFilePath);
        var gameSettings = JsonUtility.FromJson<GameSettings>(_gameSettingsJsonString);

        return gameSettings;
    }
    #endregion
    
    #region Json Setters

    //Metodo para escribir el JSON de usuario
    public void SetUser(User user)
    {
        _userJsonString = JsonUtility.ToJson(user);
        File.WriteAllText(_userFilePath, _userJsonString);
    }

    public void SetListAttempts(ListAttempts listAttempts)
    {
        _listAttemptsJsonString = JsonUtility.ToJson(listAttempts);
        File.WriteAllText(_listAttemptsFilePath, _listAttemptsJsonString);
    }

    //Metodo para escribir el JSON de configuraciones de juego
    public void SetGameSettings(GameSettings gameSettings)
    {
        _gameSettingsJsonString = JsonUtility.ToJson(gameSettings);
        File.WriteAllText(_gameSettingsFilePath, _gameSettingsJsonString);
    }
    #endregion

    //=================== PRIVATE METHODS ========================

    //Metodo que devuelve un GameSettings con las variables por default
    private GameSettings GetDefaultSettings()
    {
        var gS = new GameSettings();
        gS.musicOn = true;
        gS.soundFXOn = true;

        return gS;
    }

    //Metodo que devuelve un User con las variables por default
    private User GetDefaultUser()
    {
        var user = new User();
        user.name = "Pepe";
        user.currentLevel = 01;
        user.experiencePoints = 0f;

        return user;
    }

    //Devuelve una lista vacía e inicializada de partidas
    private ListAttempts GetDefaultListAttempts()
    {
        var listAttempts = new ListAttempts();
        listAttempts.list = new List<Attempt>();

        return listAttempts;
    }

    public void DeleteListAttempts()
    {
        _listAttemptsFilePath = Application.dataPath + "/ListAttempts.json";

        //si el archivo existe, lo elimina
        if (File.Exists(_listAttemptsFilePath))
        {
            File.Delete(_listAttemptsFilePath);
        }
    }

}

[System.Serializable]
public struct User
{
    public string name;
    public int currentLevel;
    public float experiencePoints;

    public override string ToString()
    {
        return " Nombre: " + name + " - Nivel: " + currentLevel + "\n Puntos de experiencia: " + experiencePoints;
    }
}

[System.Serializable]
public struct GameSettings
{
    public bool musicOn;
    public bool soundFXOn;
}

[System.Serializable]
public struct JsonDateTime 
{
    public long value;

    public static implicit operator DateTime(JsonDateTime jdt) 
    {
        return DateTime.FromFileTimeUtc(jdt.value);
    }

    public static implicit operator JsonDateTime(DateTime dt) 
    {
        JsonDateTime jdt = new JsonDateTime();
        jdt.value = dt.ToFileTimeUtc();
        return jdt;
    }
}

[System.Serializable]
public struct Attempt
{ 
    //public int ID_Attempt; //ID: idJuego_idUsuario_123
    //public int ID_Game;
    //public int ID_User;
    public bool level_Completed; //result
    public int game_Level; //el nivel que jugó ahora -> a futuro, no siempre la escena va a corresponder a un nivel, pero en este caso sí 
    public float experience_Points_per_Attempt; //puntos de exp de la partida
    public int current_Game_Level; //el último nivel desbloqueado
    public int current_User_Level_In_The_Game; //nivel de experiencia de usuario -> cada 10 puntos cambia de nivel
    public int amount_of_Hits; //hacer metodo estatico para sumar 
    public int amount_of_Errors; //hacer metodo estatico para sumar
    public string attempt_Starting_Point; //fecha y hora de inicio de la partida
    public string attempt_End; //fecha y hora de fin de la partida
    public float attempt_Time; //(agregado, no estaba en la tabla) duración de la partida 
    public bool crashed; //(agregado, no estaba en la tabla) si la app crasheó o no
    public int where_the_Game_Stopped; //escena en la cual crasheó
    
    /*public override string ToString()
    {
        return "ID_Attempt: " + ID_Attempt + " | ID_Game: " + ID_Game + " | ID_User: " + ID_User + "\n" 
                    + "attempt_Starting_Point: " + attempt_Starting_Point + " | attempt_End: " + attempt_End + " | game_Level: " + game_Level + " | current_Game_Level: " + current_Game_Level;
    }*/
}
[System.Serializable]
public struct ListAttempts
{
    public List<Attempt> list;

    public void PrintList()
    {
        Debug.Log("--------------------- List Attempts: --------------------- \n");

        foreach (Attempt attempt in list)
        {
            Debug.Log(attempt + "\n");
        }

        Debug.Log("--------------------- End List --------------------- \n");
    }
}
