#pragma strict

var ufoMaterial : Material;
private var ufoLightIntensity : float;
private var saveOriginalColor : Color;

function Start()
{
	if(ufoMaterial)
	{	
		ufoMaterial.EnableKeyword("_EMISSION");
		saveOriginalColor=ufoMaterial.GetColor("_EmissionColor");
	}
}

function Update () 
{
	if(ufoMaterial)
	{
		ufoLightIntensity=Mathf.PingPong(Time.time*0.5, 0.8);
		ufoLightIntensity+=0.2;
		ufoMaterial.SetColor("_EmissionColor", Color(ufoLightIntensity, ufoLightIntensity, ufoLightIntensity, 1.0));
	}
}

function OnApplicationQuit()
{
	if(ufoMaterial) ufoMaterial.SetColor("_EmissionColor", saveOriginalColor);
}	