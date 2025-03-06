using Commands;
using HECSFramework.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionStayProviderMonocomponent : MonoBehaviour
{
    public Actor Actor { get; set; }

    public void Start()
    {
        if (Actor == null)
            Actor = GetComponent<Actor>();

        if (Actor == null)
            Actor = GetComponentInParent<Actor>();
    }

    public void StartOnPooling()
    {
        if (Actor == null)
            Actor = GetComponent<Actor>();

        if (Actor == null)
            Actor = GetComponentInParent<Actor>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Actor.Command(new TriggerStay2DCommand { Collider = collision });
    }
}
