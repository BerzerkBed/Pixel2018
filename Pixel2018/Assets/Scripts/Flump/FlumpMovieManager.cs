using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlumpMovieManager : MonoBehaviour {

	private static FlumpMovieManager m_Instance;
	public static FlumpMovieManager Instance
	{
		get{return m_Instance;}
	}

	void Awake()
	{
		//DontDestroyOnLoad (this);
		m_Instance = this;	
	}

	public List<FlumpMovie> m_FlumpMovieList;

	public GameObject m_FlumpMoviePrefab;

	public bool AlreadyCreated(string i_MovieName)
	{
		for(int i = 0 ; i < m_FlumpMovieList.Count ; i++)
		{
			if(m_FlumpMovieList[i].m_MovieName == i_MovieName)
			{
				return true;
			}
		}

		return false;
	}

	public FlumpMovie CreateMovie(List<FlumpMovie.Layer> i_Calques , int i_FrameRate , string i_MovieName)
	{
		GameObject go = (GameObject)GameObject.Instantiate(m_FlumpMoviePrefab);
		FlumpMovie flumpMovie = go.GetComponent<FlumpMovie>();
		flumpMovie.SetData(i_Calques , i_FrameRate , i_MovieName);
		go.transform.parent = gameObject.transform;
		go.SetActive(false);

		m_FlumpMovieList.Add(flumpMovie);

		return flumpMovie;
	}

	public FlumpMovie GetMovie(string i_Name)
	{
		FlumpMovie reference = null;
		for(int i = 0 ; i < m_FlumpMovieList.Count ; i++)
		{
			if(m_FlumpMovieList[i].m_MovieName == i_Name)
			{
				reference = m_FlumpMovieList[i];
				if(m_FlumpMovieList[i].m_Available)
				{
					FlumpMovie returnMovie = m_FlumpMovieList[i];
					return returnMovie;
				}
			}
		}

		if(reference != null)
		{	
			GameObject go = (GameObject)GameObject.Instantiate(m_FlumpMoviePrefab);
			FlumpMovie flumpMovie = go.GetComponent<FlumpMovie>();
			flumpMovie.CopyData(reference);
			go.transform.parent = gameObject.transform;
			go.SetActive(false);
		//	m_FlumpMovieList.Add(flumpMovie);

			return flumpMovie;
		}

		return null;
	}

	public FlumpMovie AddMovie(string i_MovieName , float i_PosX , float i_PosY)
	{
		FlumpMovie movie = GetMovie(i_MovieName);
		if(movie != null)
		{
			movie.gameObject.SetActive(true);
			movie.gameObject.transform.position = Vector2.zero;
			movie.gameObject.transform.SetParent (transform);
			movie.m_Available = false;
			return movie;
		}

		Debug.Log ("Couldn't find Movie : " + i_MovieName);
		return null;
	}

	public void ReturnMovie(FlumpMovie i_Movie)
	{
		i_Movie.ReturnMovies ();
		//m_FlumpMovieList.Add(i_Movie);
		i_Movie.gameObject.SetActive(false);
		i_Movie.transform.SetParent (transform);
		i_Movie.m_Available = true;
	}

	public void MakeMovieAvailable(FlumpMovie i_Movie)
	{
		//m_FlumpMovieList.Add(i_Movie);
		i_Movie.gameObject.SetActive(false);
		i_Movie.transform.SetParent (transform);
		i_Movie.m_Available = true;
	}
}