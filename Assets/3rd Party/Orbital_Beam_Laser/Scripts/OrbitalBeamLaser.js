
var LaserEffects: GameObject;
var LaserSparks: ParticleSystem;
var LaserSmoke: ParticleSystem;
var LaserChargeAudio: AudioSource;
var LaserAudio: AudioSource;
var LaserStopAudio: AudioSource;
var LaserChargeBeam: GameObject;
var SmokeAndSparks: GameObject;
var ScorchMark: GameObject;

private var ScorchMarkClone;
private var LaserSparksEmitter;
private var LaserSmokeEmitter;
private var Charging;
private var LaserChargeFlag: int = 0;

function Start() {
  // Reset and stop all effects and audio

  LaserEffects.SetActive(false);

  LaserSparksEmitter = LaserSparks.emission;
  LaserSparksEmitter.enabled = false;

  LaserSmokeEmitter = LaserSmoke.emission;
  LaserSmokeEmitter.enabled = false;

  LaserChargeBeam.SetActive(false);
  SmokeAndSparks.SetActive(false);
  SmokeAndSparks.SetActive(true);

  ScorchMarkClone = Instantiate(ScorchMark);

  LaserChargeAudio.Stop();
  LaserAudio.Stop();
  LaserStopAudio.Stop();
}

function Update() {
  // Fire laser when left mouse button is pressed

  if (Input.GetButtonDown("Fire1")&& Charging == null) {
    LaserChargeFlag = 0;
    LaserChargeAudio.Play();
    LaserChargeBeam.SetActive(true);
    Charging = LaserChargeWait();
  }

  // Stop laser if left mouse button is released

  if (Input.GetButtonUp("Fire1")) {
    if(Charging!= null) StopCoroutine(Charging);
    Charging = null;
    LaserChargeFlag = 1;
    LaserEffects.SetActive(false);
    LaserSparksEmitter.enabled = false;
    LaserSmokeEmitter.enabled = false;
    LaserAudio.Stop();
    LaserStopAudio.Play();
    LaserChargeBeam.SetActive(false);
  }
}

function LaserChargeWait() {
  // Wait for laser to charge
  yield WaitForSeconds(1.4);

  if (LaserChargeFlag == 0) {
    LaserEffects.SetActive(true);
    LaserSparksEmitter.enabled = true;
    LaserSmokeEmitter.enabled = true;
    LaserAudio.Play();
    // yield WaitForSeconds (0.2);
    ScorchMark.SetActive(true);
    LaserChargeFlag = 0;
  }

  Charging = null;
}