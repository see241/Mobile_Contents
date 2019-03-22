#pragma strict

function Start () {

}

var SpawnedParticle:GameObject;
 
function Update () 
    {
    if (Input.GetButtonDown ("Fire1")) 
        {
        var mousePos = Input.mousePosition;
        mousePos.z = 12.48309;
 
        var objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        Instantiate(SpawnedParticle, objectPos, Quaternion.identity);
        }
    }