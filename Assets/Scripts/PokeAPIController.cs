using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using System;

public class Pokemon
{
    public Texture image;
    public string name;
    public int index, height, healthPoints, speed, attack, defence, specialAttack, specialDefence, weight;
    public string[] Types;

    public Pokemon (Texture image, int index, string name, int height, int weight, int healthPoints, int speed, int attack, int defence, int specialAttack, int specialDefence, string[] types)
    {
        this.image = image;
        this.index = index;
        this.name = name;
        this.height = height;
        this.weight = weight;
        this.healthPoints = healthPoints;
        this.speed = speed;
        this.attack = attack;
        this.defence = defence;
        this.specialAttack = specialAttack;
        this.specialDefence = specialDefence;
        Types = types;
    }
}
public class PokeAPIController : MonoBehaviour
{
    private string pokeName;
    private int pokeHeight, pokeWeight, pokeHp, pokeSpd, pokeAtt, pokeDef, pokeSAtt, pokeSDef;
    public int PokeIndex = 1;
    public RawImage pokeRawImage;
    [SerializeField]
    private GameObject pokeCounter, gridController, downloadingPokemon;
    [SerializeField]
    private Button back, update;
    [HideInInspector]
    public Pokemon[] Pokemons = new Pokemon[1];
    private readonly string basePokeURL = "https://pokeapi.co/api/v2/";

    public void OnButtonUpdatePokemonList()
    {
        //Main coroutine for pokemons download
        StartCoroutine(GetPokemonAtIndex(PokeIndex));

        IEnumerator GetPokemonAtIndex(int pokemonindex)
        {
            //Get Pokemon info from web
            string pokemonURL = basePokeURL + "pokemon/" + pokemonindex.ToString();
            UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);
            yield return pokeInfoRequest.SendWebRequest();

            //Continuation of coroutine after getting all pokemons
            if (pokeInfoRequest.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(Next());

                yield break;
            }

            //Get selected info from JSON
            JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);
            
            string pokeSpriteURL = pokeInfo["sprites"]["front_default"];
            pokeName = pokeInfo["name"];
            pokeHeight = pokeInfo["height"];
            pokeWeight = pokeInfo["weight"];
            pokeHp = pokeInfo["stats"][0]["base_stat"];
            pokeSpd = pokeInfo["stats"][1]["base_stat"];
            pokeAtt = pokeInfo["stats"][2]["base_stat"];
            pokeDef = pokeInfo["stats"][3]["base_stat"];
            pokeSAtt = pokeInfo["stats"][4]["base_stat"];
            pokeSDef = pokeInfo["stats"][5]["base_stat"];
            
            JSONNode pokeTypes = pokeInfo["types"];
            string[] pokeTypenames = new string[pokeTypes.Count];
            for (int i = 0, j = pokeTypes.Count - 1; i < pokeTypes.Count; i++, j--)
            {
                pokeTypenames[i] = pokeTypes[i]["type"]["name"];
            }

            //Get Pokemon Sprite
            UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(pokeSpriteURL);
            yield return pokeSpriteRequest.SendWebRequest();
            if (pokeSpriteRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(pokeSpriteRequest.error);
                yield break;
            }

            //Set UI Objets
            pokeRawImage.texture = DownloadHandlerTexture.GetContent(pokeSpriteRequest);
            pokeRawImage.texture.filterMode = FilterMode.Point;

            //Capitalize Firzt Letters
            pokeName = CapitalizeFirstLetter(pokeName);
            for (int i = 0; i < pokeTypenames.Length; i++)
            {
                pokeTypenames[i] = CapitalizeFirstLetter(pokeTypenames[i]);
            }

            //Resizing pokemons array into needed sizes
            Array.Resize (ref Pokemons, PokeIndex);
            Pokemons[PokeIndex-1] = new Pokemon (pokeRawImage.texture, PokeIndex, pokeName, pokeHeight, pokeWeight, pokeHp, pokeSpd, pokeAtt, pokeDef, pokeSAtt, pokeSDef, pokeTypenames);
            
            //Displayed message for user about update progress
            pokeCounter.GetComponent<TextMeshProUGUI>().text = "Pokemons updated: " + PokeIndex;

            //Next pokemon
            PokeIndex++;

            //Start this coroutine again with next pokemon
            StartCoroutine(GetPokemonAtIndex(PokeIndex)); 
        }

        //Capitalize first letter method
        string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
    //Coroutine to be called after all pokemons downloaded, 3s delay for previous coroutine to finish
    IEnumerator Next(){
        
        pokeCounter.SetActive(false);
        downloadingPokemon.GetComponent<TextMeshProUGUI>().text = "Pokemons Updated";
        yield return new WaitForSeconds (3);
        downloadingPokemon.SetActive(false);
        gridController.GetComponent<GridController>().GridCreator(PokeIndex-1);
        back.interactable = true;
        update.interactable = true;
        yield break;
    }
}
