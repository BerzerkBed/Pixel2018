using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class FlumpImporter : MonoBehaviour {

	public TextAsset[] m_JsonToLoad;

	private void Start()
	{
		LoadJson();
	}
	
	private void LoadJson()
	{
		foreach(TextAsset t in m_JsonToLoad)
		{
			/*string fullJson = "";
			using(StreamReader sr = f.OpenText())
			{
				string s = "";
				while((s = sr.ReadLine()) != null)
				{
					fullJson += s;
				}
			}*/
			/*m_JsonToLoad
			CreateAnimationsFromJSON(fullJson);*/
			CreateAnimationsFromJSON(t.text);
		}
	}

	private void CreateAnimationsFromJSON(string i_Json)
	{
		JSONObject json = new JSONObject(i_Json);

		Debug.Log(json.Print());

		int frameRate = 0;
		if(json.HasField("frameRate"))		
		{
			frameRate = (int)json.GetField("frameRate").i;
		}

		List<JSONObject> textureGroups = json.GetField("textureGroups").list;
		for(int i = 0 ; i < textureGroups.Count ; i++)
		{
			List<JSONObject> atlases = textureGroups[i].GetField("atlases").list;
			for(int j = 0 ; j < atlases.Count ; j++)
			{
				//string file = atlases[j].GetField("file").str;

				//byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/sprite.png");
				//byte[] atlas = GetAtlas(file.Split(',')[0]);// file.Split(",", 1)[0]);
				//Texture2D atlasTexture = new Texture2D(0,0);	
				//atlasTexture.LoadImage(atlas);
				//int atlasHeight = atlasTexture.height;

				List<JSONObject> textures = atlases[j].GetField("textures").list;
				for(int k = 0 ; k < textures.Count ; k++)
				{
					string symbol = textures[k].GetField("symbol").str;
					if(!FlumpSpriteManager.Instance.AlreadyCreated(symbol))
					{
						List<JSONObject> origin = textures[k].GetField("origin").list;
						List<JSONObject> rect = textures[k].GetField("rect").list;

						float x = (origin[0].f / 64f);
						float y = (origin[1].f / 64f);

						//int getX = (int)rect[0].i;
						//int getY = (int)rect[1].i;
						int width = (int)rect[2].i;
						int height = (int)rect[3].i;

						float offsetX = ((width / 64f) * 0.5f) - x;
						float offsetY = y - ((height / 64f) * 0.5f);

						FlumpSpriteManager.Instance.CreateSprite(offsetX , offsetY , symbol);
					}
				}
			}
		}

		List<JSONObject> moviesGroup = json.GetField("movies").list;
		for(int i = 0 ; i < moviesGroup.Count ; i++)
		{
			JSONObject id = moviesGroup[i].GetField("id");
			string movieName = "Error_NoName";
			if(id != null)
			{
				movieName = id.str;
			}

			if(!FlumpMovieManager.Instance.AlreadyCreated(movieName))
			{
				List<FlumpMovie.Layer> movieLayers = new List<FlumpMovie.Layer> ();

				List<JSONObject> layers = moviesGroup[i].GetField("layers").list;
				for(int j = 0 ; j < layers.Count ; j++)
				{
					//string layer = layers[j].GetField("name").str;

					List<FlumpMovie.KeyFrame> keyframeList = new List<FlumpMovie.KeyFrame>();

					List<JSONObject> keyframes = layers[j].GetField("keyframes").list;
					for(int k = 0 ; k < keyframes.Count ; k++)
					{
						FlumpMovie.KeyFrame keyFrame = new FlumpMovie.KeyFrame();

						JSONObject key = keyframes[k];
						JSONObject pivot = key.GetField("pivot");
						if(pivot != null)
						{
							keyFrame.m_Pivot = new Vector2 (pivot.list [0].f, -pivot.list [1].f) * (1 / 64f);//0.01f;
						}

						JSONObject loc = key.GetField("loc");
						if(loc != null)
						{
							keyFrame.m_Position = new Vector2 (loc.list [0].f, -loc.list [1].f) * (1 / 64f);//0.01f;
						}

						JSONObject skew = key.GetField("skew");
						if(skew != null)
						{
							keyFrame.m_Rotation = new Vector2(skew.list[0].f , skew.list[1].f);
						}

						JSONObject ease = key.GetField("ease");
						if(ease != null)
						{
							keyFrame.m_Ease = int.Parse(ease.Print());
						}

						JSONObject ḑuration = key.GetField("duration");
						if(ḑuration != null)
						{
							keyFrame.m_Duration = int.Parse(ḑuration.Print());
						}

						JSONObject index = key.GetField("index");
						if(index != null)
						{
							keyFrame.m_Index = int.Parse(index.Print());
						}

						JSONObject reference = key.GetField("ref");
						if(reference != null)
						{
							keyFrame.m_Ref = reference.str;
						}

						JSONObject tweened = key.GetField("tweened");
						if(tweened != null)
						{
							keyFrame.m_Tweened = bool.Parse(tweened.Print());
						}

						JSONObject scale = key.GetField ("scale");
						if (scale != null) 
						{
							keyFrame.m_Scale = new Vector3(scale.list[0].f , scale.list[1].f , 1f);
						}

						keyframeList.Add(keyFrame);
					}
					movieLayers.Add(new FlumpMovie.Layer(keyframeList));
				}
				FlumpMovieManager.Instance.CreateMovie(movieLayers , frameRate , movieName);
			}
		}
	}
}