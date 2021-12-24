using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;

[System.Serializable]
public class SetTextToSpeech
{
	public SetInput input;
	public SetVoice voice;
	public SetAudioConfig audioConfig;
}

[System.Serializable]
public class SetAudioConfig
{
	public string audioEncoding;
	public float speakingRate;
	public int pitch;
	public int volumeGainDb;

	//순서대로 오디오 인코딩 방식, 말하는 속도, 피치, 데시벨 설정
}

[System.Serializable]
public class SetVoice
{
	public string languageCode;
	public string name;
	public string ssmlGender;

	//languageCode = 영어 : en-US, 한국어 ko-KR, 등등 지정 가능
	//name = 목소리 버전, ko-KR-Wavenet-A~D 네가지 버전 존재.
	//ssmlGender =목소리 성별
}

[System.Serializable]
public class SetInput
{
	public string text;

	//string 데이터 전송.
}
[System.Serializable]
public class GetContent
{
	public string audioContent;

	//통신이 잘 이루어졌다면, audioContent로 내용을 받게 된다.
	//base64로 인코딩된 string 값, 따라서 다시 디코딩을 진행 해주어야한다./
}

public class TextToSpeech : MonoBehaviour
{
	private string apiURL = "https://texttospeech.googleapis.com/v1beta1/text:synthesize?key=AIzaSyDAA11HYeWrjMU5JIt5Ok9M0khCCv5KYTY";
	SetTextToSpeech tts = new SetTextToSpeech();
	public AudioSource audioSource_sound = null;
	public AudioSource audioSource_sync = null;
	
	//Start is called before the first frame update

	void Start()
	{
		audioSource_sync = this.transform.Find("unitychan").gameObject.GetComponent<AudioSource>();
	}

	//Update is called once per frame

	void Update()
	{

	}

	public void Init(string text)
	{
		SetInput si = new SetInput();
		si.text = text;
		tts.input = si;

		SetVoice sv = new SetVoice();
		sv.languageCode = "ko-KR";
		sv.name = "ko-KR-Standard-A";
		sv.ssmlGender = "FEMALE";
		tts.voice = sv;

		SetAudioConfig sa = new SetAudioConfig();
		sa.audioEncoding = "LINEAR16";
		sa.speakingRate = 0.5f;
		sa.pitch = (int).1;
		sa.volumeGainDb = 3;
		tts.audioConfig = sa;

		CreateAudio();
	}

	private void CreateAudio()
	{

		//str에 base64 형태로 인코딩된 파일 저장.
		var str = TextToSpeechPost(tts);
		GetContent info = JsonUtility.FromJson<GetContent>(str);

		//bytes로 디코딩
		var bytes = Convert.FromBase64String(info.audioContent);

		//byte array to float array
		var f = ConvertByteToFloat(bytes);

		AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
		audioClip.SetData(f, 0);

		AudioSource audioSource = GameObject.FindObjectOfType<AudioSource>();
		audioSource.PlayOneShot(audioClip);
		//if((audioSource_sound != null)&&(audioSource_sync != null))
		if(audioSource_sound !=null)
        {
			audioSource_sound.PlayOneShot(audioClip);
			audioSource_sync.PlayOneShot(audioClip);
        }
	}

	private static float[] ConvertByteToFloat(byte[] array)
	{
		float[] floatArr = new float[array.Length / 2];

		for (int i = 0; i < floatArr.Length; i++)
		{
			floatArr[i] = BitConverter.ToInt16(array, i * 2) / 32768.0f;
		}

		return floatArr;
	}

	public string TextToSpeechPost(object data)
	{
		//str data를 byte로 변환.
		string str = JsonUtility.ToJson(data);
		var bytes = System.Text.Encoding.UTF8.GetBytes(str);

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = bytes.Length;

		try
		{
			//bytes stream 으로 전송.
			using (var stream = request.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
				stream.Flush();
				stream.Close();
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			string json = reader.ReadToEnd();

			return json;

		}
		catch (WebException e)
		{
			Debug.Log(e);
			return null;
		}
	}
}