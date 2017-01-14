#pragma strict
var fuseLight : Light;
private var fuseLightIntensity : int = 10;

function Start () {

}

function Update () {

    fuseLightIntensity = (Random.Range (5, 14));
    fuseLight.intensity = fuseLightIntensity;

}