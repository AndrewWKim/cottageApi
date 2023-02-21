using System;
using System.IO;
using CottageApi.Core.Configurations;

namespace CottageApi.Core.Helpers
{
	public static class FilesHelper
	{
		private static FilesConfig _filesConfig;
		private static string _baseDirectory;

		public static void SetRootUrl(FilesConfig filesConfig, string baseDirectory)
		{
			_filesConfig = filesConfig;
			_baseDirectory = baseDirectory;
		}

		public static string ConvertFileToBase64(string fileName)
		{
			var fullFilePath = GetUserPhotoFullPath(fileName);
			var bytes = File.ReadAllBytes(fullFilePath);
			var fileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1);
			return $"data:image/{fileExtension};base64," + Convert.ToBase64String(bytes);
		}

		public static string GetUserPhotoFullPath(string fileName)
		{
			return string.IsNullOrEmpty(fileName)
				? null
				: GetFileFullPath(fileName, _filesConfig.LogoPath);
		}

		public static string SavePhoto(string fileBase64, string photoExtension, string prevFilePath)
		{
			var fullFolderPath = GetFullPath(_filesConfig.LogoPath);

			CheckCreateDirectory(fullFolderPath);

			DeleteFile(prevFilePath);

			var fileName = $"{Guid.NewGuid().ToString()}.{photoExtension}";
			var filePath = GetFullPath(Path.Combine(_filesConfig.LogoPath, fileName));

			File.WriteAllBytes(filePath, Convert.FromBase64String(fileBase64));

			return fileName;
		}

		public static void CheckCreateDirectory(string fullPath)
		{
			if (!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}
		}

		public static void DeleteFile(string filePath)
		{
			if (!string.IsNullOrWhiteSpace(filePath))
			{
				var correctPath = GetFullPath(Path.Combine(_filesConfig.LogoPath, filePath));

				if (File.Exists(correctPath))
				{
					File.Delete(correctPath);
				}
			}
		}

		private static string GetFileFullPath(string fileName, string folderPath)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return null;
			}

			var path = $"{_baseDirectory}{folderPath.Replace('\\', '/')}/{fileName}";

			return path;
		}

		private static string GetFullPath(string folder)
		{
			return $"{_baseDirectory}{folder}".Replace("\\\\", "\\");
		}
	}
}