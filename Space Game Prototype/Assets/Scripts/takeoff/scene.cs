using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using Unity.VisualScripting;

public class scene : MonoBehaviour
{

    public CinemachineVirtualCamera init_cam, round_cam;
    private PlayableDirector init_anim, round_anim;
    public GameObject rocket, floor;
    private GameObject rocket_flames_container, floor_smoke_container;
    private ParticleSystem[] rocket_flames, floor_smokes;
    private takeoff takeoff_script;

    public bool init_anim_complete = false;

    // Start is called before the first frame update
    void Start()
    {
        init_anim = init_cam.GetComponent<PlayableDirector>();
        round_anim = round_cam.GetComponent<PlayableDirector>();
        takeoff_script = rocket.GetComponent<takeoff>();

        rocket_flames_container = rocket.transform.Find("flames").gameObject;
        floor_smoke_container = floor.transform.Find("smoke").gameObject;

        Debug.Log(rocket_flames_container);

        rocket_flames = rocket_flames_container.GetComponentsInChildren<ParticleSystem>();
        floor_smokes = floor_smoke_container.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem p in rocket_flames) p.Pause();
        foreach (ParticleSystem p in floor_smokes) p.Pause();

        camera_switcher.register(init_cam);
        camera_switcher.register(round_cam);

        camera_switcher.switchCamera(init_cam);

        init_anim.stopped += OnInitAnimStopped;

        init_anim.Play();

    }

    void OnInitAnimStopped(PlayableDirector anim)
    {
        init_anim_complete = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (init_anim_complete)
        {
            camera_switcher.switchCamera(round_cam);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            foreach (ParticleSystem p in rocket_flames) p.Play();
            foreach (ParticleSystem p in floor_smokes) p.Play();
            takeoff_script.launch();
        }
    }
}
