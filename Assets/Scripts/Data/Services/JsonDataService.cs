using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class JsonDataService : IDataService
{
	public bool SaveData<T>(string RelativePath, T Data, bool Encrypted)
	{
		string path = Path.Combine(Application.persistentDataPath, RelativePath);

		try
		{
			if (File.Exists(path))
			{
				Debug.Log("Данные существуют. Удаляем старый файл и записываем новый!");
				File.Delete(path);
			}
			else 
			{
				Debug.Log("Файл записан в первый раз!");
			}
			
			using FileStream stream = File.Create(path);
			stream.Close();
			File.WriteAllText(path, JsonConvert.SerializeObject(Data));
			return true;
		}
		catch (Exception e)
		{
			Debug.Log($"Не удалось сохранить данные из-за: {e.Message} {e.StackTrace}");
			return false;
		}
	}
	
	public T LoadData<T>(string RelativePath, bool Encrypted)
	{
		string path = Path.Combine(Application.persistentDataPath, RelativePath);

		if (!File.Exists(path))
		{
			Debug.LogError($"Не получилось загрузить файл по пути {path}. Файл не был найден!");
			throw new FileNotFoundException($"{path} не был найден!");
		}
		
		try 
		{
			T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
			return data;
		}
		catch (Exception e)
		{
			Debug.LogError($"Не получилось загрузить файл из-за: {e.Message}");
			throw e;
		}
	}
	
	public void ClearData(string RelativePath)
	{
		string path = Path.Combine(Application.persistentDataPath, RelativePath);

		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
				Debug.Log($"Файл {RelativePath} успешно удален.");
			}
			else
			{
				Debug.LogWarning($"Файл {RelativePath} не существует.");
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Не удалось удалить файл {RelativePath} из-за: {e.Message}");
		}
	}

}
