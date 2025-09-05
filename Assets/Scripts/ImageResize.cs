using UnityEngine;
using System.Collections;
using System.IO;
using Aspose.Imaging;
using UnityEngine.Networking;

public class ImageResize : MonoBehaviour 
{
	private string _convertFloor = "//Planer//ConvertFloor//";
	

	public void Resize(int width, int height, string idRoom, bool isFloor)
	{
		if (isFloor)
		{
			Image image = Image.Load(Directory.GetCurrentDirectory() + "//Planer//PlansFloor//" + idRoom + ".png");
			image.Resize(width, height);
			image.Save(Directory.GetCurrentDirectory() + _convertFloor + idRoom + ".png");
		}
		else
		{
			Image image = Image.Load(Directory.GetCurrentDirectory() + "//Planer//PlansRoom//" + idRoom + ".png");
			image.Resize(width, height);
			image.Save(Directory.GetCurrentDirectory() + "//Planer//ConvertRoom//" + idRoom + ".png");
		}
	}


}