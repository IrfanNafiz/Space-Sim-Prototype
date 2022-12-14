using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Manager : MonoBehaviour
{
    public Dictionary<string, string> infoDictionary = new Dictionary<string, string>();
    [SerializeField] GameObject[] planets;

    [SerializeField] GameObject ParkerSolarProbe;

    CinemachineVirtualCamera activeCam = null;

    public bool IsPaused = false;
    private int ConstantTimeScale = 100;

    public TMP_Text camText;
    public TMP_Text ProbeText;
    public TMP_Text BreakText;
    public TMP_Text InformationText;

    public GameObject CameraPerson;
    private CameraPerson CameraPersonScript;
    public GameObject gm;

    public Vector3 probeTextOffset;

    private bool IsBreaking = false;
    private bool InspectionMode = false;

    public int FinalShotIndex = 1;
    public int BreakAfter = 4;

    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        CameraPersonScript = CameraPerson.GetComponent<CameraPerson>();
    }

    void SetUI(CinemachineVirtualCamera active){
        camText.text = active.GetComponent<CameraProps>().CameraName;
        
        if (active.GetComponent<CameraProps>().AllowProbeText && MouseOverProbe() == "Probe"){
            Vector3 probePos = Camera.main.WorldToScreenPoint(ParkerSolarProbe.transform.position);
            ProbeText.transform.position = probePos + probeTextOffset;
            double velocity = ParkerSolarProbe.GetComponent<SimpleKeplerOrbits.KeplerOrbitMover>().OrbitData.Velocity.x;
            ProbeText.text = "Velocity: " + Mathf.Abs(Mathf.Round((float)velocity * (float)(692000/1.54398) * 10000f) * (1/10000f)).ToString() + " km/h";
        } else {
            ProbeText.text = "";
        }

        if (IsBreaking) {
            BreakText.text = "Press B to destruct Probe";
        }

        if(MouseOverProbe() != "Probe" && MouseOverProbe() != "None"){
            InformationText.text = MouseOverProbe();
        } else {
            InformationText.text = "";
        }

    }

    string MouseOverProbe(){
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            return hit.collider.name;
        }
        return "None";
    }

    public void SwitchToFinalShot() {
        CameraPersonScript.ActiveCamIndex = FinalShotIndex;
        CameraSwitcher.SwitchCam(CameraPersonScript.cameras[FinalShotIndex]);
        IsBreaking = true;
    }

    IEnumerator EndTrigger(){
        yield return new WaitForSeconds(5f);
        Debug.Log("Should End Now");
        gm.GetComponent<GameManager>().StartGameTransition();
    }

    // Update is called once per frame
    void Update()
    {
        var destroyable = FindObjectsOfType<MeshDestroy>();
        if(destroyable.Length > 0){
            if(MeshDestroy.BreakCounter >= BreakAfter){
			    StartCoroutine(EndTrigger());
            }
        }
        Debug.Log(MouseOverProbe());
        // if(InspectionMode){
        //     IsPaused = true;
        //     MainCamera.enabled = false;
        // } else {
        //     MainCamera.enabled = true;
        // }
        //IsBreaking = ParkerSolarProbe.GetComponent<SimpleKeplerOrbits.KeplerOrbitMover>().IsBreaking;
        activeCam = CameraPersonScript.cameras[CameraPersonScript.ActiveCamIndex];
        SetUI(activeCam);
        if(CameraPersonScript.ActiveCamIndex == 4) {
            if(Input.GetKey(KeyCode.D)){
                activeCam.transform.Rotate(0.0f, 1.0f, 0.0f, Space.Self);
            }
            if(Input.GetKey(KeyCode.A)){
                activeCam.transform.Rotate(0.0f, -1.0f, 0.0f, Space.Self);
            }
            if(Input.GetKey(KeyCode.S)){
                activeCam.transform.Rotate(1.0f, 0.0f, 0.0f, Space.Self);
            }
            if(Input.GetKey(KeyCode.W)){
                activeCam.transform.Rotate(-1.0f, 0.0f, 0.0f, Space.Self);
            }
        }
        foreach(GameObject planet in planets) {
            if(IsBreaking) {
                continue;
            } else {
                if(IsPaused) {
                    planet.GetComponent<SimpleKeplerOrbits.KeplerOrbitMover>().TimeScale = 0f;
                } else {
                    planet.GetComponent<SimpleKeplerOrbits.KeplerOrbitMover>().TimeScale = ConstantTimeScale;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            IsPaused = !IsPaused;
        }

        if(!activeCam.GetComponent<CameraProps>().AllowProbeText) {
            ParkerSolarProbe.GetComponent<SphereCollider>().enabled = false;
        } else {
            ParkerSolarProbe.GetComponent<SphereCollider>().enabled = true;
        }
    }
}
